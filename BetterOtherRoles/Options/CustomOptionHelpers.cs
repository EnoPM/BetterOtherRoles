using System.Collections.Generic;
using System.Reflection;
using AmongUsSpecimen;
using AmongUsSpecimen.ModOptions;
using AmongUsSpecimen.Utils;
using UnityEngine;
using static AmongUsSpecimen.ModOptions.Helpers;
using static AmongUsSpecimen.OptionTabs;

namespace BetterOtherRoles.Options;

public static partial class CustomOptionHolder
{
    private static ModFloatOption SpawnRateOption(this ModOptionTab tab, string name, BaseModOption parent = null)
    {
        return tab.FloatOption(name, 0f, 100f, 10f, 0f, parent, suffix: "%");
    }
    
    private static ModFloatOption QuantityOption(this ModOptionTab tab, string name, BaseModOption parent = null)
    {
        return tab.FloatOption(name, 1f, 15f, 1f, 1f, parent);
    }

    private static ModFloatOption CooldownOption(this ModOptionTab tab, string name, BaseModOption parent = null,
        float minValue = 10f, float defaultValue = 30f)
    {
        return tab.FloatOption(name, minValue, 60f, 2.5f, defaultValue, parent, suffix: "s");
    }

    private static ModFloatOption DurationOption(this ModOptionTab tab, string name, BaseModOption parent = null)
    {
        return tab.FloatOption(name, 1f, 30f, 0.5f, 10f, parent, suffix: "s");
    }

    private static ModFloatOption ApplyDurationOption(this ModOptionTab tab, string name, BaseModOption parent = null)
    {
        return tab.FloatOption(name, 0f, 10f, 1f, 1f, parent, suffix: "s");
    }

    private static string Cs(Color color, string text) => ColorHelpers.Colorize(color, text);
    
    public class RandomMapModOptionMap
    {
        internal static readonly List<RandomMapModOptionMap> AllMaps = [];
        
        internal readonly ModFloatOption Percentage;
        internal readonly ModBoolOption ShouldUseSpecificPreset;
        internal readonly ModStringOption PresetName;

        internal RandomMapModOptionMap(string mapName)
        {
            Percentage = OutsidePreset(MainTab.FloatOption(mapName, 0f, 100f, 1f, 0f, RandomMap, suffix: "%"));
            ShouldUseSpecificPreset = OutsidePreset(MainTab.BoolOption("Specific Preset", false, Percentage));
            PresetName = OutsidePreset(MainTab.StringOption("Map Preset", CoreOptions.GetPresetNames, CoreOptions.GetPresetNames()[0], ShouldUseSpecificPreset));
            AllMaps.Add(this);
        }
    }
    
    private static void OnHostChanged()
    {
        foreach (var mapOptions in RandomMapModOptionMap.AllMaps)
        {
            mapOptions.PresetName.Update();
        }
    }
    
    private static Sprite GetSprite(string name, float pixelsPerUnit = 100f)
    {
        return Assembly.GetExecutingAssembly()
            .LoadSpriteFromResources($"BetterOtherRoles.Resources.{name}.png", pixelsPerUnit);
    }
}