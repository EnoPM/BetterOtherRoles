using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Engineer
{
    public static PlayerControl engineer;
    public static Color color = new Color32(0, 40, 245, byte.MaxValue);
    private static Sprite buttonSprite;

    public static int remainingFixes = 1;
    public static bool highlightForImpostors = true;
    public static bool highlightForTeamJackal = true;

    public static Sprite getButtonSprite()
    {
        if (buttonSprite) return buttonSprite;
        buttonSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.RepairButton.png", 115f);
        return buttonSprite;
    }

    public static void clearAndReload()
    {
        engineer = null;
        remainingFixes = Mathf.RoundToInt(CustomOptionHolder.EngineerNumberOfFixes.GetFloat());
        highlightForImpostors = CustomOptionHolder.EngineerHighlightForImpostors.GetBool();
        highlightForTeamJackal = CustomOptionHolder.EngineerHighlightForTeamJackal.GetBool();
    }
}