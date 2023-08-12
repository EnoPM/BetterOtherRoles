using BetterOtherRoles.Modules;
using HarmonyLib;
using Il2CppSystem.Text;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(UploadDataTask))]
public static class UploadDataTaskPatches
{
    [HarmonyPatch(nameof(UploadDataTask.AppendTaskText))]
    [HarmonyPrefix]
    private static bool AppendTaskTextPrefix(UploadDataTask __instance, StringBuilder sb)
    {
        if (__instance.taskStep > 0)
        {
            if (__instance.IsComplete)
            {
                sb.Append("<color=#00DD00FF>");
            }

            else
            {
                sb.Append("<color=#FFFF00FF>");
            }
        }

        var room = __instance.taskStep == 0 ? __instance.StartAt : __instance.EndAt;
        if (TaskPositionsRandomizer.RelocatedDownloads.TryGetValue(room, out var newRoom))
        {
            room = newRoom;
        }

        sb.Append(DestroyableSingleton<TranslationController>.Instance.GetString(room));
        sb.Append(": ");
        sb.Append(DestroyableSingleton<TranslationController>.Instance.GetString(__instance.taskStep == 0
            ? StringNames.DownloadData
            : StringNames.UploadData));
        sb.Append(" (");
        sb.Append(__instance.taskStep);
        sb.Append("/");
        sb.Append(__instance.MaxStep);
        sb.AppendLine(") </color>");
        return false;
    }
}