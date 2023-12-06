using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Fallen
{
    public static PlayerControl Player;
    public static readonly Color Color = new Color32(71, 99, 45, byte.MaxValue);

    public static void ClearAndReload()
    {
        Player = null;
    }
}