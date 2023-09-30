using System.Collections.Generic;
using System.Linq;
using BetterOtherRoles.Modules;
using BetterOtherRoles.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UI;

namespace BetterOtherRoles.UI.Panels;

public class CreditsPanel : WrappedPanel
{
    public CreditsPanel(UIBase owner) : base(owner)
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

        _categoryTitle = UIFactory.CreateLabel(titleContainer, "Title", "Mods Credits", TextAnchor.MiddleCenter,
            UIPalette.Secondary, true, 28);
        _categoryTitle.fontStyle = FontStyle.Bold;
        UIFactory.SetLayoutElement(_categoryTitle.gameObject, minWidth: MinWidth - 66, flexibleWidth: 0, minHeight: 40,
            flexibleHeight: 0);
        ConstructCredits();
    }

    private void ConstructCredits()
    {
        BetterOtherRolesPlugin.Logger.LogWarning($"Local options required");
        var scrollerObject = UIFactory.CreateScrollView(ContentRoot, "CustomOptionsScrollView", out var content, out var _);
        UIFactory.SetLayoutElement(scrollerObject, MinWidth, 25, 0, 9999);
        UIFactory.SetLayoutElement(content, MinWidth, 25, 0, 9999);

        var botTitle = UIFactory.CreateLabel(content, "BORTitle", @"
<color=#fcba03>Better</color><color=#ff351f>OtherRoles</color> Credits:

Based on <color=#ff351f>TheOtherRoles</color>
Created by <color=#FCCE03FF>EnoPM</color>
");
        botTitle.fontStyle = FontStyle.Bold;
        botTitle.fontSize = 20;
        botTitle.alignment = TextAnchor.MiddleCenter;

        var borCredits = UIFactory.CreateLabel(content, "BORCredits", @"
<b>BetterPolus map modifications:</b>
Brybry

<b>Undertaker integration:</b>
Thilladon

<b>Custom key binds:</b>
Dadoum
");
        borCredits.alignment = TextAnchor.MiddleCenter;

        var torCreditsTitle = UIFactory.CreateLabel(content, "TORTitle", @"

<color=#ff351f>TheOtherRoles</color> Credits:

Modded by <color=#FCCE03FF>Eisbison</color>, <color=#FCCE03FF>EndOfFile</color>
<color=#FCCE03FF>Thunderstorm584</color>, <color=#FCCE03FF>Mallöris</color> & <color=#FCCE03FF>Gendelo</color>
Design by <color=#FCCE03FF>Bavari</color>
");
        torCreditsTitle.fontStyle = FontStyle.Bold;
        torCreditsTitle.fontSize = 20;
        torCreditsTitle.alignment = TextAnchor.MiddleCenter;
        var torCredits = UIFactory.CreateLabel(content, "TORContributors", @"
<b>Github Contributors:</b>

Alex2911    amsyarasyiq    MaximeGillot
Psynomit    probablyadnf    JustASysAdmin

<b>Discord Moderators:</b>

Streamblox    Draco Cordraconis
Thanks to all our discord helpers!

Thanks to miniduikboot & GD for hosting modded servers

<b>Other Credits & Resources:</b>

OxygenFilter - For the versions v2.3.0 to v2.6.1, we were using the OxygenFilter for automatic deobfuscation
Reactor - The framework used for all versions before v2.0.0, and again since 4.2.0
BepInEx - Used to hook game functions
Essentials - Custom game options by DorCoMaNdO:
Before v1.6: We used the default Essentials release
v1.6-v1.8: We slightly changed the default Essentials.
v2.0.0 and later: As we were not using Reactor anymore, we are using our own implementation, inspired by the one from DorCoMaNdO
Jackal and Sidekick - Original idea for the Jackal and Sidekick came from Dhalucard
Among-Us-Love-Couple-Mod - Idea for the Lovers modifier comes from Woodi-dev
Jester - Idea for the Jester role came from Maartii
ExtraRolesAmongUs - Idea for the Engineer and Medic role came from NotHunter101. Also some code snippets from their implementation were used.
Among-Us-Sheriff-Mod - Idea for the Sheriff role came from Woodi-dev
TooManyRolesMods - Idea for the Detective and Time Master roles comes from Hardel-DW. Also some code snippets from their implementation were used.
TownOfUs - Idea for the Swapper, Shifter, Arsonist and a similar Mayor role came from Slushiegoose
Ottomated - Idea for the Morphling, Snitch and Camouflager role came from Ottomated
Crowded-Mod - Our implementation for 10+ player lobbies was inspired by the one from the Crowded Mod Team
Goose-Goose-Duck - Idea for the Vulture role came from Slushiegoose
TheEpicRoles - Idea for the first kill shield (partly) and the tabbed option menu (fully + some code), by LaicosVK DasMonschta Nova
");
        torCredits.alignment = TextAnchor.MiddleCenter;
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