using System.Collections.Generic;
using System.Linq;
using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Jackal
{
    public static PlayerControl jackal;
    public static Color color = new Color32(0, 180, 235, byte.MaxValue);
    public static PlayerControl fakeSidekick;
    public static PlayerControl currentTarget;
    public static List<PlayerControl> formerJackals = new List<PlayerControl>();

    public static float cooldown = 30f;
    public static float createSidekickCooldown = 30f;
    public static bool canUseVents = true;
    public static bool canCreateSidekick = true;
    public static Sprite buttonSprite;
    public static bool jackalPromotedFromSidekickCanCreateSidekick = true;
    public static bool canCreateSidekickFromImpostor = true;
    public static bool hasImpostorVision = false;
    public static bool wasTeamRed;
    public static bool wasImpostor;
    public static bool wasSpy;

    public static Sprite getSidekickButtonSprite()
    {
        if (buttonSprite) return buttonSprite;
        buttonSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.SidekickButton.png", 115f);
        return buttonSprite;
    }

    public static void removeCurrentJackal()
    {
        if (!formerJackals.Any(x => x.PlayerId == jackal.PlayerId)) formerJackals.Add(jackal);
        jackal = null;
        currentTarget = null;
        fakeSidekick = null;
        cooldown = CustomOptionHolder.JackalKillCooldown.GetFloat();
        createSidekickCooldown = CustomOptionHolder.JackalCreateSidekickCooldown.GetFloat();
    }

    public static void clearAndReload()
    {
        jackal = null;
        currentTarget = null;
        fakeSidekick = null;
        cooldown = CustomOptionHolder.JackalKillCooldown.GetFloat();
        createSidekickCooldown = CustomOptionHolder.JackalCreateSidekickCooldown.GetFloat();
        canUseVents = CustomOptionHolder.JackalCanUseVents.GetBool();
        canCreateSidekick = CustomOptionHolder.JackalCanCreateSidekick.GetBool();
        jackalPromotedFromSidekickCanCreateSidekick =
            CustomOptionHolder.JackalPromotedFromSidekickCanCreateSidekick.GetBool();
        canCreateSidekickFromImpostor = CustomOptionHolder.JackalCanCreateSidekickFromImpostor.GetBool();
        formerJackals.Clear();
        hasImpostorVision = CustomOptionHolder.JackalAndSidekickHaveImpostorVision.GetBool();
        wasTeamRed = wasImpostor = wasSpy = false;
    }
}