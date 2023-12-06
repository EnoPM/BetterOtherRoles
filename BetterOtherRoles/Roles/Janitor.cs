using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Janitor
{
    public static PlayerControl janitor;
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
        janitor = null;
        cooldown = CustomOptionHolder.JanitorCooldown.GetFloat();
    }
}