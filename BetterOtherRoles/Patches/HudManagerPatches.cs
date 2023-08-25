using BetterOtherRoles.Modules;
using BetterOtherRoles.UI;
using HarmonyLib;
using InnerNet;

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

    private static InnerNetClient.GameStates _lastGameState = InnerNetClient.GameStates.NotJoined;

    [HarmonyPatch(nameof(HudManager.Update))]
    [HarmonyPostfix]
    private static void UpdatePostfix()
    {
        if(!AmongUsClient.Instance) return;
        if (AmongUsClient.Instance.GameState == _lastGameState) return;
        _lastGameState = AmongUsClient.Instance.GameState;
        OnGameStateUpdated();
    }

    private static void OnGameStateUpdated()
    {
        UIManager.CloseAllPanels();
    }
}