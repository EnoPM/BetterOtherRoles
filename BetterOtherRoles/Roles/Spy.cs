using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Spy
{
    public static PlayerControl spy;
    public static Color color = Palette.ImpostorRed;

    public static bool impostorsCanKillAnyone = true;
    public static bool canEnterVents = false;
    public static bool hasImpostorVision = false;

    public static void clearAndReload()
    {
        spy = null;
        impostorsCanKillAnyone = CustomOptionHolder.SpyImpostorsCanKillAnyone.GetBool();
        canEnterVents = CustomOptionHolder.SpyCanEnterVents.GetBool();
        hasImpostorVision = CustomOptionHolder.SpyHasImpostorVision.GetBool();
    }
}