using System;
using BetterOtherRoles.Objects;
using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Bomber
{
    public static PlayerControl bomber = null;
    public static Color color = Palette.ImpostorRed;

    public static Bomb bomb = null;
    public static bool isPlanted = false;
    public static bool isActive = false;
    public static float destructionTime = 20f;
    public static float destructionRange = 2f;
    public static float hearRange = 30f;
    public static float defuseDuration = 3f;
    public static float bombCooldown = 15f;
    public static float bombActiveAfter = 3f;

    private static Sprite buttonSprite;

    public static Sprite getButtonSprite()
    {
        if (buttonSprite) return buttonSprite;
        buttonSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.Bomb_Button_Plant.png", 115f);
        return buttonSprite;
    }

    public static void clearBomb(bool flag = true)
    {
        if (bomb != null)
        {
            UnityEngine.Object.Destroy(bomb.bomb);
            UnityEngine.Object.Destroy(bomb.background);
            bomb = null;
        }

        isPlanted = false;
        isActive = false;
        if (flag)
        {
            try
            {
                SoundEffectsManager.stop("bombFuseBurning");
            }
            catch (Exception e)
            {
                BetterOtherRolesPlugin.Logger.LogWarning("Unable to stop bomb sound");
            }
        }
    }

    public static void clearAndReload()
    {
        clearBomb(false);
        bomber = null;
        bomb = null;
        isPlanted = false;
        isActive = false;
        destructionTime = CustomOptionHolder.BomberBombDestructionTime.GetFloat();
        destructionRange = CustomOptionHolder.BomberBombDestructionRange.GetFloat() / 10;
        hearRange = CustomOptionHolder.BomberBombHearRange.GetFloat() / 10;
        defuseDuration = CustomOptionHolder.BomberDefuseDuration.GetFloat();
        bombCooldown = CustomOptionHolder.BomberBombCooldown.GetFloat();
        bombActiveAfter = CustomOptionHolder.BomberBombActiveAfter.GetFloat();
        Bomb.clearBackgroundSprite();
    }
}