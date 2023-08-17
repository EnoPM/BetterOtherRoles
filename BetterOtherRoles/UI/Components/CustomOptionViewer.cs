using System.Collections.Generic;
using System.Linq;
using BetterOtherRoles.Modules;
using BetterOtherRoles.UI.Panels;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;

namespace BetterOtherRoles.UI.Components;

public class CustomOptionViewer
{
    public static readonly List<CustomOptionViewer> Options = new();
    
    private readonly CustomOption _option;
    private readonly GameObject _container;
    private readonly Text _label;
    private readonly Text _value;

    public CustomOptionViewer(CustomOptionsPanel panel, GameObject contentRoot, CustomOption option)
    {
        _option = option;
        var depth = Depth;
        var group = contentRoot;
        
        _container = UIFactory.CreateHorizontalGroup(group, "Container",
            false, true, true,
            true, padding: new Vector4(0f + (_option.isHeader ? 20f : 0f), 0f, 10f, 0f), bgColor: UIPalette.Transparent);
        UIFactory.SetLayoutElement(_container, minHeight: 35, flexibleHeight: 9999, minWidth: panel.MinWidth,
            flexibleWidth: 0);
        
        _label = UIFactory.CreateLabel(_container, "Label", string.Empty);
        UIFactory.SetLayoutElement(_label.gameObject, 350, 25, 0, 0);
        var container = UIFactory.CreateUIObject("EditorValueContainer", _container);
        UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(container, true, false, true, true, 0,
            childAlignment: TextAnchor.MiddleRight, padRight: 20, padLeft: 20);

        var valueString = _option.DisplayUIValue;
        
        var valueContainer = UIFactory.CreateHorizontalGroup(container, "ValueContainer",
            true, true, true, true,
            padding: new Vector4(0f, 0f, 5f, 5f), childAlignment: TextAnchor.MiddleRight);
        UIFactory.SetLayoutElement(valueContainer, 200, 30, flexibleWidth: 0, 0);
        _value = UIFactory.CreateLabel(valueContainer, "ValueLabel", valueString,
            TextAnchor.MiddleRight, UIPalette.Secondary);
        _value.alignment = TextAnchor.MiddleCenter;
        _value.fontSize = 18;
        
        _label.text = _option.name;
        _label.color = UIPalette.Secondary;
        _label.fontSize = 18;
        
        Options.Add(this);
        SetActive(IsParentActive);
        _option.OnChange += UpdateValue;
    }

    private void UpdateValue()
    {
        _value.text = _option.DisplayUIValue;
        var subOptions = Options.Where(o => o._option.parent == _option);
        foreach (var subOption in subOptions)
        {
            subOption.UpdateValue();
        }
        SetActive(IsParentActive);
    }
    
    public void SetActive(bool active)
    {
        _container.SetActive(active);
    }

    private int Depth
    {
        get
        {
            var currentOption = _option;
            var depth = 0;
            while (currentOption.parent != null)
            {
                depth++;
                currentOption = currentOption.parent;
            }

            return depth;
        }
    }

    private bool IsParentActive
    {
        get
        {
            if (_option.parent == null) return true;
            var currentOption = _option;
            var active = true;
            while (currentOption.parent != null && active)
            {
                currentOption = currentOption.parent;
                active = currentOption.selection > 0;
            }

            return active;
        }
    }

    private bool HasChildren => CustomOption.options.Any(o => o.parent == _option);
}