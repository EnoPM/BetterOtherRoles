using System.Collections.Generic;
using BetterOtherRoles.Modules;
using HarmonyLib;
using UnityEngine;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(MeetingHud))]
public static class MeetingHudPatches
{
    [HarmonyPatch(nameof(MeetingHud.Start))]
    [HarmonyPostfix]
    private static void StartPostfix(MeetingHud __instance)
    {
        GameEvents.TriggerMeetingStarted();
    }
}