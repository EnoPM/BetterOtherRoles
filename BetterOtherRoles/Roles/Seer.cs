using System.Collections.Generic;
using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Seer
{
    public static PlayerControl seer;
    public static Color color = new Color32(97, 178, 108, byte.MaxValue);
    public static List<Vector3> deadBodyPositions = new List<Vector3>();

    public static float soulDuration = 15f;
    public static bool limitSoulDuration = false;
    public static int mode = 0;

    private static Sprite soulSprite;

    public static Sprite getSoulSprite()
    {
        if (soulSprite) return soulSprite;
        soulSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.Soul.png", 500f);
        return soulSprite;
    }

    public static void clearAndReload()
    {
        seer = null;
        deadBodyPositions = new List<Vector3>();
        limitSoulDuration = CustomOptionHolder.SeerLimitSoulDuration.GetBool();
        soulDuration = CustomOptionHolder.SeerSoulDuration.GetFloat();
        mode = CustomOptionHolder.SeerMode.CurrentSelection;
    }
}