using System.Collections.Generic;
using BetterOtherRoles.Objects;
using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Vulture
{
    public static PlayerControl vulture;
    public static Color color = new Color32(139, 69, 19, byte.MaxValue);
    public static List<Arrow> localArrows = new List<Arrow>();
    public static float cooldown = 30f;
    public static int vultureNumberToWin = 4;
    public static int eatenBodies = 0;
    public static bool triggerVultureWin = false;
    public static bool canUseVents = true;
    public static bool showArrows = true;
    private static Sprite buttonSprite;

    public static Sprite getButtonSprite()
    {
        if (buttonSprite) return buttonSprite;
        buttonSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.VultureButton.png", 115f);
        return buttonSprite;
    }

    public static void clearAndReload()
    {
        vulture = null;
        vultureNumberToWin = Mathf.RoundToInt(CustomOptionHolder.VultureNumberToWin.GetFloat());
        eatenBodies = 0;
        cooldown = CustomOptionHolder.VultureCooldown.GetFloat();
        triggerVultureWin = false;
        canUseVents = CustomOptionHolder.VultureCanUseVents.GetBool();
        showArrows = CustomOptionHolder.VultureShowArrow.GetBool();
        if (localArrows != null)
        {
            foreach (Arrow arrow in localArrows)
                if (arrow?.arrow != null)
                    UnityEngine.Object.Destroy(arrow.arrow);
        }

        localArrows = new List<Arrow>();
    }
}