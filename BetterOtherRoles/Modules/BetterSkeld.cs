using System;
using System.Collections;
using System.Linq;
using System.Threading;
using BepInEx.Unity.IL2CPP.Utils;
using BetterOtherRoles.Utilities.Attributes;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BetterOtherRoles.Modules;

[Autoload]
public static class BetterSkeld
{
    private static bool Enabled => CustomOptionHolder.EnableBetterSkeld.getBool();
    private static bool EnableAdmin => CustomOptionHolder.BetterSkeldEnableAdmin.getBool();
    private static bool EnableVitals => CustomOptionHolder.BetterSkeldEnableVitals.getBool();

    static BetterSkeld()
    {
        GameEvents.OnGameStarted += Start;
    }

    private static void Start()
    {
        if (!ShipStatus.Instance || !Enabled || ShipStatus.Instance.Type != ShipStatus.MapType.Ship) return;
        var gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>().ToList();
        if (!EnableAdmin)
        {
            var admin = gameObjects.Find(o => o.name == "MapRoomConsole");
            var adminAnimation = gameObjects.Find(o => o.name == "MapAnimation");
            admin.GetComponent<CircleCollider2D>().enabled = false;
            adminAnimation.active = false;
        }

        if (EnableVitals)
        {
            ShipStatus.Instance.StartCoroutine(CoCreateVitals());
        }

        var adminVent = gameObjects.Find(o => o.name == "AdminVent").GetComponent<Vent>();
        var cafeteriaVent = gameObjects.Find(o => o.name == "CafeVent").GetComponent<Vent>();
        var navNorthVent = gameObjects.Find(o => o.name == "NavVentNorth").GetComponent<Vent>();
        var navSouthVent = gameObjects.Find(o => o.name == "NavVentSouth").GetComponent<Vent>();
        var weaponsVent = gameObjects.Find(o => o.name == "WeaponsVent").GetComponent<Vent>();
        var shieldsVent = gameObjects.Find(o => o.name == "ShieldsVent").GetComponent<Vent>();
        var bigYVent = gameObjects.Find(o => o.name == "BigYVent").GetComponent<Vent>();
        var elecVent = gameObjects.Find(o => o.name == "ElecVent").GetComponent<Vent>();
        var upperReactorVent = gameObjects.Find(o => o.name == "UpperReactorVent").GetComponent<Vent>();
        var lowerReactorVent = gameObjects.Find(o => o.name == "ReactorVent").GetComponent<Vent>();
        var upperEngineVent = gameObjects.Find(o => o.name == "LEngineVent").GetComponent<Vent>();
        var lowerEngineVent = gameObjects.Find(o => o.name == "REngineVent").GetComponent<Vent>();
        var securityVent = gameObjects.Find(o => o.name == "SecurityVent").GetComponent<Vent>();
        var medVent = gameObjects.Find(o => o.name == "MedVent").GetComponent<Vent>();

        weaponsVent.Center = cafeteriaVent;
        cafeteriaVent.Center = weaponsVent;
        navNorthVent.Right = navSouthVent;
        navSouthVent.Right = navNorthVent;
        navNorthVent.Center = bigYVent;
        navSouthVent.Center = bigYVent;
        bigYVent.Center = navNorthVent;
        weaponsVent.Left = bigYVent;
        adminVent.Center = shieldsVent;
        shieldsVent.Center = adminVent;

        upperEngineVent.Center = medVent;
        medVent.Center = upperEngineVent;
        upperReactorVent.Center = securityVent;
        securityVent.Center = upperReactorVent;
        upperReactorVent.Left = lowerReactorVent;
        lowerReactorVent.Left = upperReactorVent;
        lowerReactorVent.Center = securityVent;
        elecVent.Center = lowerEngineVent;
        lowerEngineVent.Center = elecVent;
    }

    private static IEnumerator CoCreateVitals()
    {
        var polusLoader = Addressables
            .LoadAssetAsync<GameObject>(AmongUsClient.Instance.ShipPrefabs[(Index)(int)ShipStatus.MapType.Pb]);
        while (!polusLoader.IsDone)
        {
            System.Console.WriteLine(polusLoader.PercentComplete);
            yield return new WaitForEndOfFrame();
        }
        var polus = polusLoader.Result;
        var vitalsObj = polus.transform.Find("Office/panel_vitals").gameObject;
        var vitals = UnityEngine.Object.Instantiate(vitalsObj);
        vitals.transform.position = new Vector3(1.9162f, -16.1985f, -2.4142f);
        vitals.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        vitals.transform.localScale = new Vector3(0.6636f, 0.7418f, 1f);
    }
}