using System.Collections.Generic;
using BetterOtherRoles.Options;

namespace BetterOtherRoles.Modifiers;

public static class Invert
{
    public static List<PlayerControl> invert = new List<PlayerControl>();
    public static int meetings = 3;

    public static void clearAndReload()
    {
        invert = [];
        meetings = CustomOptionHolder.ModifierInvertDuration.GetInt();
    }
}