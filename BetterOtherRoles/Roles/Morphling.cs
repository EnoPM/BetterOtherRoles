using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Morphling
{
    public static PlayerControl morphling;
    public static Color color = Palette.ImpostorRed;
    private static Sprite sampleSprite;
    private static Sprite morphSprite;

    public static float cooldown = 30f;
    public static float duration = 10f;

    public static PlayerControl currentTarget;
    public static PlayerControl sampledTarget;
    public static PlayerControl morphTarget;
    public static float morphTimer = 0f;

    public static void resetMorph()
    {
        morphTarget = null;
        morphTimer = 0f;
        if (morphling == null) return;
        morphling.setDefaultLook();
    }

    public static void clearAndReload()
    {
        resetMorph();
        morphling = null;
        currentTarget = null;
        sampledTarget = null;
        morphTarget = null;
        morphTimer = 0f;
        cooldown = CustomOptionHolder.MorphlingCooldown.GetFloat();
        duration = CustomOptionHolder.MorphlingDuration.GetFloat();
    }

    public static Sprite getSampleSprite()
    {
        if (sampleSprite) return sampleSprite;
        sampleSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.SampleButton.png", 115f);
        return sampleSprite;
    }

    public static Sprite getMorphSprite()
    {
        if (morphSprite) return morphSprite;
        morphSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.MorphButton.png", 115f);
        return morphSprite;
    }
}