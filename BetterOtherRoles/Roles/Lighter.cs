using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Lighter
{
    public static PlayerControl lighter;
    public static Color color = new Color32(238, 229, 190, byte.MaxValue);

    public static float lighterModeLightsOnVision = 2f;
    public static float lighterModeLightsOffVision = 0.75f;
    public static float flashlightWidth = 0.75f;

    public static void clearAndReload()
    {
        lighter = null;
        flashlightWidth = CustomOptionHolder.LighterFlashlightWidth.GetFloat();
        lighterModeLightsOnVision = CustomOptionHolder.LighterModeLightsOnVision.GetFloat();
        lighterModeLightsOffVision = CustomOptionHolder.LighterModeLightsOffVision.GetFloat();
    }
}