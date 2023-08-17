using UnityEngine;
using UniverseLib.UI;

namespace BetterOtherRoles.UI.Panels;

public class OverlayPanel : WrappedPanel
{
    public OverlayPanel(UIBase owner) : base(owner)
    {
    }

    public override string Name => "Loading";

    public override int MinWidth => Screen.width;
    public override int MinHeight => Screen.height;
    public override Vector2 DefaultAnchorMin => new(0.125f, 0.175f);
    public override Vector2 DefaultAnchorMax => new(0.325f, 0.925f);
    public override Vector2 DefaultPosition => new(0f, 0f);
    public override bool CanDragAndResize => false;
    public override bool ShowByDefault => false;
    public override bool DisplayTitleBar => false;
    public override Color BackgroundColor => new(0f, 0f, 0f, 0.6f);

    public override Positions Position => Positions.TopLeft;
}