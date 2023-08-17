using System.Linq;
using BetterOtherRoles.Modules;
using HarmonyLib;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(SpecimenGame))]
public static class SpecimenGamePatches
{
    [HarmonyPatch(nameof(SpecimenGame.Begin))]
    [HarmonyPostfix]
    private static void BeginPostfix(SpecimenGame __instance)
    {
        if (!DevConfig.IsDingusRelease) return;
        var spriteRenderer = __instance.SpecimenSprites.FirstOrDefault(sr => sr.name == "panel_speciment_undertalesans");
        if (spriteRenderer == null) return;
        spriteRenderer.sprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.dfg.png", 500f);
    }
}