using System.Collections.Generic;
using BetterOtherRoles.Options;

namespace BetterOtherRoles.Modifiers;

public static class Sunglasses
{
    public static List<PlayerControl> sunglasses = new List<PlayerControl>();
    public static int vision = 1;

    public static void clearAndReload()
    {
        sunglasses = [];
        vision = CustomOptionHolder.ModifierSunglassesVision.CurrentSelection + 1;
    }
}