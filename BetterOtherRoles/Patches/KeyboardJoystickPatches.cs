using HarmonyLib;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(KeyboardJoystick))]
public static class KeyboardJoystickPatches
{
    [HarmonyPatch(nameof(KeyboardJoystick.HandleHud))]
    [HarmonyPrefix]
    private static bool HandleHudPrefix(KeyboardJoystick __instance)
    {
        if (!DestroyableSingleton<HudManager>.InstanceExists) return false;
        if (KeyboardJoystick.player.GetButtonDown(RewiredConsts.Action.ActionTertiary))
            DestroyableSingleton<HudManager>.Instance.ReportButton.DoClick();
        if (KeyboardJoystick.player.GetButtonDown(RewiredConsts.Action.ActionPrimary))
            DestroyableSingleton<HudManager>.Instance.UseButton.DoClick();
        if (KeyboardJoystick.player.GetButtonDown(RewiredConsts.Action.ToggleMap) && !DestroyableSingleton<HudManager>.Instance.Chat.IsOpenOrOpening)
            DestroyableSingleton<HudManager>.Instance.ToggleMapVisible(GameManager.Instance.GetMapOptions());
        if (KeyboardJoystick.player.GetButtonDown(RewiredConsts.Action.ActionQuaternary))
            DestroyableSingleton<HudManager>.Instance.AbilityButton.DoClick();
        if (PlayerControl.LocalPlayer.Data == null) return false;
        if (PlayerControl.LocalPlayer.Data.Role.IsImpostor && KeyboardJoystick.player.GetButtonDown(RewiredConsts.Action.ActionSecondary))
            DestroyableSingleton<HudManager>.Instance.KillButton.DoClick();
        if (!PlayerControl.LocalPlayer.roleCanUseVents() || !KeyboardJoystick.player.GetButtonDown(RewiredConsts.Action.UseVent)) return false;
        DestroyableSingleton<HudManager>.Instance.ImpostorVentButton.DoClick();
        return false;
    }
}