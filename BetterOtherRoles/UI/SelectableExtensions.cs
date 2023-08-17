using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;

namespace BetterOtherRoles.UI;

public static class SelectableExtensions
{
    public static void SetColorsAuto(this Selectable selectable, Color color)
    {
        selectable.SetColors(normal: color, hover: color * 1.2f, pressed: color * 0.8f, focused: color, disabled: UIPalette.Danger);
    }
    
    public static void SetColors(this Selectable selectable, Color? normal = null, Color? hover = null, Color? pressed = null, Color? disabled = null, Color? focused = null)
    {
        var colors = selectable.colors;
        if (normal != null)
        {
            colors.normalColor = normal.Value;
        }

        if (hover != null)
        {
            colors.highlightedColor = hover.Value;
        }

        if (pressed != null)
        {
            colors.pressedColor = pressed.Value;
        }

        if (disabled != null)
        {
            colors.disabledColor = disabled.Value;
        }

        if (focused != null)
        {
            colors.selectedColor = focused.Value;
        }

        selectable.colors = colors;
    }
}