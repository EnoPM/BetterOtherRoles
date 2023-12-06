using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Medic
{
    public static PlayerControl medic;
    public static PlayerControl shielded;
    public static PlayerControl futureShielded;

    public static Color color = new Color32(126, 251, 194, byte.MaxValue);
    public static bool usedShield;

    public static int showShielded = 0;
    public static bool showAttemptToShielded = false;
    public static bool showAttemptToMedic = false;
    public static bool setShieldAfterMeeting = false;
    public static bool showShieldAfterMeeting = false;
    public static bool meetingAfterShielding = false;

    public static Color shieldedColor = new Color32(0, 221, 255, byte.MaxValue);
    public static PlayerControl currentTarget;

    private static Sprite buttonSprite;

    public static Sprite getButtonSprite()
    {
        if (buttonSprite) return buttonSprite;
        buttonSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.ShieldButton.png", 115f);
        return buttonSprite;
    }

    public static void clearAndReload()
    {
        medic = null;
        shielded = null;
        futureShielded = null;
        currentTarget = null;
        usedShield = false;
        showShielded = CustomOptionHolder.MedicShowShielded.CurrentSelection;
        showAttemptToShielded = CustomOptionHolder.MedicShowAttemptToShielded.GetBool();
        showAttemptToMedic = CustomOptionHolder.MedicShowAttemptToMedic.GetBool();
        setShieldAfterMeeting = CustomOptionHolder.MedicSetOrShowShieldAfterMeeting.CurrentSelection == 2;
        showShieldAfterMeeting = CustomOptionHolder.MedicSetOrShowShieldAfterMeeting.CurrentSelection == 1;
        meetingAfterShielding = false;
    }
}