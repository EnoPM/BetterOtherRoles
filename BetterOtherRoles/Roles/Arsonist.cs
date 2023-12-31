﻿using System.Collections.Generic;
using System.Linq;
using BetterOtherRoles.Options;
using BetterOtherRoles.Players;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Arsonist
{
    public static PlayerControl arsonist;
    public static Color color = new Color32(238, 112, 46, byte.MaxValue);

    public static float cooldown = 30f;
    public static float duration = 3f;
    public static bool triggerArsonistWin = false;

    public static PlayerControl currentTarget;
    public static PlayerControl douseTarget;
    public static List<PlayerControl> dousedPlayers = new List<PlayerControl>();

    private static Sprite douseSprite;

    public static Sprite getDouseSprite()
    {
        if (douseSprite) return douseSprite;
        douseSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.DouseButton.png", 115f);
        return douseSprite;
    }

    private static Sprite igniteSprite;

    public static Sprite getIgniteSprite()
    {
        if (igniteSprite) return igniteSprite;
        igniteSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.IgniteButton.png", 115f);
        return igniteSprite;
    }

    public static bool dousedEveryoneAlive()
    {
        return CachedPlayer.AllPlayers.All(x =>
        {
            return x.PlayerControl == Arsonist.arsonist || x.Data.IsDead || x.Data.Disconnected ||
                   Arsonist.dousedPlayers.Any(y => y.PlayerId == x.PlayerId);
        });
    }

    public static void clearAndReload()
    {
        arsonist = null;
        currentTarget = null;
        douseTarget = null;
        triggerArsonistWin = false;
        dousedPlayers = new List<PlayerControl>();
        foreach (PoolablePlayer p in TORMapOptions.playerIcons.Values)
        {
            if (p != null && p.gameObject != null) p.gameObject.SetActive(false);
        }

        cooldown = CustomOptionHolder.ArsonistCooldown.GetFloat();
        duration = CustomOptionHolder.ArsonistDuration.GetFloat();
    }
}