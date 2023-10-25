using BetterOtherRoles.Modules;
using HarmonyLib;
using Hazel;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(ReactorSystemType))]
public static class ReactorSystemTypePatches
{
    [HarmonyPatch(nameof(ReactorSystemType.UpdateSystem))]
    [HarmonyPrefix]
    private static bool RepairDamagePrefix(ReactorSystemType __instance, PlayerControl player, MessageReader msgReader)
    {
        var oldPos = msgReader._position;
        var opCode = msgReader.ReadByte();
        msgReader._position = oldPos;
        if (ShipStatus.Instance.Type != ShipStatus.MapType.Pb || opCode != 128 || __instance.IsActive) return true;
        __instance.Countdown = BetterPolus.ReactorCountdown.getFloat();
        __instance.UserConsolePairs.Clear();
        __instance.IsDirty = true;

        return false;
    }
}