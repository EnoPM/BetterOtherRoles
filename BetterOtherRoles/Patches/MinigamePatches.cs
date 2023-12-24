using System;
using System.Collections;
using System.Collections.Generic;
using AmongUsSpecimen;
using BepInEx.Unity.IL2CPP.Utils;
using HarmonyLib;
using UnityEngine;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(Minigame))]
internal static class MinigamePatches
{
    private static readonly List<PlayerControl> WalkingToTask = [];
    
    [Rpc]
    private static void RpcHalt(this PlayerControl __sender, Vector3 position)
    {
        __sender.StartCoroutine(CoWalkToTask(__sender, position));
    }

    internal static bool IsWalkingToTask(this PlayerControl player) => WalkingToTask.Contains(player);
    internal static void SetWalkingToTask(this PlayerControl player, bool walkingToTask)
    {
        var existing = WalkingToTask.Contains(player);
        if (walkingToTask && !existing)
        {
            WalkingToTask.Add(player);
        }
        else if (!walkingToTask && existing)
        {
            WalkingToTask.Remove(player);
        }
    }

    private static IEnumerator CoWalkToTask(this PlayerControl player, Vector3 position)
    {
        const float tolerance = 0.01f;
        player.NetTransform.SetPaused(true);
        if (player.AmOwner) player.MyPhysics.inputHandler.enabled = true;
        player.SetWalkingToTask(true);
        player.moveable = false;
        var playerPos = player.transform.position;
        if (Math.Abs(playerPos.x - position.x) > tolerance || Math.Abs(playerPos.y - position.y) > tolerance || Math.Abs(playerPos.z - position.z) > tolerance)
        {
            yield return player.MyPhysics.WalkPlayerTo(position, tolerance);
        }
        // var minSid = (ushort)(player.NetTransform.lastSequenceId + 1U);
        // player.NetTransform.SnapTo(position, minSid);
        player.SetWalkingToTask(false);
        player.NetTransform.SetPaused(false);
        player.moveable = true;
        player.NetTransform.body.velocity = Vector2.zero;
    }
    
    [HarmonyPatch(nameof(Minigame.Begin)), HarmonyPrefix]
    private static bool BeginPrefix(Minigame __instance, PlayerTask task)
    {
        Minigame.Instance = __instance;
        __instance.MyTask = task;
        __instance.MyNormTask = task as NormalPlayerTask;
        __instance.timeOpened = Time.realtimeSinceStartup;
        if (PlayerControl.LocalPlayer)
        {
            if (MapBehaviour.Instance) MapBehaviour.Instance.Close();
            PlayerControl.LocalPlayer.moveable = false;
            //PlayerControl.LocalPlayer.NetTransform.HaltForTask();
            PlayerControl.LocalPlayer.RpcHalt(PlayerControl.LocalPlayer.transform.position);
        }
        __instance.logger.Info($"Opening minigame {__instance.GetType().Name}");
        __instance.StartCoroutine(__instance.CoAnimateOpen());
        DestroyableSingleton<DebugAnalytics>.Instance.Analytics.MinigameOpened(PlayerControl.LocalPlayer.Data, __instance.TaskType);
        return false;
    }
    
    /*

    private static void HaltForTask(this CustomNetworkTransform netTransform)
    {
        var minSid = (ushort) (netTransform.lastSequenceId + 1U);
        netTransform.SnapToForTask(netTransform.transform.position, minSid);
    }

    private static void SnapToForTask(this CustomNetworkTransform netTransform)
    {
        
    }
    
    */
}