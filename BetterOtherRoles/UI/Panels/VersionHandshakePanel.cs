using System.Text;
using BetterOtherRoles.Eno;
using BetterOtherRoles.Modules;
using BetterOtherRoles.Players;
using InnerNet;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;

namespace BetterOtherRoles.UI.Panels;

public class VersionHandshakePanel : WrappedPanel
{
    public VersionHandshakePanel(UIBase owner) : base(owner)
    {
    }

    public override string Name =>
        $"<color=#fcba03>Better</color><color=#ff351f>OtherRoles</color> v{BetterOtherRolesPlugin.VersionString}";

    public override int MinWidth => 250;
    public override int MinHeight => 450;
    public override bool CanDragAndResize => false;
    public override bool ShowByDefault => false;
    public override bool DisplayTitleBar => false;
    public override bool CanClickThroughPanel => true;
    public override Color BackgroundColor => UIPalette.Transparent;
    public override bool AlwaysOnTop => false;
    public override Positions Position => Positions.MiddleRight;

    private Text _content;

    protected override void ConstructPanelContent()
    {
        base.ConstructPanelContent();
        var container = UIFactory.CreateVerticalGroup(ContentRoot, "ImageContainer",
            false,
            false,
            true,
            true,
            childAlignment: TextAnchor.UpperLeft);
        container.GetComponent<Image>().color = UIPalette.Transparent;
        UIFactory.SetLayoutElement(container, minHeight: MinHeight, flexibleHeight: 0, minWidth: MinWidth,
            flexibleWidth: 0);

        _content = UIFactory.CreateLabel(container, "TextContent", "", TextAnchor.UpperLeft,
            UIPalette.Secondary, true, 18);
        UIFactory.SetLayoutElement(_content.gameObject, minWidth: MinWidth, flexibleWidth: 0, minHeight: 40,
            flexibleHeight: 0);
    }

    public void UpdateChecks()
    {
        if (AmongUsClient.Instance.GameState != InnerNetClient.GameStates.Joined || CachedPlayer.AllPlayers == null)
        {
            SetActive(false);
            return;
        }
        SetActive(true);
        var text = new StringBuilder();
        foreach (var player in CachedPlayer.AllPlayers)
        {
            if (player.Data == null) continue;
            var clientId = player.PlayerControl.OwnerId;
            var symbol = Helpers.cs(Palette.ImpostorRed, "✖");
            var playerName = player.Data.PlayerName;
            var errorMessage = string.Empty;
            if (AmongUsClient.Instance.HostId == clientId)
            {
                playerName = Helpers.cs(Color.magenta, playerName);
            }
            if (!VersionHandshake.Has(clientId))
            {
                errorMessage = "missing or bad mod";
            }
            else if (!VersionHandshake.Find(clientId).Version.Equals(VersionHandshake.Instance.Version))
            {
                errorMessage = $"wrong version (v{VersionHandshake.Find(clientId).Version.ToString()})";
            }
            else if (!VersionHandshake.Find(clientId).GuidMatch())
            {
                errorMessage = "wrong build";
            }
            else
            {
                symbol = Helpers.cs(Palette.AcceptedGreen, "✔️");
                var version = VersionHandshake.Find(clientId);
                if (version.Flags.TryGetValue("LOBBY_NAME_COLOR", out var lobbyNameColor))
                {
                    try
                    {
                        var playerNameText = player.PlayerControl.cosmetics.nameText;
                        if (lobbyNameColor.ToLower() == "rainbow")
                        {
                            playerNameText.color = FirstKillShield.ShieldColor;
                        } else if (lobbyNameColor.StartsWith("#"))
                        {
                            playerNameText.color = Helpers.ColorFromHex(lobbyNameColor);
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            var line = $"{symbol} {playerName}";
            if (errorMessage != string.Empty)
            {
                line += $": {Helpers.cs(Palette.ImpostorRed, errorMessage)}";
            }

            text.AppendLine(line);
        }
        
        _content.text = text.ToString();
    }
}