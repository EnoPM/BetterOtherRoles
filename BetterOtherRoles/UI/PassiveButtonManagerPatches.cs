using System.Linq;
using HarmonyLib;

namespace BetterOtherRoles.UI;

[HarmonyPatch(typeof(PassiveButtonManager))]
public static class PassiveButtonManagerPatches
{
    public static bool IsUiOpen { get; private set; }

    public static void CheckIsUiOpen()
    {
        IsUiOpen = UIManager.Panels.Any(p => p is { Enabled: true, CanClickThroughPanel: false });
    }
    
    [HarmonyPatch(nameof(PassiveButtonManager.Update))]
    [HarmonyPrefix]
    private static bool PassiveButtonManagerUpdatePrefix()
    {
        return !IsUiOpen;
    }
}