using System.Collections.Generic;
using BetterOtherRoles.Options;

namespace BetterOtherRoles.Modifiers;

public static class Vip
{
    public static List<PlayerControl> vip = new List<PlayerControl>();
    public static bool showColor = true;

    public static void clearAndReload()
    {
        vip = new List<PlayerControl>();
        showColor = CustomOptionHolder.ModifierVipShowColor.GetBool();
    }
}