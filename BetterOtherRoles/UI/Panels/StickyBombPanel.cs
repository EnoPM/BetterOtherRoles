using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;

namespace BetterOtherRoles.UI.Panels;

public class StickyBombPanel : WrappedPanel
{
    public StickyBombPanel(UIBase owner) : base(owner)
    {
    }
    
    public override string Name => $"<color=#fcba03>Better</color><color=#ff351f>OtherRoles</color> v{BetterOtherRolesPlugin.VersionString}";

    public override int MinWidth => 400;
    public override int MinHeight => 400;
    public override bool CanDragAndResize => false;
    public override bool ShowByDefault => false;
    public override bool DisplayTitleBar => false;
    public override bool CanClickThroughPanel => true;
    public override Color BackgroundColor => UIPalette.Transparent;
    public override bool AlwaysOnTop => false;
    public override Positions Position => Positions.BottomCenter;

    private Text _label;

    protected override void ConstructPanelContent()
    {
        base.ConstructPanelContent();
        var imageContainer = UIFactory.CreateVerticalGroup(ContentRoot, "ImageContainer",
            false,
            false,
            true,
            true,
            childAlignment: TextAnchor.MiddleCenter);
        imageContainer.GetComponent<Image>().color = UIPalette.Transparent;
        UIFactory.SetLayoutElement(imageContainer, minHeight: MinHeight, flexibleHeight: 0, minWidth: MinWidth,
            flexibleWidth: 0);

        var image = UIFactory.CreateHorizontalGroup(imageContainer, "Image",
            false,
            false,
            false,
            false);
        var img = image.GetComponent<Image>();
        img.color = Palette.EnabledColor;
        img.sprite = StickyBomber.StickyButton;
        _label = UIFactory.CreateLabel(imageContainer, "Title", "You have a sticky bomb, it will explode in 10 seconds.", TextAnchor.MiddleCenter,
            Palette.ImpostorRed, true, 28);
        _label.fontStyle = FontStyle.Bold;
        UIFactory.SetLayoutElement(_label.gameObject, minWidth: MinWidth, flexibleWidth: 0, minHeight: 40,
            flexibleHeight: 0);
    }

    public void UpdateTimer(float timer)
    {
        var seconds = Mathf.RoundToInt(timer);
        _label.text = $"You have a sticky bomb, it will explode in {seconds} seconds.";
    }
}