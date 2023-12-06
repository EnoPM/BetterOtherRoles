using BetterOtherRoles.Options;
using BetterOtherRoles.Utilities;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class SecurityGuard
{
    public static PlayerControl securityGuard;
    public static Color color = new Color32(195, 178, 95, byte.MaxValue);

    public static float cooldown = 30f;
    public static int remainingScrews = 7;
    public static int totalScrews = 7;
    public static int ventPrice = 1;
    public static int camPrice = 2;
    public static int placedCameras = 0;
    public static float duration = 10f;
    public static int maxCharges = 5;
    public static int rechargeTasksNumber = 3;
    public static int rechargedTasks = 3;
    public static int charges = 1;
    public static bool cantMove = true;
    public static Vent ventTarget = null;
    public static Minigame minigame = null;

    private static Sprite closeVentButtonSprite;

    public static Sprite getCloseVentButtonSprite()
    {
        if (closeVentButtonSprite) return closeVentButtonSprite;
        closeVentButtonSprite =
            Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.CloseVentButton.png", 115f);
        return closeVentButtonSprite;
    }

    private static Sprite placeCameraButtonSprite;

    public static Sprite getPlaceCameraButtonSprite()
    {
        if (placeCameraButtonSprite) return placeCameraButtonSprite;
        placeCameraButtonSprite =
            Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.PlaceCameraButton.png", 115f);
        return placeCameraButtonSprite;
    }

    private static Sprite animatedVentSealedSprite;
    private static float lastPPU;

    public static Sprite getAnimatedVentSealedSprite()
    {
        float ppu = 185f;
        if (SubmergedCompatibility.IsSubmerged) ppu = 120f;
        if (lastPPU != ppu)
        {
            animatedVentSealedSprite = null;
            lastPPU = ppu;
        }

        if (animatedVentSealedSprite) return animatedVentSealedSprite;
        animatedVentSealedSprite =
            Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.AnimatedVentSealed.png", ppu);
        return animatedVentSealedSprite;
    }

    private static Sprite staticVentSealedSprite;

    public static Sprite getStaticVentSealedSprite()
    {
        if (staticVentSealedSprite) return staticVentSealedSprite;
        staticVentSealedSprite =
            Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.StaticVentSealed.png", 160f);
        return staticVentSealedSprite;
    }

    private static Sprite submergedCentralUpperVentSealedSprite;

    public static Sprite getSubmergedCentralUpperSealedSprite()
    {
        if (submergedCentralUpperVentSealedSprite) return submergedCentralUpperVentSealedSprite;
        submergedCentralUpperVentSealedSprite =
            Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.CentralUpperBlocked.png", 145f);
        return submergedCentralUpperVentSealedSprite;
    }

    private static Sprite submergedCentralLowerVentSealedSprite;

    public static Sprite getSubmergedCentralLowerSealedSprite()
    {
        if (submergedCentralLowerVentSealedSprite) return submergedCentralLowerVentSealedSprite;
        submergedCentralLowerVentSealedSprite =
            Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.CentralLowerBlocked.png", 145f);
        return submergedCentralLowerVentSealedSprite;
    }

    private static Sprite camSprite;

    public static Sprite getCamSprite()
    {
        if (camSprite) return camSprite;
        camSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton.fastUseSettings[ImageNames.CamsButton]
            .Image;
        return camSprite;
    }

    private static Sprite logSprite;

    public static Sprite getLogSprite()
    {
        if (logSprite) return logSprite;
        logSprite = FastDestroyableSingleton<HudManager>.Instance.UseButton
            .fastUseSettings[ImageNames.DoorLogsButton].Image;
        return logSprite;
    }

    public static void clearAndReload()
    {
        securityGuard = null;
        ventTarget = null;
        minigame = null;
        duration = CustomOptionHolder.SecurityGuardCamDuration.GetFloat();
        maxCharges = Mathf.RoundToInt(CustomOptionHolder.SecurityGuardCamMaxCharges.GetFloat());
        rechargeTasksNumber = Mathf.RoundToInt(CustomOptionHolder.SecurityGuardCamRechargeTasksNumber.GetFloat());
        rechargedTasks = Mathf.RoundToInt(CustomOptionHolder.SecurityGuardCamRechargeTasksNumber.GetFloat());
        charges = Mathf.RoundToInt(CustomOptionHolder.SecurityGuardCamMaxCharges.GetFloat()) / 2;
        placedCameras = 0;
        cooldown = CustomOptionHolder.SecurityGuardCooldown.GetFloat();
        totalScrews = remainingScrews = Mathf.RoundToInt(CustomOptionHolder.SecurityGuardTotalScrews.GetFloat());
        camPrice = Mathf.RoundToInt(CustomOptionHolder.SecurityGuardCamPrice.GetFloat());
        ventPrice = Mathf.RoundToInt(CustomOptionHolder.SecurityGuardVentPrice.GetFloat());
        cantMove = CustomOptionHolder.SecurityGuardNoMove.GetBool();
    }
}