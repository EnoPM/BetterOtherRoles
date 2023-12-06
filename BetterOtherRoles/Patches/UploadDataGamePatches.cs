using BetterOtherRoles.Modules;
using BetterOtherRoles.Options;
using HarmonyLib;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(UploadDataGame))]
public static class UploadDataGamePatches
{
    [HarmonyPatch(nameof(UploadDataGame.Begin))]
    [HarmonyPostfix]
    private static void BeginPostfix(UploadDataGame __instance, PlayerTask task)
    {
        if (!CustomOptionHolder.RandomizeUploadTaskPosition.GetBool()) return;
        if (__instance.MyNormTask.taskStep == 0 && TaskPositionsRandomizer.RelocatedDownloads.TryGetValue(__instance.MyNormTask.StartAt, out var room))
        {
            __instance.SourceText.text = DestroyableSingleton<TranslationController>.Instance.GetString(room);
        }
    }
}