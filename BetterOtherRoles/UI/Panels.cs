using BetterOtherRoles.UI.Panels;

namespace BetterOtherRoles.UI;

public static partial class UIManager
{
    private static bool _initialized = false;
    public static OverlayPanel Overlay { get; private set; }
    public static CustomOptionsPanel CustomOptionsPanel { get; private set; }
    public static LocalOptionsPanel LocalOptionsPanel { get; private set; }
    public static CreditsPanel CreditsPanel { get; private set; }
    public static StickyBombPanel StickyBombPanel { get; private set; }
    public static VersionHandshakePanel VersionHandshakePanel { get; private set; }
    public static UpdatePluginPanel UpdatePluginPanel { get; private set; }
    
    private static void InitPanels()
    {
        if (UiBase == null) return;

        Overlay = new OverlayPanel(UiBase);
        CustomOptionsPanel = new CustomOptionsPanel(UiBase);
        LocalOptionsPanel = new LocalOptionsPanel(UiBase);
        CreditsPanel = new CreditsPanel(UiBase);
        StickyBombPanel = new StickyBombPanel(UiBase);
        VersionHandshakePanel = new VersionHandshakePanel(UiBase);
        UpdatePluginPanel = new UpdatePluginPanel(UiBase);

        _initialized = true;
    }

    public static void CloseAllPanels()
    {
        if (!_initialized) return;
        CustomOptionsPanel.SetActive(false);
        LocalOptionsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
        StickyBombPanel.SetActive(false);
        VersionHandshakePanel.SetActive(false);
    }
}