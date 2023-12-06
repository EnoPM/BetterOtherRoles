using BetterOtherRoles.Objects;
using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class BountyHunter
{
    public static PlayerControl bountyHunter;
    public static Color color = Palette.ImpostorRed;

    public static Arrow arrow;
    public static float bountyDuration = 30f;
    public static bool showArrow = true;
    public static float bountyKillCooldown = 0f;
    public static float punishmentTime = 15f;
    public static float arrowUpdateIntervall = 10f;

    public static float arrowUpdateTimer = 0f;
    public static float bountyUpdateTimer = 0f;
    public static PlayerControl bounty;
    public static TMPro.TextMeshPro cooldownText;

    public static void clearAndReload()
    {
        arrow = new Arrow(color);
        bountyHunter = null;
        bounty = null;
        arrowUpdateTimer = 0f;
        bountyUpdateTimer = 0f;
        if (arrow != null && arrow.arrow != null) UnityEngine.Object.Destroy(arrow.arrow);
        arrow = null;
        if (cooldownText != null && cooldownText.gameObject != null)
            UnityEngine.Object.Destroy(cooldownText.gameObject);
        cooldownText = null;
        foreach (PoolablePlayer p in TORMapOptions.playerIcons.Values)
        {
            if (p != null && p.gameObject != null) p.gameObject.SetActive(false);
        }


        bountyDuration = CustomOptionHolder.BountyHunterBountyDuration.GetFloat();
        bountyKillCooldown = CustomOptionHolder.BountyHunterReducedCooldown.GetFloat();
        punishmentTime = CustomOptionHolder.BountyHunterPunishmentTime.GetFloat();
        showArrow = CustomOptionHolder.BountyHunterShowArrow.GetBool();
        arrowUpdateIntervall = CustomOptionHolder.BountyHunterArrowUpdateInterval.GetFloat();
    }
}