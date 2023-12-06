using System.Collections;
using System.Linq;
using AmongUsSpecimen.ModOptions;
using BepInEx.Unity.IL2CPP.Utils;
using BetterOtherRoles.Modules;
using BetterOtherRoles.Options;
using HarmonyLib;
using UnityEngine;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(MedScanMinigame._WalkToPad_d__16))]
public static class MedScanMinigameWalkToPadPatches
{
    private static ModBoolOption RandomizeScanPlayerPosition => CustomOptionHolder.RandomizePositionDuringScan;
    
    [HarmonyPatch(nameof(MedScanMinigame._WalkToPad_d__16.MoveNext))]
    [HarmonyPrefix]
    private static bool MoveNextPrefix(MedScanMinigame._WalkToPad_d__16 __instance)
    {
        if (!RandomizeScanPlayerPosition.GetBool() || (ShipStatus.Instance && ShipStatus.Instance.Type != ShipStatus.MapType.Pb)) return true;
        var minigame = __instance.__4__this;
        minigame.StartCoroutine(WalkToPadEnumerator(minigame));
        
        return false;
    }
    
    private static IEnumerator WalkToPadEnumerator(MedScanMinigame minigame)
    {
        var panel = UnityEngine.Object.FindObjectsOfType<GameObject>()
            .FirstOrDefault(o => o.name == "panel_medplatform");

        if (panel == null || Camera.main == null) yield break;
                
        var panelSize = panel.GetComponent<SpriteRenderer>().bounds.size * 0.3f;
            
        minigame.state = MedScanMinigame.PositionState.WalkingToPad;
        var myPhysics = PlayerControl.LocalPlayer.MyPhysics;

        Vector2 worldPos = ShipStatus.Instance.MedScanner.Position;
        var xRange = UnityEngine.Random.Range(-panelSize.x, panelSize.x);
        var yRange = UnityEngine.Random.Range(-panelSize.y, 0f);
        worldPos += new Vector2(xRange, yRange);

        Camera.main.GetComponent<FollowerCamera>().Locked = false;
        yield return myPhysics.WalkPlayerTo(worldPos, 0.001f, 1f);
        yield return new WaitForSeconds(0.1f);
        Camera.main.GetComponent<FollowerCamera>().Locked = true;
        minigame.walking = null;
    }
}