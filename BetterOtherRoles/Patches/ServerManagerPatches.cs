using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Unity.IL2CPP.Utils;
using BetterOtherRoles.Modules;
using BetterOtherRoles.Utilities.Extensions;
using HarmonyLib;
using Innersloth.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(ServerManager))]
public static class ServerManagerPatches
{
    private static readonly string RegionFileJson = Path.Combine(Paths.PluginPath, "BetterOtherRoles", "Regions.json");

    [HarmonyPatch(nameof(ServerManager.LoadServers))]
    [HarmonyPrefix]
    private static bool LoadServersPrefix(ServerManager __instance)
    {
        if (FileIO.Exists(RegionFileJson))
        {
            try
            {
                var jsonServerData = JsonConvert.DeserializeObject<ServerManager.JsonServerData>(
                    FileIO.ReadAllText(RegionFileJson), new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    });
                jsonServerData.CleanAndMerge(CustomRegions.DefaultRegions);
                __instance.AvailableRegions = jsonServerData.Regions;
                __instance.CurrentRegion =
                    __instance.AvailableRegions[
                        jsonServerData.CurrentRegionIdx.Wrap(__instance.AvailableRegions.Length)];
                __instance.CurrentUdpServer = __instance.CurrentRegion.Servers.ToList().GetOneRandom();
                __instance.state = UpdateState.Success;
                __instance.SaveServers();
            }
            catch (Exception ex)
            {
                BetterOtherRolesPlugin.Logger.LogWarning(ex);
                __instance.StartCoroutine(ReselectRegionFromDefaults());
            }
        }
        else
        {
            __instance.StartCoroutine(ReselectRegionFromDefaults());
        }

        return false;
    }

    [HarmonyPatch(nameof(ServerManager.SaveServers))]
    [HarmonyPrefix]
    private static bool SaveServersPrefix(ServerManager __instance)
    {
        try
        {
            FileIO.WriteAllText(RegionFileJson, JsonConvert.SerializeObject(new ServerManager.JsonServerData
            {
                CurrentRegionIdx = __instance.AvailableRegions.ToList()
                    .FindIndex(r => r.Name == __instance.CurrentRegion.Name),
                Regions = __instance.AvailableRegions
            }, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            }));
        }
        catch
        {
        }

        return false;
    }

    private static IEnumerator ReselectRegionFromDefaults()
    {
        if (!ServerManager.InstanceExists) yield break;
        var sm = ServerManager.Instance;
        sm.AvailableRegions = CustomRegions.DefaultRegions;
        var dnsLookup = CustomRegions.DefaultRegions.Select(r => Dns.GetHostAddressesAsync(r.PingServer)).ToList();
        while (dnsLookup.Any(task => !task.IsCompleted))
        {
            yield return null;
        }

        var pings = new List<ServerManager.PingWrapper>();
        for (var index = 0; index < CustomRegions.DefaultRegions.Length; index++)
        {
            var defaultRegion = CustomRegions.DefaultRegions[index];
            IPAddress[] result;
            try
            {
                result = dnsLookup[index].ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                BetterOtherRolesPlugin.Logger.LogWarning(ex);
                continue;
            }

            if (result == null || result.Length == 0)
            {
                BetterOtherRolesPlugin.Logger.LogWarning("DNS - no IPs resolved for " + defaultRegion.PingServer);
            }
            else
            {
                pings.Add(new ServerManager.PingWrapper(defaultRegion, new Ping(result.ToList().GetOneRandom().ToString())));
            }
        }

        for (var timeElapsedSeconds = 0f; pings.Count > 0 && timeElapsedSeconds < 5f && !pings.Any(p => p.Ping.isDone && p.Ping.time >= 0); timeElapsedSeconds += Time.deltaTime)
        {
            yield return null;
        }

        var regionInfo = CustomRegions.DefaultRegions.First();
        var num = int.MaxValue;
        foreach (var pingWrapper in pings)
        {
            if (pingWrapper.Ping.isDone && pingWrapper.Ping.time >= 0)
            {
                if (pingWrapper.Ping.time < num)
                {
                    regionInfo = pingWrapper.Region;
                    num = pingWrapper.Ping.time;
                }
            }
            pingWrapper.Ping.DestroyPing();
        }
        
        sm.CurrentRegion = regionInfo.Duplicate();
        sm.ReselectServer();
        sm.SaveServers();
    }
}