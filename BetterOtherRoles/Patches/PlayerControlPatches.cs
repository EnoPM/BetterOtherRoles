using HarmonyLib;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(PlayerControl))]
internal static class PlayerControlPatches
{
    [HarmonyPatch(nameof(PlayerControl.CmdCheckMurder)), HarmonyPrefix]
    private static bool CmdCheckMurderPrefix(PlayerControl __instance, PlayerControl target)
    {
        BetterOtherRolesPlugin.Logger.LogMessage($"CmdCheckMurderPrefix: {(AmongUsClient.Instance.AmLocalHost || AmongUsClient.Instance.AmModdedHost ? "yes": "no")}");
        return true;
    }
}