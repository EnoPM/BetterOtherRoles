using HarmonyLib;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(Console))]
public static class ConsolePatches
{
    [HarmonyPatch(nameof(Console.Use))]
    [HarmonyPostfix]
    private static void UsePostfix(Console __instance)
    {
        System.Console.WriteLine($"Opened console: {__instance.name}");
    }
}