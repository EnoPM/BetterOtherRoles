using System.Collections;
using BetterOtherRoles.Options;
using BetterOtherRoles.Players;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class StickyBomber
{
    public static PlayerControl Player;
    public static Color Color = Palette.ImpostorRed;

    public static PlayerControl StuckPlayer;
    public static float RemainingTime;
    public static float RemainingDelay;
    public static PlayerControl CurrentTarget;
    public static PlayerControl CurrentTransferTarget;

    public static float BombCooldown;
    public static float FirstDelay;
    public static float OtherDelay;
    public static float Duration;
    public static bool CanReceiveBomb;
    public static bool ShieldedPlayerCanReceiveBomb;
    public static bool AllowKillButton;
    public static bool TriggerBothCooldown;
    public static bool ShowRemainingTime;

    public static void ClearAndReload()
    {
        Player = null;
        StuckPlayer = null;
        RemainingTime = 0f;
        RemainingDelay = 0f;
        CurrentTarget = null;
        CurrentTransferTarget = null;

        BombCooldown = CustomOptionHolder.StickyBomberCooldown.GetFloat();
        FirstDelay = CustomOptionHolder.StickyBomberFirstDelay.GetFloat();
        OtherDelay = CustomOptionHolder.StickyBomberOtherDelay.GetFloat();
        Duration = CustomOptionHolder.StickyBomberDuration.GetFloat();
        CanReceiveBomb = CustomOptionHolder.StickyBomberCanReceiveBomb.GetBool();
        ShieldedPlayerCanReceiveBomb = CustomOptionHolder.StickyBomberCanGiveBombToShielded.GetBool();
        AllowKillButton = CustomOptionHolder.StickyBomberEnableKillButton.GetBool();
        TriggerBothCooldown = CustomOptionHolder.StickyBomberTriggerAllCooldowns.GetBool();
        ShowRemainingTime = CustomOptionHolder.StickyBomberShowTimer.GetBool();
    }

    public static Sprite StickyButton =>
        Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.StickyBombButton.png", 115f);

    public static Sprite StickyTransferButton =>
        Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.StickyBombTransferButton.png", 115f);

    public static void RpcGiveBomb(byte playerId)
    {
        var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId,
            (byte)CustomRPC.StickyBomberGiveBomb, Hazel.SendOption.Reliable, -1);
        writer.Write(playerId);
        AmongUsClient.Instance.FinishRpcImmediately(writer);
        GiveBomb(playerId);
    }

    public static void GiveBomb(byte playerId)
    {
        if (playerId == byte.MaxValue)
        {
            StuckPlayer = null;
            if (Player != PlayerControl.LocalPlayer) return;
            HudManagerStartPatch.stickyBomberButton.HasEffect = false;
            HudManagerStartPatch.stickyBomberButton.Timer = HudManagerStartPatch.stickyBomberButton.MaxTimer;
            return;
        }

        var player = Helpers.playerById(playerId);
        if (player == null) return;
        RemainingDelay = StuckPlayer == null ? FirstDelay : OtherDelay;
        if (StuckPlayer == null)
        {
            RemainingTime = Duration;
            if (TriggerBothCooldown && Player == CachedPlayer.LocalPlayer.PlayerControl)
            {
                Player.killTimer = Duration + 1f;
            }
        }

        StuckPlayer = player;
    }

    public static IEnumerator CoCreateBomb()
    {
        var timer = 0f;
        while (timer <= Duration)
        {
            timer += Time.deltaTime;
            if (StuckPlayer && StuckPlayer.Data.IsDead)
            {
                System.Console.WriteLine($"Stuck player is dead");
                RpcGiveBomb(byte.MaxValue);
                if (HudManagerStartPatch.stickyBomberButton != null)
                {
                    HudManagerStartPatch.stickyBomberButton.HasEffect = false;
                    HudManagerStartPatch.stickyBomberButton.Timer = 0.5f;
                }

                if (TriggerBothCooldown)
                {
                    Player.killTimer = 0.5f;
                }

                break;
            }

            yield return new WaitForEndOfFrame();
        }

        if (!Player || !StuckPlayer) yield break;
        var killAttempt = Helpers.checkMurderAttemptAndKill(Player, StuckPlayer, showAnimation: false);
        if (killAttempt == MurderAttemptResult.PerformKill)
        {
            var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId,
                (byte)CustomRPC.ShareGhostInfo, Hazel.SendOption.Reliable, -1);
            writer.Write(CachedPlayer.LocalPlayer.PlayerId);
            writer.Write((byte)RPCProcedure.GhostInfoTypes.DeathReasonAndKiller);
            writer.Write(StuckPlayer.PlayerId);
            writer.Write((byte)DeadPlayer.CustomDeathReason.StickyBomb);
            writer.Write(Player.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            GameHistory.overrideDeathReasonAndKiller(StuckPlayer, DeadPlayer.CustomDeathReason.StickyBomb,
                killer: StickyBomber.Player);
        }

        RpcGiveBomb(byte.MaxValue);
    }
}