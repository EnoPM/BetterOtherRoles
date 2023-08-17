using BetterOtherRoles.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Panels;

namespace BetterOtherRoles.UI;

public abstract class WrappedPanel : PanelBase
{
    protected WrappedPanel(UIBase owner) : base(owner)
    {
        UIManager.Panels.Add(this);
    }
    
    public enum Positions
    {
        TopLeft,
        TopRight,
        TopCenter,
        MiddleCenter,
        MiddleRight,
        MiddleLeft,
        BottomLeft,
        BottomCenter,
        BottomRight
    }
    
    public virtual bool ShowByDefault => false;
    public virtual bool DisplayTitleBar => false;
    public virtual Color BackgroundColor => UIPalette.Primary;
    public virtual Positions Position => Positions.MiddleCenter;
    public virtual Vector4 Padding => Vector4.zero;
    public virtual int Spacing => 0;
    public virtual bool CanClickThroughPanel => false;
    public float InnerWidth => MinWidth - Padding.y - Padding.w;
    public float InnerHeight => MinHeight - Padding.x - Padding.z;
    public HeaderBar? Header { get; protected set; }
    
    public override Vector2 DefaultAnchorMin => Vector2.zero;
    public override Vector2 DefaultAnchorMax => Vector2.zero;

    public override bool CanDragAndResize => false;

    public override void ConstructUI()
    {
        base.ConstructUI();
        UIRoot.GetComponent<Image>().color = UIPalette.Transparent;
        SetBackgroundColor(BackgroundColor);
        TitleBar.SetActive(false);
        var layout = ContentRoot.GetComponent<VerticalLayoutGroup>();
        layout.spacing = 0f;
        layout.padding.top = 0;
        layout.padding.right = 0;
        layout.padding.bottom = 0;
        layout.padding.left = 0;
    }

    protected override void ConstructPanelContent()
    {
        // Header
        Header = new HeaderBar(this, Name);
        TitleBar.SetActive(false);
        if (!DisplayTitleBar)
        {
            Header.SetActive(false);
        }
    }

    public virtual void OnCloseButtonClick()
    {
        OnClosePanelClicked();
    }

    protected override void OnClosePanelClicked()
    {
        SetActive(false);
    }

    protected void SetBackgroundColor(Color color)
    {
        ContentRoot.GetComponent<Image>().color = color;
    }
    
    public override void EnsureValidPosition()
    {
        var screenHeight = Screen.height;
        var screenWidth = Screen.width;

        switch (Position)
        {
            case Positions.MiddleCenter:
                Rect.position = new Vector3(screenWidth / 2f - MinWidth / 2f, screenHeight / 2f + MinHeight / 2f,
                    Rect.position.z);
                break;
            case Positions.TopLeft:
                Rect.position = new Vector3(0f, screenHeight, Rect.position.z);
                break;
            case Positions.TopCenter:
                Rect.position = new Vector3(screenWidth / 2f - MinWidth / 2f, screenHeight, Rect.position.z);
                break;
            case Positions.MiddleLeft:
                Rect.position = new Vector3(0f, screenHeight / 2f + MinHeight / 2f);
                break;
            case Positions.TopRight:
                Rect.position = new Vector3(screenWidth - MinWidth, screenHeight, Rect.position.z);
                break;
            case Positions.MiddleRight:
                Rect.position = new Vector3(screenWidth - MinWidth, screenHeight / 2f + MinHeight / 2f, Rect.position.z);
                break;
            case Positions.BottomLeft:
                Rect.position = new Vector3(0f, MinHeight, Rect.position.z);
                break;
            case Positions.BottomCenter:
                Rect.position = new Vector3(screenWidth / 2f - MinWidth / 2f, MinHeight, Rect.position.z);
                break;
            case Positions.BottomRight:
                Rect.position = new Vector3(screenWidth - MinWidth, MinHeight, Rect.position.z);
                break;
        }
        
        Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MinWidth);
        Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, MinHeight);
    }
    
    public override void SetActive(bool active)
    {
        base.SetActive(active);
        PassiveButtonManagerPatches.CheckIsUiOpen();
    }
}