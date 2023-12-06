using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Cleaner
{
    public static PlayerControl cleaner;
    public static Color color = Palette.ImpostorRed;

    public static float cooldown = 30f;

    private static Sprite buttonSprite;

    public static Sprite getButtonSprite()
    {
        if (buttonSprite) return buttonSprite;
        buttonSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.CleanButton.png", 115f);
        return buttonSprite;
    }

    public static void clearAndReload()
    {
        cleaner = null;
        cooldown = CustomOptionHolder.CleanerCooldown.GetFloat();
    }
}