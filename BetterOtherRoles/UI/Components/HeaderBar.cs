using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace BetterOtherRoles.UI.Components;

public class HeaderBar
{
    public readonly GameObject Bar;
    public readonly Text Title;
    public readonly GameObject CloseButtonContainer;
    public readonly ButtonRef CloseButton;

    public HeaderBar(WrappedPanel panel, string title)
    {
        Bar = UIFactory.CreateHorizontalGroup(panel.ContentRoot, "TitleBar", 
            false, true, true, true, 2,
            new Vector4(5, 5, 10, 2), UIPalette.Black);
        UIFactory.SetLayoutElement(Bar, minHeight: 25, flexibleHeight: 0);
        
        Title =  UIFactory.CreateLabel(Bar, "TitleBar", title, TextAnchor.MiddleLeft, UIPalette.Secondary,
            true, 20);
        UIFactory.SetLayoutElement(Title.gameObject, 50, 25, 9999, 0);

        CloseButtonContainer = UIFactory.CreateUIObject("CloseHolder", Bar);
        UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(CloseButtonContainer, false, false, true, true, 3,
            childAlignment: TextAnchor.MiddleRight);
        
        CloseButton = UIFactory.CreateButton(CloseButtonContainer, "CloseButton", "╳");
        CloseButton.ButtonText.fontSize = 25;
        
        UIFactory.SetLayoutElement(CloseButton.Component.gameObject, minHeight: 30, minWidth: 30, flexibleWidth: 0);
        CloseButton.Component.SetColors(normal: UIPalette.Danger, hover: UIPalette.LightDanger, pressed: UIPalette.LightDanger, focused: UIPalette.Danger);
        
        CloseButton.OnClick += panel.OnCloseButtonClick;
    }

    public void SetActive(bool value)
    {
        Bar.SetActive(value);
    }
}