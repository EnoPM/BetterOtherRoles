using BetterOtherRoles.Modules;
using HarmonyLib;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(TutorialManager))]
public class TutorialManagerPatches
{
    [HarmonyPatch(nameof(TutorialManager.RunTutorial))]
    [HarmonyPostfix]
    private static void RunTutorialPostfix()
    {
        GameEvents.TriggerGameStarted();
    }
}