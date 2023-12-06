using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Godfather
{
    public static PlayerControl godfather;
    public static Color color = Palette.ImpostorRed;

    public static void clearAndReload()
    {
        godfather = null;
    }
}