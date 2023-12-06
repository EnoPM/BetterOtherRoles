using System.Collections.Generic;
using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Eraser
{
    public static PlayerControl eraser;
    public static Color color = Palette.ImpostorRed;

    public static List<byte> alreadyErased = new List<byte>();

    public static List<PlayerControl> futureErased = new List<PlayerControl>();
    public static PlayerControl currentTarget;
    public static float cooldown = 30f;
    public static bool canEraseAnyone = false;

    private static Sprite buttonSprite;

    public static Sprite getButtonSprite()
    {
        if (buttonSprite) return buttonSprite;
        buttonSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.EraserButton.png", 115f);
        return buttonSprite;
    }

    public static void clearAndReload()
    {
        eraser = null;
        futureErased = new List<PlayerControl>();
        currentTarget = null;
        cooldown = CustomOptionHolder.EraserCooldown.GetFloat();
        canEraseAnyone = CustomOptionHolder.EraserCanEraseAnyone.GetBool();
        alreadyErased = new List<byte>();
    }
}