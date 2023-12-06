using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Detective
{
    public static PlayerControl detective;
    public static Color color = new Color32(45, 106, 165, byte.MaxValue);

    public static float footprintIntervall = 1f;
    public static float footprintDuration = 1f;
    public static bool anonymousFootprints = false;
    public static float reportNameDuration = 0f;
    public static float reportColorDuration = 20f;
    public static float timer = 6.2f;

    public static void clearAndReload()
    {
        detective = null;
        anonymousFootprints = CustomOptionHolder.DetectiveAnonymousFootprints.GetBool();
        footprintIntervall = CustomOptionHolder.DetectiveFootprintInterval.GetFloat();
        footprintDuration = CustomOptionHolder.DetectiveFootprintDuration.GetFloat();
        reportNameDuration = CustomOptionHolder.DetectiveReportNameDuration.GetFloat();
        reportColorDuration = CustomOptionHolder.DetectiveReportColorDuration.GetFloat();
        timer = 6.2f;
    }
}