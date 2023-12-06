using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Warlock
{
    public static PlayerControl warlock;
    public static Color color = Palette.ImpostorRed;

    public static PlayerControl currentTarget;
    public static PlayerControl curseVictim;
    public static PlayerControl curseVictimTarget;

    public static float cooldown = 30f;
    public static float rootTime = 5f;

    private static Sprite curseButtonSprite;
    private static Sprite curseKillButtonSprite;

    public static Sprite getCurseButtonSprite()
    {
        if (curseButtonSprite) return curseButtonSprite;
        curseButtonSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.CurseButton.png", 115f);
        return curseButtonSprite;
    }

    public static Sprite getCurseKillButtonSprite()
    {
        if (curseKillButtonSprite) return curseKillButtonSprite;
        curseKillButtonSprite =
            Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.CurseKillButton.png", 115f);
        return curseKillButtonSprite;
    }

    public static void clearAndReload()
    {
        warlock = null;
        currentTarget = null;
        curseVictim = null;
        curseVictimTarget = null;
        cooldown = CustomOptionHolder.WarlockCooldown.GetFloat();
        rootTime = CustomOptionHolder.WarlockRootTime.GetFloat();
    }

    public static void resetCurse()
    {
        HudManagerStartPatch.warlockCurseButton.Timer = HudManagerStartPatch.warlockCurseButton.MaxTimer;
        HudManagerStartPatch.warlockCurseButton.Sprite = Warlock.getCurseButtonSprite();
        HudManagerStartPatch.warlockCurseButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
        currentTarget = null;
        curseVictim = null;
        curseVictimTarget = null;
    }
}