using System.Collections.Generic;
using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Modifiers;

public static class Bait
{
    public static List<PlayerControl> bait = new List<PlayerControl>();
    public static Dictionary<DeadPlayer, float> active = new Dictionary<DeadPlayer, float>();
    public static Color color = new Color32(0, 247, 255, byte.MaxValue);

    public static float reportDelayMin = 0f;
    public static float reportDelayMax = 0f;
    public static bool showKillFlash = true;

    public static void clearAndReload()
    {
        bait = new List<PlayerControl>();
        active = new Dictionary<DeadPlayer, float>();
        reportDelayMin = CustomOptionHolder.ModifierBaitReportDelayMin.GetFloat();
        reportDelayMax = CustomOptionHolder.ModifierBaitReportDelayMax.GetFloat();
        if (reportDelayMin > reportDelayMax) reportDelayMin = reportDelayMax;
        showKillFlash = CustomOptionHolder.ModifierBaitShowKillFlash.GetBool();
    }
}