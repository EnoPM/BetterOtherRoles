using System.Collections.Generic;
using System.Linq;
using BetterOtherRoles.Modules;
using BetterOtherRoles.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UI;

namespace BetterOtherRoles.UI.Panels;

public class CustomOptionsPanel : WrappedPanel
{
    public CustomOptionsPanel(UIBase owner) : base(owner)
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

    private readonly Dictionary<int, GameObject> _categoryHolders = new();

    private readonly Dictionary<CustomOption.CustomOptionType, string> _categories = new()
    {
        { CustomOption.CustomOptionType.General, "Mods settings" },
        { CustomOption.CustomOptionType.Guesser, "Guesser Mode Settings" },
        { CustomOption.CustomOptionType.Impostor, "Impostor Roles Settings" },
        { CustomOption.CustomOptionType.Neutral, "Neutral Roles Settings" },
        { CustomOption.CustomOptionType.Crewmate, "Crewmate Roles Settings" },
        { CustomOption.CustomOptionType.Modifier, "Modifier Settings" },
        { CustomOption.CustomOptionType.HideNSeekMain, "Hide 'N Seek Settings" },
        { CustomOption.CustomOptionType.HideNSeekRoles, "Hide 'N Seek Roles Settings" },
    };
    
    private List<CustomOption.CustomOptionType> Categories
    {
        get
        {
            return TORMapOptions.gameMode switch
            {
                CustomGamemodes.Classic => new List<CustomOption.CustomOptionType>
                {
                    CustomOption.CustomOptionType.General,
                    CustomOption.CustomOptionType.Impostor,
                    CustomOption.CustomOptionType.Neutral,
                    CustomOption.CustomOptionType.Crewmate,
                    CustomOption.CustomOptionType.Modifier
                },
                CustomGamemodes.Guesser => new List<CustomOption.CustomOptionType>
                {
                    CustomOption.CustomOptionType.General,
                    CustomOption.CustomOptionType.Guesser,
                    CustomOption.CustomOptionType.Impostor,
                    CustomOption.CustomOptionType.Neutral,
                    CustomOption.CustomOptionType.Crewmate,
                    CustomOption.CustomOptionType.Modifier
                },
                _ => new List<CustomOption.CustomOptionType>
                {
                    CustomOption.CustomOptionType.General,
                    CustomOption.CustomOptionType.HideNSeekMain,
                    CustomOption.CustomOptionType.HideNSeekRoles
                },
            };
        }
    }
    
    private Text _categoryTitle = null!;
    private int _currentIndex;

    protected override void ConstructPanelContent()
    {
        base.ConstructPanelContent();
        var titleContainer = UIFactory.CreateHorizontalGroup(ContentRoot, "TitleContainer", false, false, true, true,
            0, new Vector4(5f, 5f, 0f, 0f), UIPalette.Transparent);
        UIFactory.SetLayoutElement(titleContainer, minHeight: 40, flexibleHeight: 0, minWidth: MinWidth,
            flexibleWidth: 0);

        var previousButton = UIFactory.CreateButton(titleContainer, "PreviousButton", "\u27a4", UIPalette.Black);
        RuntimeHelper.SetColorBlock(previousButton.Component, UIPalette.Black, UIPalette.Black,
            UIPalette.Black, UIPalette.Black);
        UIFactory.SetLayoutElement(previousButton.GameObject, 30, 30, 0, 0);
        previousButton.ButtonText.transform.Rotate(Vector3.up, 180f);
        previousButton.ButtonText.fontSize = 20;
        previousButton.OnClick = PreviousCategory;

        _categoryTitle = UIFactory.CreateLabel(titleContainer, "Title", "Custom options", TextAnchor.MiddleCenter,
            UIPalette.Secondary, true, 20);
        _categoryTitle.fontStyle = FontStyle.Bold;
        UIFactory.SetLayoutElement(_categoryTitle.gameObject, minWidth: MinWidth - 66, flexibleWidth: 0, minHeight: 30,
            flexibleHeight: 0);

        var nextButton = UIFactory.CreateButton(titleContainer, "NextButton", "\u27a4", UIPalette.Black);
        RuntimeHelper.SetColorBlock(nextButton.Component, UIPalette.Black, UIPalette.Black,
            UIPalette.Black, UIPalette.Black);
        UIFactory.SetLayoutElement(nextButton.GameObject, 30, 30, 0, 0);
        nextButton.ButtonText.fontSize = 20;
        nextButton.OnClick = NextCategory;
        ConstructCustomOptions();
    }

    private void ResetCategories()
    {
        HideCurrentCategory();
        _currentIndex = 0;
        UpdateCurrentCategory();
    }
    
    public void CheckForUpdate(CustomGamemodes oldGameMode, CustomGamemodes newGameMode)
    {
        if (oldGameMode == newGameMode) return;
        ResetCategories();
    }

    private void ConstructCustomOptions()
    {
        var i = 0;
        foreach (var category in _categories)
        {
            _categoryHolders[i] =
                UIFactory.CreateScrollView(ContentRoot, "CustomOptionsScrollView", out var content, out var _);
            UIFactory.SetLayoutElement(_categoryHolders[i], MinWidth, 25, 0, 9999);
            UIFactory.SetLayoutElement(content, MinWidth, 25, 0, 9999);
            var options = CustomOption.options.Where(o => o.type == category.Key);
            foreach (var option in options)
            {
                _ = new CustomOptionViewer(this, content, option);
            }
            _categoryHolders[i].SetActive(false);
            i++;
        }
        _currentIndex = 0;
        UpdateCurrentCategory();
    }
    
    private void HideCurrentCategory()
    {
        CurrentCategoryHolder.SetActive(false);
    }

    private void PreviousCategory()
    {
        HideCurrentCategory();
        _currentIndex--;
        if (_currentIndex < 0)
        {
            _currentIndex = Categories.Count - 1;
        }

        UpdateCurrentCategory();
    }

    private void NextCategory()
    {
        HideCurrentCategory();
        _currentIndex++;
        if (_currentIndex >= Categories.Count)
        {
            _currentIndex = 0;
        }

        UpdateCurrentCategory();
    }

    private void UpdateCurrentCategory()
    {
        CurrentCategoryHolder.SetActive(true);
    }

    private GameObject CurrentCategoryHolder
    {
        get
        {
            var categories = Categories;
            var key = categories[_currentIndex];
            var category = _categories[key];
            _categoryTitle.text = category;
            return _categoryHolders[_categories.Keys.ToList().IndexOf(key)];
        }
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