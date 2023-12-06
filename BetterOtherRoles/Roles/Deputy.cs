using System.Collections.Generic;
using BetterOtherRoles.Options;
using BetterOtherRoles.Players;
using Hazel;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Deputy
    {
        public static PlayerControl deputy;
        public static Color color = Sheriff.color;

        public static PlayerControl currentTarget;
        public static List<byte> handcuffedPlayers = new List<byte>();
        public static int promotesToSheriff; // No: 0, Immediately: 1, After Meeting: 2
        public static bool keepsHandcuffsOnPromotion;
        public static float handcuffDuration;
        public static float remainingHandcuffs;
        public static float handcuffCooldown;
        public static bool knowsSheriff;
        public static Dictionary<byte, float> handcuffedKnows = new Dictionary<byte, float>();

        private static Sprite buttonSprite;
        private static Sprite handcuffedSprite;

        public static Sprite getButtonSprite()
        {
            if (buttonSprite) return buttonSprite;
            buttonSprite =
                Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.DeputyHandcuffButton.png", 115f);
            return buttonSprite;
        }

        public static Sprite getHandcuffedButtonSprite()
        {
            if (handcuffedSprite) return handcuffedSprite;
            handcuffedSprite =
                Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.DeputyHandcuffed.png", 115f);
            return handcuffedSprite;
        }

        // Can be used to enable / disable the handcuff effect on the target's buttons
        public static void setHandcuffedKnows(bool active = true, byte playerId = byte.MaxValue)
        {
            if (playerId == byte.MaxValue)
                playerId = CachedPlayer.LocalPlayer.PlayerId;

            if (active && playerId == CachedPlayer.LocalPlayer.PlayerId)
            {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(
                    CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ShareGhostInfo,
                    Hazel.SendOption.Reliable, -1);
                writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                writer.Write((byte)RPCProcedure.GhostInfoTypes.HandcuffNoticed);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }

            if (active)
            {
                handcuffedKnows.Add(playerId, handcuffDuration);
                handcuffedPlayers.RemoveAll(x => x == playerId);
            }

            if (playerId == CachedPlayer.LocalPlayer.PlayerId)
            {
                HudManagerStartPatch.setAllButtonsHandcuffedStatus(active);
                SoundEffectsManager.play("deputyHandcuff");
            }
        }

        public static void clearAndReload()
        {
            deputy = null;
            currentTarget = null;
            handcuffedPlayers = new List<byte>();
            handcuffedKnows = new Dictionary<byte, float>();
            HudManagerStartPatch.setAllButtonsHandcuffedStatus(false, true);
            promotesToSheriff = CustomOptionHolder.DeputyGetsPromoted.CurrentSelection;
            remainingHandcuffs = CustomOptionHolder.DeputyNumberOfHandcuffs.GetFloat();
            handcuffCooldown = CustomOptionHolder.DeputyHandcuffCooldown.GetFloat();
            keepsHandcuffsOnPromotion = CustomOptionHolder.DeputyKeepsHandcuffs.GetBool();
            handcuffDuration = CustomOptionHolder.DeputyHandcuffDuration.GetFloat();
            knowsSheriff = CustomOptionHolder.DeputyKnowsSheriff.GetBool();
        }
    }