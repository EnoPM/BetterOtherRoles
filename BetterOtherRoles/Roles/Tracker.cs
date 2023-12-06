using System.Collections.Generic;
using BetterOtherRoles.Objects;
using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Tracker
{
    public static PlayerControl tracker;
    public static Color color = new Color32(100, 58, 220, byte.MaxValue);
    public static List<Arrow> localArrows = new List<Arrow>();

    public static float updateIntervall = 5f;
    public static bool resetTargetAfterMeeting = false;
    public static bool canTrackCorpses = false;
    public static float corpsesTrackingCooldown = 30f;
    public static float corpsesTrackingDuration = 5f;
    public static float corpsesTrackingTimer = 0f;
    public static List<Vector3> deadBodyPositions = new List<Vector3>();

    public static PlayerControl currentTarget;
    public static PlayerControl tracked;
    public static bool usedTracker = false;
    public static float timeUntilUpdate = 0f;
    public static Arrow arrow = new Arrow(Color.blue);

    private static Sprite trackCorpsesButtonSprite;

    public static Sprite getTrackCorpsesButtonSprite()
    {
        if (trackCorpsesButtonSprite) return trackCorpsesButtonSprite;
        trackCorpsesButtonSprite =
            Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.PathfindButton.png", 115f);
        return trackCorpsesButtonSprite;
    }

    private static Sprite buttonSprite;

    public static Sprite getButtonSprite()
    {
        if (buttonSprite) return buttonSprite;
        buttonSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.TrackerButton.png", 115f);
        return buttonSprite;
    }

    public static void resetTracked()
    {
        currentTarget = tracked = null;
        usedTracker = false;
        if (arrow?.arrow != null) UnityEngine.Object.Destroy(arrow.arrow);
        arrow = new Arrow(Color.blue);
        if (arrow.arrow != null) arrow.arrow.SetActive(false);
    }

    public static void clearAndReload()
    {
        tracker = null;
        resetTracked();
        timeUntilUpdate = 0f;
        updateIntervall = CustomOptionHolder.TrackerUpdateInterval.GetFloat();
        resetTargetAfterMeeting = CustomOptionHolder.TrackerResetTargetAfterMeeting.GetBool();
        if (localArrows != null)
        {
            foreach (Arrow arrow in localArrows)
                if (arrow?.arrow != null)
                    UnityEngine.Object.Destroy(arrow.arrow);
        }

        deadBodyPositions = new List<Vector3>();
        corpsesTrackingTimer = 0f;
        corpsesTrackingCooldown = CustomOptionHolder.TrackerCorpsesTrackingCooldown.GetFloat();
        corpsesTrackingDuration = CustomOptionHolder.TrackerCorpsesTrackingDuration.GetFloat();
        canTrackCorpses = CustomOptionHolder.TrackerCanTrackCorpses.GetBool();
    }
}