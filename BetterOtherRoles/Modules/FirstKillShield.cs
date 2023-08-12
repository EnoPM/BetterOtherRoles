using BetterOtherRoles.Utilities.Extensions;
using UnityEngine;

namespace BetterOtherRoles.Modules;

public static class FirstKillShield
{
    public static Color ShieldColor => Color.HSVToRGB(Mathf.PingPong(Time.time * 0.30f, 1), 1, 1);
    public static bool Enabled => CustomOptionHolder.shieldFirstKill.getBool();
    public static bool ExpireWithTimer => CustomOptionHolder.ExpireFirstKillShield.getBool();
    public static float MaxShieldTimer => CustomOptionHolder.FirstKillShieldDuration.getFloat();
    public static string FirstKilledPlayerName = string.Empty;
    public static PlayerControl ShieldedPlayer;
    public static float ShieldTimer = 0f;

    public static void ClearAndReload()
    {
        ShieldedPlayer = null;
        ShieldTimer = MaxShieldTimer;
    }

    public static void HudUpdate()
    {
        if (ShieldedPlayer == null) return;
        if (!Enabled) return;
        if (!ExpireWithTimer) return;
        if (ShieldTimer > 0f)
        {
            ShieldTimer -= Time.deltaTime;
        }

        if (ShieldTimer <= 0f)
        {
            ShieldedPlayer = null;
        }
    }
}