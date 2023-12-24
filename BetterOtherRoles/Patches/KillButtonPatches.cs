using BetterOtherRoles.Modifiers;
using BetterOtherRoles.Players;
using BetterOtherRoles.Roles;
using HarmonyLib;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(KillButton))]
internal static class KillButtonPatches
{
    [HarmonyPatch(nameof(KillButton.DoClick)), HarmonyPrefix]
    private static bool DoClickPrefix(KillButton __instance)
    {
        if (! __instance.isActiveAndEnabled || !__instance.currentTarget || __instance.isCoolingDown || PlayerControl.LocalPlayer.Data.IsDead || !PlayerControl.LocalPlayer.CanMove) return false;
        if (Deputy.handcuffedPlayers.Contains(CachedPlayer.LocalPlayer.PlayerId))
        {
            Deputy.setHandcuffedKnows();
            return false;
        }
        var localPlayer = CachedPlayer.LocalPlayer.PlayerControl;
        var res = Helpers.checkMurderAttemptAndKill(localPlayer, __instance.currentTarget);
        if (res == MurderAttemptResult.BlankKill)
        {
            localPlayer.killTimer = GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown;
            if (localPlayer == Cleaner.cleaner)
            {
                Cleaner.cleaner.killTimer = HudManagerStartPatch.cleanerCleanButton.Timer = HudManagerStartPatch.cleanerCleanButton.MaxTimer;
            }
            else if (localPlayer == Warlock.warlock)
            {
                Warlock.warlock.killTimer = HudManagerStartPatch.warlockCurseButton.Timer = HudManagerStartPatch.warlockCurseButton.MaxTimer;
            }
                
            else if (localPlayer == Mini.mini && Mini.mini.Data.Role.IsImpostor)
            {
                Mini.mini.SetKillTimer(GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown * (Mini.isGrownUp() ? 0.66f : 2f));
            }
            else if (localPlayer == Witch.witch)
            {
                Witch.witch.killTimer = HudManagerStartPatch.witchSpellButton.Timer = HudManagerStartPatch.witchSpellButton.MaxTimer;
            }
            else if (localPlayer == Ninja.ninja)
            {
                Ninja.ninja.killTimer = HudManagerStartPatch.ninjaButton.Timer = HudManagerStartPatch.ninjaButton.MaxTimer;
            }
            else if (StickyBomber.TriggerBothCooldown && localPlayer == StickyBomber.Player)
            {
                localPlayer.killTimer = HudManagerStartPatch.stickyBomberButton.Timer = HudManagerStartPatch.stickyBomberButton.MaxTimer;
            }
        }
        BetterOtherRolesPlugin.Logger.LogMessage($"DoClickPrefix: {res.ToString()}");
        // PlayerControl.LocalPlayer.CmdCheckMurder(__instance.currentTarget);
        __instance.SetTarget(null);
        return false;
    }
}