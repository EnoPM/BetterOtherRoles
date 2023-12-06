using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using BepInEx.Configuration;
using UnityEngine;

namespace BetterOtherRoles.Modules;

public static class DevConfig
{
    private static ConfigEntry<string> DingusModeKey { get; set; }
    public static bool DisableEndGameConditions { get; set; }
    public static bool DisablePlayerRequirementToLaunch { get; set; }
    public static Guid CurrentGuid { get; set; }
    public static bool IsDingusRelease { get; set; }

    public static Dictionary<string, string> LocalFlags { get; set; }
    public static Dictionary<string, string> Flags { get; set; }

    static DevConfig()
    {
        DingusModeKey = BetterOtherRolesPlugin.Instance.Config.Bind("Special Edition Flags", "Dingus", "Unknown", "Password to unlock Dingus special edition");
        if (DingusModeKey.Value != string.Empty)
        {
            IsDingusRelease = Encrypt(DingusModeKey.Value) == "310fa3717f2b746dc22bb95470ae458e2a07322ee8e631007b670fb34baa8913";
        }
#if RELEASE
        DisableEndGameConditions = false;
        DisablePlayerRequirementToLaunch = false;
        CurrentGuid = Assembly.GetExecutingAssembly().ManifestModule.ModuleVersionId;
        LocalFlags = new Dictionary<string, string>() { { "UNLOCK_ALL_COSMETICS", "1" }, { "SHOW_GHOST_INFOS", "1" } };
        Flags = new Dictionary<string, string>() { { "NO_GUID_CHECK", "1" }, { "LOBBY_NAME_COLOR", "rainbow" } };
#else
        DisableEndGameConditions = false;
        DisablePlayerRequirementToLaunch = true;
        CurrentGuid = Assembly.GetExecutingAssembly().ManifestModule.ModuleVersionId;
        LocalFlags = new Dictionary<string, string>() { { "UNLOCK_ALL_COSMETICS", "1" }, { "SHOW_GHOST_INFOS", "1" } };
        Flags = new Dictionary<string, string> () { { "NO_GUID_CHECK", "1" } };
#endif
    }

    public static bool HasFlag(string name, string value)
    {
        if (LocalFlags.TryGetValue(name, out var val1))
        {
            return value == val1;
        }
        if (Flags.TryGetValue(name, out var val2))
        {
            return value == val2;
        }

        return false;
    }
    
    public static bool HasFlag(string name) => LocalFlags.ContainsKey(name) || Flags.ContainsKey(name);

    public static string Encrypt(string entry)
    {
        var sb = new StringBuilder();
        var sha = SHA256.Create();
        var hashed = sha.ComputeHash(Encoding.UTF8.GetBytes(entry));
        foreach (var b in hashed)
        {
            sb.Append(b.ToString("x2"));
        }

        return sb.ToString();
    }

    public static void LogTransform(Transform transform, string prefix = "")
    {
        var components = transform.gameObject.GetComponents(Il2CppType.From(typeof(Il2CppSystem.ComponentModel.Component)));
        System.Console.WriteLine($"[LogGameObject]{prefix} {transform.name} ({transform.position}) ({components.Count} components)");
        foreach (var component in components)
        {
            System.Console.WriteLine($"[LogComponent]{prefix.Replace("-", ">")} {component.ToString()}");
        }
        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            LogTransform(child, prefix + "-");
        }
    }
}