using BetterOtherRoles.Modules;
using HarmonyLib;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(HudManager))]
public static class HudManagerPatches
{
    [HarmonyPatch(nameof(HudManager.OnGameStart))]
    [HarmonyPostfix]
    private static void OnGameStartPostfix(HudManager __instance)
    {
        GameEvents.TriggerGameStarted();
    }
}