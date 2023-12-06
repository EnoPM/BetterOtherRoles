using BetterOtherRoles.Options;
using BetterOtherRoles.Players;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Camouflager
{
    public static PlayerControl camouflager;
    public static Color color = Palette.ImpostorRed;

    public static float cooldown = 30f;
    public static float duration = 10f;
    public static float camouflageTimer = 0f;

    private static Sprite buttonSprite;

    public static Sprite getButtonSprite()
    {
        if (buttonSprite) return buttonSprite;
        buttonSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.CamoButton.png", 115f);
        return buttonSprite;
    }

    public static void resetCamouflage()
    {
        camouflageTimer = 0f;
        foreach (PlayerControl p in CachedPlayer.AllPlayers)
        {
            if (p == Ninja.ninja && Ninja.isInvisble)
                continue;
            p.setDefaultLook();
        }
    }

    public static void clearAndReload()
    {
        resetCamouflage();
        camouflager = null;
        camouflageTimer = 0f;
        cooldown = CustomOptionHolder.CamouflagerCooldown.GetFloat();
        duration = CustomOptionHolder.CamouflagerDuration.GetFloat();
    }
}