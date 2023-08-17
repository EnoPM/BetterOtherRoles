using BepInEx.Configuration;
using BetterOtherRoles.UI.Panels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniverseLib.UI;

namespace BetterOtherRoles.UI.Components;

public class LocalOptionEditor
{
    private readonly GameObject _container;
    private readonly Toggle _toggle;
    private readonly ConfigEntry<bool> _entry;

    public delegate void Updated(bool value);
    public event Updated OnUpdated;
    
    public LocalOptionEditor(string title, LocalOptionsPanel panel, GameObject contentRoot, ConfigEntry<bool> entry)
    {
        _entry = entry;
        _container = UIFactory.CreateHorizontalGroup(contentRoot, "Container",
            false, true, true,
            true, padding: new Vector4(0f, 0f, 10f, 0f), bgColor: UIPalette.Transparent);
        UIFactory.SetLayoutElement(_container, minHeight: 50, flexibleHeight: 9999, minWidth: panel.MinWidth - 30,
            flexibleWidth: 0);
        
        UIFactory.CreateToggle(_container, "Toggle", out _toggle, out var label);
        _toggle.isOn = _entry.Value;
        _toggle.onValueChanged.AddListener((UnityAction<bool>)OnToggleValueChanged);
        _toggle.graphic.color = UIPalette.Success;
        
        label.text = title;
        label.color = UIPalette.Secondary;
        label.fontSize = 20;
    }
    
    private void OnToggleValueChanged(bool value)
    {
        System.Console.WriteLine($"entry: {_entry != null}");
        if (_entry != null)
        {
            _entry.Value = value;
        }
        OnUpdated?.Invoke(value);
    }
    
    public void SetActive(bool active)
    {
        _container.SetActive(active);
    }
}