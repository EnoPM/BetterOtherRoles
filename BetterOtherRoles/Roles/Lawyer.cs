using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Lawyer
{
    public static PlayerControl lawyer;
    public static PlayerControl target;
    public static Color color = new Color32(134, 153, 25, byte.MaxValue);
    public static Sprite targetSprite;
    public static bool triggerProsecutorWin = false;
    public static bool isProsecutor = false;
    public static bool canCallEmergency = true;

    public static float vision = 1f;
    public static bool lawyerKnowsRole = false;
    public static bool targetCanBeJester = false;
    public static bool targetWasGuessed = false;

    public static Sprite getTargetSprite()
    {
        if (targetSprite) return targetSprite;
        targetSprite = Helpers.loadSpriteFromResources("", 150f);
        return targetSprite;
    }

    public static void clearAndReload(bool clearTarget = true)
    {
        lawyer = null;
        if (clearTarget)
        {
            target = null;
            targetWasGuessed = false;
        }

        isProsecutor = false;
        triggerProsecutorWin = false;
        vision = CustomOptionHolder.LawyerVision.GetFloat();
        lawyerKnowsRole = CustomOptionHolder.LawyerKnowsRole.GetBool();
        targetCanBeJester = CustomOptionHolder.LawyerTargetCanBeJester.GetBool();
        canCallEmergency = CustomOptionHolder.LawyerCanCallEmergency.GetBool();
    }
}