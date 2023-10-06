using BetterOtherRoles.Modules;
using HarmonyLib;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(PlayerPurchasesData))]
public static class PlayerPurchasesDataPatches
{
    [HarmonyPatch(nameof(PlayerPurchasesData.GetPurchase))]
    [HarmonyPrefix]
    private static bool GetPurchasePrefix(PlayerPurchasesData __instance, string itemKey, string bundleKey, out bool __result)
    {
        if (DevConfig.HasFlag("UNLOCK_ALL_COSMETICS"))
        {
            __result = true;
            return false;
        }
        bool purchase = false;
        if (!string.IsNullOrEmpty(bundleKey))
            purchase = __instance.purchases.Contains(bundleKey);
        if (!purchase && !string.IsNullOrEmpty(itemKey))
            purchase = __instance.purchases.Contains(itemKey);
        __result = purchase;
        return false;
    }
}