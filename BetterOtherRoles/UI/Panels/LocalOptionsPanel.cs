using System.Collections.Generic;
using System.Linq;
using BetterOtherRoles.Modules;
using BetterOtherRoles.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UI;

namespace BetterOtherRoles.UI.Panels;

public class LocalOptionsPanel : WrappedPanel
{
    public LocalOptionsPanel(UIBase owner) : base(owner)
    {
    }
    
    public override string Name => $"<color=#fcba03>Better</color><color=#ff351f>OtherRoles</color> v{BetterOtherRolesPlugin.VersionString}";

    public override int MinWidth => 600;
    public override int MinHeight => Screen.height;
    public override bool CanDragAndResize => false;
    public override bool ShowByDefault => false;
    public override bool DisplayTitleBar => true;
    public override Color BackgroundColor => UIPalette.Dark;
    public override bool AlwaysOnTop => true;
    public override Positions Position => Positions.TopLeft;
    
    private Text _categoryTitle = null!;

    protected override void ConstructPanelContent()
    {
        base.ConstructPanelContent();
        var titleContainer = UIFactory.CreateHorizontalGroup(ContentRoot, "TitleContainer", false, false, true, true,
            0, new Vector4(5f, 5f, 0f, 0f), UIPalette.Transparent);
        UIFactory.SetLayoutElement(titleContainer, minHeight: 40, flexibleHeight: 0, minWidth: MinWidth,
            flexibleWidth: 0);

        _categoryTitle = UIFactory.CreateLabel(titleContainer, "Title", "Local Options", TextAnchor.MiddleCenter,
            UIPalette.Secondary, true, 28);
        _categoryTitle.fontStyle = FontStyle.Bold;
        UIFactory.SetLayoutElement(_categoryTitle.gameObject, minWidth: MinWidth - 66, flexibleWidth: 0, minHeight: 40,
            flexibleHeight: 0);
        ConstructCustomOptions();
    }

    private void ConstructCustomOptions()
    {
        BetterOtherRolesPlugin.Logger.LogWarning($"Local options required");
        var scrollerObject = UIFactory.CreateScrollView(ContentRoot, "CustomOptionsScrollView", out var content, out var _);
        UIFactory.SetLayoutElement(scrollerObject, MinWidth, 25, 0, 9999);
        UIFactory.SetLayoutElement(content, MinWidth, 25, 0, 9999);
        
        var seeInfos = new LocalOptionEditor("Ghosts See Tasks & Other Info", this, content, BetterOtherRolesPlugin.GhostsSeeInformation);
        seeInfos.OnUpdated += value =>
        {
            TORMapOptions.ghostsSeeInformation = value;
        };

        var seeVotes = new LocalOptionEditor("Ghosts Can See Votes", this, content, BetterOtherRolesPlugin.GhostsSeeVotes);
        seeVotes.OnUpdated += value =>
        {
            TORMapOptions.ghostsSeeVotes = value;
        };

        var seeRoles = new LocalOptionEditor("Ghosts Can See Roles", this, content, BetterOtherRolesPlugin.GhostsSeeRoles);
        seeRoles.OnUpdated += value =>
        {
            TORMapOptions.ghostsSeeRoles = value;
        };
        
        var seeModifier = new LocalOptionEditor("Ghosts Can Additionally See Modifier", this, content, BetterOtherRolesPlugin.GhostsSeeModifier);
        seeModifier.OnUpdated += value =>
        {
            TORMapOptions.ghostsSeeModifier = value;
        };
        
        var showRoleSummary = new LocalOptionEditor("Show Role Summary", this, content, BetterOtherRolesPlugin.ShowRoleSummary);
        showRoleSummary.OnUpdated += value =>
        {
            TORMapOptions.showRoleSummary = value;
        };
        
        var showColorType = new LocalOptionEditor("Show Lighter / Darker Color", this, content, BetterOtherRolesPlugin.ShowLighterDarker);
        showColorType.OnUpdated += value =>
        {
            TORMapOptions.showLighterDarker = value;
        };
        
        var enableSoundEffects = new LocalOptionEditor("Enable Sound Effects", this, content, BetterOtherRolesPlugin.EnableSoundEffects);
        enableSoundEffects.OnUpdated += value =>
        {
            TORMapOptions.enableSoundEffects = value;
        };
    }
    
    public override void SetActive(bool active)
    {
        if (active)
        {
            // UpdateEditable();
        }
        UIManager.Overlay?.SetActive(active);
        base.SetActive(active);
        Owner.SetOnTop();
    }
}