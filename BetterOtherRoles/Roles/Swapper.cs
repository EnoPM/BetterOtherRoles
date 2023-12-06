using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Swapper
{
    public static PlayerControl swapper;
    public static Color color = new Color32(134, 55, 86, byte.MaxValue);
    private static Sprite spriteCheck;
    public static bool canCallEmergency = false;
    public static bool canOnlySwapOthers = false;
    public static int charges;
    public static float rechargeTasksNumber;
    public static float rechargedTasks;

    public static byte playerId1 = byte.MaxValue;
    public static byte playerId2 = byte.MaxValue;

    public static Sprite getCheckSprite()
    {
        if (spriteCheck) return spriteCheck;
        spriteCheck = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.SwapperCheck.png", 150f);
        return spriteCheck;
    }

    public static void clearAndReload()
    {
        swapper = null;
        playerId1 = byte.MaxValue;
        playerId2 = byte.MaxValue;
        canCallEmergency = CustomOptionHolder.SwapperCanCallEmergency.GetBool();
        canOnlySwapOthers = CustomOptionHolder.SwapperCanOnlySwapOther.GetBool();
        charges = Mathf.RoundToInt(CustomOptionHolder.SwapperSwapsNumber.GetFloat());
        rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.SwapperRechargeTasksNumber.GetFloat());
        rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.SwapperRechargeTasksNumber.GetFloat());
    }
}