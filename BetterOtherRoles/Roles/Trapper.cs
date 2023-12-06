using System.Collections.Generic;
using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Trapper
{
    public static PlayerControl trapper;
    public static Color color = new Color32(110, 57, 105, byte.MaxValue);

    public static float cooldown = 30f;
    public static int maxCharges = 5;
    public static int rechargeTasksNumber = 3;
    public static int rechargedTasks = 3;
    public static int charges = 1;
    public static int trapCountToReveal = 2;
    public static List<PlayerControl> playersOnMap = new List<PlayerControl>();
    public static bool anonymousMap = false;
    public static int infoType = 0; // 0 = Role, 1 = Good/Evil, 2 = Name
    public static float trapDuration = 5f;

    private static Sprite trapButtonSprite;

    public static Sprite getButtonSprite()
    {
        if (trapButtonSprite) return trapButtonSprite;
        trapButtonSprite =
            Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.Trapper_Place_Button.png", 115f);
        return trapButtonSprite;
    }

    public static void clearAndReload()
    {
        trapper = null;
        cooldown = CustomOptionHolder.TrapperCooldown.GetFloat();
        maxCharges = Mathf.RoundToInt(CustomOptionHolder.TrapperMaxCharges.GetFloat());
        rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.TrapperRechargeTasksNumber.GetFloat());
        rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.TrapperRechargeTasksNumber.GetFloat());
        charges = Mathf.RoundToInt(CustomOptionHolder.TrapperMaxCharges.GetFloat()) / 2;
        trapCountToReveal = Mathf.RoundToInt(CustomOptionHolder.TrapperTrapNeededTriggerToReveal.GetFloat());
        playersOnMap = new List<PlayerControl>();
        anonymousMap = CustomOptionHolder.TrapperAnonymousMap.GetBool();
        infoType = CustomOptionHolder.TrapperInfoType.CurrentSelection;
        trapDuration = CustomOptionHolder.TrapperTrapDuration.GetFloat();
    }
}