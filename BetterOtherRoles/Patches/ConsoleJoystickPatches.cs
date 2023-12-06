using HarmonyLib;
using UnityEngine;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(ConsoleJoystick))]
public static class ConsoleJoystickPatches
{
    [HarmonyPatch(nameof(ConsoleJoystick.HandleHUD))]
    [HarmonyPrefix]
    private static bool HandleHUDPrefix(ConsoleJoystick __instance)
    {
        if (PlayerControl.LocalPlayer)
        {
            var canMove = PlayerControl.LocalPlayer.CanMove;
            if (!canMove && ConsoleJoystick.inputState == ConsoleJoystick.ConsoleInputState.Gameplay)
            {
                ConsoleJoystick.SetMode_Menu();
                return false;
            }

            if (canMove && ConsoleJoystick.inputState != ConsoleJoystick.ConsoleInputState.Sabotage &&
                ConsoleJoystick.inputState != ConsoleJoystick.ConsoleInputState.Gameplay)
            {
                ConsoleJoystick.SetMode_Gameplay();
                return false;
            }
        }

        if (ConsoleJoystick.inputState == ConsoleJoystick.ConsoleInputState.Vent)
        {
            if (!ControllerManager.Instance.IsUiControllerActive)
            {
                if (!Vent.currentVent)
                {
                    ConsoleJoystick.SetMode_Gameplay();
                }
                else
                {
                    var flag = false;
                    if (__instance.delta.sqrMagnitude > 0.25)
                    {
                        flag = true;
                        var normalized1 = __instance.delta.normalized;
                        var num1 = float.NegativeInfinity;
                        var num2 = -1;
                        for (var index = 0; index < Vent.currentVent.Buttons.Length; ++index)
                        {
                            if (!Vent.currentVent.Buttons[index].isActiveAndEnabled) continue;
                            Vector2 vector2 = Vent.currentVent.Buttons[index].transform.localPosition;
                            var normalized2 = vector2.normalized;
                            var num3 = Vector2.Dot(normalized1, normalized2);
                            if (num2 != -1 && num3 <= num1) continue;
                            num1 = num3;
                            num2 = index;
                        }

                        if (num1 > 0.699999988079071)
                        {
                            if (ConsoleJoystick.highlightedVentIndex != -1 && ConsoleJoystick.highlightedVentIndex <
                                Vent.currentVent.Buttons.Length)
                            {
                                Vent.currentVent.Buttons[ConsoleJoystick.highlightedVentIndex].spriteRenderer.color =
                                    Color.white;
                                ConsoleJoystick.highlightedVentIndex = -1;
                            }

                            ConsoleJoystick.highlightedVentIndex = num2;
                            Vent.currentVent.Buttons[ConsoleJoystick.highlightedVentIndex].spriteRenderer.color =
                                Color.red;
                        }
                        else if (ConsoleJoystick.highlightedVentIndex != -1 &&
                                 ConsoleJoystick.highlightedVentIndex < Vent.currentVent.Buttons.Length)
                        {
                            Vent.currentVent.Buttons[ConsoleJoystick.highlightedVentIndex].spriteRenderer.color =
                                Color.white;
                            ConsoleJoystick.highlightedVentIndex = -1;
                        }
                    }
                    else if (ConsoleJoystick.highlightedVentIndex != -1 && ConsoleJoystick.highlightedVentIndex < Vent.currentVent.Buttons.Length)
                    {
                        Vent.currentVent.Buttons[ConsoleJoystick.highlightedVentIndex].spriteRenderer.color = Color.white;
                        ConsoleJoystick.highlightedVentIndex = -1;
                    }
                    if (!flag)
                    {
                        if (ConsoleJoystick.player.GetButtonDown(RewiredConsts.Action.ActionPrimary))
                            DestroyableSingleton<HudManager>.Instance.UseButton.DoClick();
                        if (ConsoleJoystick.player.GetButtonDown(RewiredConsts.Action.ActionQuaternary))
                            DestroyableSingleton<HudManager>.Instance.AbilityButton.DoClick();
                        if (PlayerControl.LocalPlayer && PlayerControl.LocalPlayer.roleCanUseVents() && ConsoleJoystick.player.GetButtonDown(RewiredConsts.Action.UseVent))
                            DestroyableSingleton<HudManager>.Instance.ImpostorVentButton.DoClick();
                    }
                    else if (ConsoleJoystick.highlightedVentIndex != -1 && ConsoleJoystick.player.GetButtonDown(RewiredConsts.Action.ActionPrimary))
                    {
                        Vent.currentVent.Buttons[ConsoleJoystick.highlightedVentIndex].spriteRenderer.color = Color.white;
                        Vent.currentVent.Buttons[ConsoleJoystick.highlightedVentIndex].OnClick.Invoke();
                        ConsoleJoystick.highlightedVentIndex = -1;
                    }
                }
            }
        }
        else
        {
            if (ConsoleJoystick.player.GetButtonDown(RewiredConsts.Action.ActionQuaternary))
                DestroyableSingleton<HudManager>.Instance.AbilityButton.DoClick();
            if (ConsoleJoystick.player.GetButtonDown(RewiredConsts.Action.ActionTertiary))
                DestroyableSingleton<HudManager>.Instance.ReportButton.DoClick();
            if (ConsoleJoystick.player.GetButtonDown(RewiredConsts.Action.ActionPrimary))
                DestroyableSingleton<HudManager>.Instance.UseButton.DoClick();
            if (PlayerControl.LocalPlayer.Data != null && PlayerControl.LocalPlayer.Data.Role != null)
            {
                if (PlayerControl.LocalPlayer.Data.Role.IsImpostor && ConsoleJoystick.player.GetButtonDown(RewiredConsts.Action.ActionSecondary))
                    DestroyableSingleton<HudManager>.Instance.KillButton.DoClick();
                if (PlayerControl.LocalPlayer.roleCanUseVents() && ConsoleJoystick.player.GetButtonDown(RewiredConsts.Action.UseVent))
                    DestroyableSingleton<HudManager>.Instance.ImpostorVentButton.DoClick();
            }
        }
        if (ConsoleJoystick.player.GetButtonDown(RewiredConsts.Action.ToggleTasks) && DestroyableSingleton<HudManager>.InstanceExists)
            DestroyableSingleton<HudManager>.Instance.TaskPanel.ToggleOpen();
        if (ConsoleJoystick.player.GetButtonDown(RewiredConsts.Action.Pause))
        {
            if (DestroyableSingleton<HudManager>.Instance.GameMenu.IsOpen)
                DestroyableSingleton<HudManager>.Instance.GameMenu.Close();
            else
                DestroyableSingleton<HudManager>.Instance.GameMenu.Open();
        }
        if (ConsoleJoystick.player.GetButtonDown(RewiredConsts.Action.ToggleMap))
        {
            if (ConsoleJoystick.inputState == ConsoleJoystick.ConsoleInputState.Sabotage)
            {
                ConsoleJoystick.SetMode_Gameplay();
                if (MapBehaviour.Instance)
                    MapBehaviour.Instance.Close();
            }
            else
                DestroyableSingleton<HudManager>.Instance.ToggleMapVisible(GameManager.Instance.GetMapOptions());
        }
        if (ConsoleJoystick.player.GetButtonDown(RewiredConsts.Action.MenuLT) && ConsoleJoystick.inputState == ConsoleJoystick.ConsoleInputState.Menu && MeetingHud.Instance)
            DestroyableSingleton<HudManager>.Instance.ToggleMapVisible(new MapOptions()
            {
                Mode = MapOptions.Modes.Normal
            });
        if (!ConsoleJoystick.player.GetButtonDown(RewiredConsts.Action.MenuCancel) || ControllerManager.Instance.IsUiControllerActive)
            return false;
        if (Minigame.Instance)
            Minigame.Instance.Close();
        else if (MapBehaviour.Instance)
            MapBehaviour.Instance.Close();
        if (Vent.currentVent)
        {
            ConsoleJoystick.SetMode_Vent();
        }
        else
        {
            if (ConsoleJoystick.inputState == ConsoleJoystick.ConsoleInputState.Gameplay)
                return false;
            ConsoleJoystick.SetMode_Gameplay();
        }
        return false;
    }
}