using System.Collections.Generic;
using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Thief
{
    public static PlayerControl thief;
    public static Color color = new Color32(71, 99, 45, byte.MaxValue);
    public static PlayerControl currentTarget;
    public static PlayerControl formerThief;

    public static float cooldown = 30f;

    public static bool suicideFlag = false; // Used as a flag for suicide

    public static bool hasImpostorVision;
    public static bool canUseVents;
    public static bool canKillSheriff;
    public static bool canStealWithGuess;

    public static void clearAndReload()
    {
        thief = null;
        suicideFlag = false;
        currentTarget = null;
        formerThief = null;
        hasImpostorVision = CustomOptionHolder.ThiefHasImpVision.GetBool();
        cooldown = CustomOptionHolder.ThiefCooldown.GetFloat();
        canUseVents = CustomOptionHolder.ThiefCanUseVents.GetBool();
        canKillSheriff = CustomOptionHolder.ThiefCanKillSheriff.GetBool();
        canStealWithGuess = CustomOptionHolder.ThiefCanStealWithGuess.GetBool();
    }

    public static bool isFailedThiefKill(PlayerControl target, PlayerControl killer, RoleInfo targetRole)
    {
        return killer == Thief.thief && !target.Data.Role.IsImpostor && !new List<RoleInfo>
            { RoleInfo.jackal, canKillSheriff ? RoleInfo.sheriff : null, RoleInfo.sidekick }.Contains(targetRole);
    }
}