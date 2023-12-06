using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Mafioso
{
    public static PlayerControl mafioso;
    public static Color color = Palette.ImpostorRed;

    public static void clearAndReload()
    {
        mafioso = null;
    }
}