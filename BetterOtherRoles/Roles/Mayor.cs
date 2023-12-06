using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Mayor
{
    public static PlayerControl mayor;
    public static Color color = new Color32(32, 77, 66, byte.MaxValue);
    public static Minigame emergency = null;
    public static Sprite emergencySprite = null;
    public static int remoteMeetingsLeft = 1;

    public static bool canSeeVoteColors = false;
    public static int tasksNeededToSeeVoteColors;
    public static bool meetingButton = true;
    public static int mayorChooseSingleVote;

    public static bool voteTwice = true;

    public static Sprite getMeetingSprite()
    {
        if (emergencySprite) return emergencySprite;
        emergencySprite =
            Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.EmergencyButton.png", 550f);
        return emergencySprite;
    }

    public static void clearAndReload()
    {
        mayor = null;
        emergency = null;
        emergencySprite = null;
        remoteMeetingsLeft = Mathf.RoundToInt(CustomOptionHolder.MayorMaxRemoteMeetings.GetFloat());
        canSeeVoteColors = CustomOptionHolder.MayorCanSeeVoteColors.GetBool();
        tasksNeededToSeeVoteColors = CustomOptionHolder.MayorTasksNeededToSeeVoteColors.GetInt();
        meetingButton = CustomOptionHolder.MayorMeetingButton.GetBool();
        mayorChooseSingleVote = CustomOptionHolder.MayorChooseSingleVote.CurrentSelection;
        voteTwice = true;
    }
}