using HarmonyLib;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(CustomNetworkTransform))]
internal static class CustomNetworkTransformPatches
{
    [HarmonyPatch(nameof(CustomNetworkTransform.IsInMiddleOfAnimationThatMakesPlayerInvisible)), HarmonyPostfix]
    private static void IsInMiddleOfAnimationThatMakesPlayerInvisiblePrefix(CustomNetworkTransform __instance, ref bool __result)
    {
        __result = __instance.myPlayer.MyPhysics.Animations.IsPlayingEnterVentAnimation() || __instance.myPlayer.walkingToVent || __instance.myPlayer.IsWalkingToTask();
    }
}