using System.Collections.Generic;
using System.Linq;
using BetterOtherRoles.Modules;
using UnityEngine;
using UniverseLib;
using UniverseLib.Config;
using UniverseLib.UI;

namespace BetterOtherRoles.UI;

public static partial class UIManager
{
    private const float StartupDelay = 1f;
    public static bool Initializing { get; private set; } = true;
    internal static UIBase? UiBase { get; private set; }
    public static GameObject? UiRoot => UiBase?.RootObject;
    public static RectTransform? UiRootRect { get; private set; }
    public static Canvas? UiCanvas { get; private set; }

    internal static readonly List<WrappedPanel> Panels = new();
    
    public static bool HasAlwaysOnTopPanelEnabled => Panels.Any(p => p is { Enabled: true, AlwaysOnTop: true });
    
    public static bool ShowMenu
    {
        get => UiBase is { Enabled: true };
        set
        {
            if (UiBase == null || !UiRoot || UiBase.Enabled == value) return;
            UniversalUI.SetUIActive(DevConfig.CurrentGuid.ToString(), value);
        }
    }

    internal static void Init()
    {
        Universe.Init(StartupDelay, InitUi, HandleLog, new UniverseLibConfig
        {
            Disable_EventSystem_Override = true,
            Force_Unlock_Mouse = false,
        });
    }
    
    private static void HandleLog(string message, LogType logType)
    {
        System.Console.WriteLine($"[UIManager:{logType}] {message}");
    }

    internal static void InitUi()
    {
        UiBase = UniversalUI.RegisterUI<UIBase>(DevConfig.CurrentGuid.ToString(), Update);
        if (UiRoot == null) return;
        UiRootRect = UiRoot.GetComponent<RectTransform>();
        UiCanvas = UiRoot.GetComponent<Canvas>();
        
        InitPanels();

        foreach (var panel in Panels)
        {
            panel.SetActive(panel.ShowByDefault);
        }

        Initializing = false;
    }

    private static void Update()
    {
        if (!UiRoot) return;
    }
}