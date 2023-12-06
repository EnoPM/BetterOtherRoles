using System.Collections.Generic;
using BetterOtherRoles.Players;
using UnityEngine;

namespace BetterOtherRoles.Modifiers;

public static class AntiTeleport
{
    public static List<PlayerControl> antiTeleport = new List<PlayerControl>();
    public static Vector3 position;

    public static void clearAndReload()
    {
        antiTeleport = new List<PlayerControl>();
        position = Vector3.zero;
    }

    public static void setPosition()
    {
        if (position == Vector3.zero)
            return; // Check if this has been set, otherwise first spawn on submerged will fail
        if (antiTeleport.FindAll(x => x.PlayerId == CachedPlayer.LocalPlayer.PlayerId).Count > 0)
        {
            CachedPlayer.LocalPlayer.NetTransform.RpcSnapTo(position);
            if (SubmergedCompatibility.IsSubmerged)
            {
                SubmergedCompatibility.ChangeFloor(position.y > -7);
            }
        }
    }
}