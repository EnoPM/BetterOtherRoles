using System.Collections.Generic;
using AmongUsSpecimen;
using AmongUsSpecimen.ModOptions;
using BetterOtherRoles.Roles;
using UnityEngine;
using static AmongUsSpecimen.ModOptions.Helpers;
using static AmongUsSpecimen.OptionTabs;

namespace BetterOtherRoles.Options;

[ModOptionContainer(ContainerType.Tabs)]
public static partial class CustomOptionHolder
{
    public static readonly Dictionary<byte, byte[]> BlockedRolePairings = new ()
    {
        [(byte)RoleId.Vampire] =  [(byte)RoleId.Warlock],
        [(byte)RoleId.Warlock] =  [(byte)RoleId.Vampire],
        [(byte)RoleId.Spy] =  [(byte)RoleId.Mini],
        [(byte)RoleId.Mini] =  [(byte)RoleId.Spy],
        [(byte)RoleId.Vulture] =  [(byte)RoleId.Cleaner],
        [(byte)RoleId.Cleaner] =  [(byte)RoleId.Vulture],
    };
    
    static CustomOptionHolder()
    {
        MainTab.OverrideSprite(GetSprite("SettingsTab"));
        GuesserTab = new ModOptionTab("Guesser", "Guesser Settings", GetSprite("TabIconGuesserSettings"),
            AfterTab(MainTab));
        GuesserTab.SetActive(false);
        CrewmateTab = new ModOptionTab("Crewmate", "Crewmate Settings", GetSprite("TabIconCrewmate"));
        NeutralTab = new ModOptionTab("Neutral", "Neutral Settings", GetSprite("TabIconNeutral"));
        ImpostorTab = new ModOptionTab("Impostor", "Impostor Settings", GetSprite("TabIconImpostor"));
        ModifierTab = new ModOptionTab("Modifier", "Modifier Settings", GetSprite("TabIconModifier"));
        
        RandomMap = OutsidePreset(MainTab.BoolOption("Random Map", false));

        TheSkeldMap = new RandomMapModOptionMap("The Skeld");
        PolusMap = new RandomMapModOptionMap("Polus");
        MiraHqMap = new RandomMapModOptionMap("Mira HQ");
        AirshipMap = new RandomMapModOptionMap("Airship");
        TheFungleMap = new RandomMapModOptionMap("The Fungle");

        GameEventManager.HostChanged += OnHostChanged;

        EnableBetterPolus = MainTab.BoolOption("Better Polus", false);
        PolusReactorCountdown = MainTab.FloatOption("Polus Reactor Countdown", 10f, 120f, 2.5f, 40f, EnableBetterPolus, suffix: "s");
        
        EnableBetterSkeld = MainTab.BoolOption("Better Skeld", false);
        BetterSkeldEnableAdmin = MainTab.BoolOption("Enable Admin Table", false, EnableBetterSkeld);
        BetterSkeldEnableVitals = MainTab.BoolOption("Enable Vitals", false, EnableBetterSkeld);

        if (Utilities.EventUtility.canBeEnabled)
        {
            EnableCodenameHorseMode = MainTab.BoolOption(Cs(Color.green, "Enable Codename Horsemode"), true);
            EnableCodenameDisableHorses =
                MainTab.BoolOption(Cs(Color.green, "Disable Horses"), false, EnableCodenameHorseMode);
        }

        var yellow = new Color(204f / 255f, 204f / 255f, 0, 1f);
        ActivateRoles = MainTab.BoolOption(Cs(yellow, "Enable Mod Roles And Block Vanilla Roles"), true);

        GuesserMode = MainTab.BoolOption("Guesser Mode", false, ActivateRoles);
        GuesserMode.ValueChanged += () => GuesserTab.SetActive(GuesserMode);

        MaxNumberOfMeetings = MainTab.FloatOption("Number Of Meetings (excluding Mayor meeting)", 0f, 15f, 1f, 10f);
        RandomizePlayersInMeeting = MainTab.BoolOption("Randomize Players Order In Meetings", false, MaxNumberOfMeetings);
        BlockSkippingInEmergencyMeetings = MainTab.BoolOption("Block Skipping In Emergency Meetings", false, MaxNumberOfMeetings);
        NoVoteIsSelfVote = MainTab.BoolOption("No Vote Is Self Vote", false, BlockSkippingInEmergencyMeetings);

        RandomizeWireTaskPositions = MainTab.BoolOption("Randomize Wire Tasks Positions", false);
        
        RandomizeUploadTaskPosition = MainTab.BoolOption("Randomize Upload Task Positions", false);

        HidePlayerNames = MainTab.BoolOption("Hide Player Names", false);

        AllowParallelMedBayScans = MainTab.BoolOption("Allow Parallel MedBay Scans", true);
        RandomizePositionDuringScan = MainTab.BoolOption("Randomize Position During Scan", true, AllowParallelMedBayScans);

        ShieldFirstKill = MainTab.BoolOption("Shield Last Game First Kill", false);
        ExpireFirstKillShield = MainTab.BoolOption("Expire shield with duration", false, ShieldFirstKill);
        FirstKillShieldDuration = MainTab.FloatOption("Shield duration", 10f, 300f, 5f, 80f, ExpireFirstKillShield, suffix: "s");

        FinishTasksBeforeHauntingOrZoomingOut = MainTab.BoolOption("Finish Tasks Before Haunting Or Zooming Out", true);
        
        CamsNightVision = MainTab.BoolOption("Cams Switch To Night Vision If Lights Are Off", false);
        CamsNoNightVisionIfImpVision = MainTab.BoolOption("Impostor Vision Ignores Night Vision Cams", false, CamsNightVision);
        
        FillCrewmateRoles = Header(CrewmateTab.BoolOption(Cs(yellow, "Fill Crewmate Roles"), false, ActivateRoles));
        MinCrewmateRoles =
            Inverted(CrewmateTab.FloatOption(Cs(yellow, "Minimum Crewmate Roles"), 0f, 15f, 1f, 15f,
                FillCrewmateRoles));
        MaxCrewmateRoles =
            Inverted(CrewmateTab.FloatOption(Cs(yellow, "Maximum Crewmate Roles"), 0f, 15f, 1f, 15f,
                FillCrewmateRoles));

        MinNeutralRoles =
            Header(NeutralTab.FloatOption(Cs(yellow, "Minimum Neutral Roles"), 0f, 15f, 1f, 15f, ActivateRoles));
        MaxNeutralRoles = NeutralTab.FloatOption(Cs(yellow, "Maximum Neutral Roles"), 0f, 15f, 1f, 15f, ActivateRoles);

        MinImpostorRoles =
            Header(ImpostorTab.FloatOption(Cs(yellow, "Minimum Impostor Roles"), 0f, 15f, 1f, 15f, ActivateRoles));
        MaxImpostorRoles =
            ImpostorTab.FloatOption(Cs(yellow, "Maximum Impostor Roles"), 0f, 15f, 1f, 15f, ActivateRoles);

        MinModifiers = Header(ModifierTab.FloatOption(Cs(yellow, "Minimum Modifier"), 0f, 15f, 1f, 15f, ActivateRoles));
        MaxModifiers = ModifierTab.FloatOption(Cs(yellow, "Maximum Modifier"), 0f, 15f, 1f, 15f, ActivateRoles);

        MafiaSpawnRate = Header(ImpostorTab.SpawnRateOption(Cs(Janitor.color, "Mafia"), ActivateRoles));
        JanitorCooldown = ImpostorTab.CooldownOption("Janitor Cooldown", MafiaSpawnRate);

        MorphlingSpawnRate = Header(ImpostorTab.SpawnRateOption(Cs(Morphling.color, "Morphling"), ActivateRoles));
        MorphlingCooldown = ImpostorTab.CooldownOption("Morph Cooldown", MorphlingSpawnRate);
        MorphlingDuration = ImpostorTab.DurationOption("Morph Duration", MorphlingSpawnRate);

        CamouflagerSpawnRate = Header(ImpostorTab.SpawnRateOption(Cs(Camouflager.color, "Camouflager"), ActivateRoles));
        CamouflagerCooldown = ImpostorTab.CooldownOption("Camouflager Cooldown", CamouflagerSpawnRate);
        CamouflagerDuration = ImpostorTab.DurationOption("Camo Duration", CamouflagerSpawnRate);

        VampireSpawnRate = Header(ImpostorTab.SpawnRateOption(Cs(Vampire.color, "Vampire"), ActivateRoles));
        VampireKillDelay = ImpostorTab.DurationOption("Vampire Kill Delay", VampireSpawnRate);
        VampireCooldown = ImpostorTab.CooldownOption("Vampire Cooldown", VampireSpawnRate);
        VampireFirstCooldown = ImpostorTab.CooldownOption("Vampire First Cooldown", VampireSpawnRate);
        VampireCanKillNearGarlics = ImpostorTab.BoolOption("Vampire Can Kill Near Garlics", true, VampireSpawnRate);

        EraserSpawnRate = Header(ImpostorTab.SpawnRateOption(Cs(Eraser.color, "Eraser"), ActivateRoles));
        EraserCooldown = ImpostorTab.CooldownOption("Eraser Cooldown", EraserSpawnRate);
        EraserCanEraseAnyone = ImpostorTab.BoolOption("Eraser Can Erase Anyone", false, EraserSpawnRate);

        TricksterSpawnRate = Header(ImpostorTab.SpawnRateOption(Cs(Trickster.color, "Trickster"), ActivateRoles));
        TricksterPlaceBoxCooldown = ImpostorTab.CooldownOption("Trickster Place Box Cooldown", TricksterSpawnRate);
        TricksterLightsOutCooldown = ImpostorTab.CooldownOption("Trickster Lights Out Cooldown", TricksterSpawnRate);
        TricksterLightsOutDuration = ImpostorTab.DurationOption("Trickster Lights Out Duration", TricksterSpawnRate);

        CleanerSpawnRate = Header(ImpostorTab.SpawnRateOption(Cs(Cleaner.color, "Cleaner"), ActivateRoles));
        CleanerCooldown = ImpostorTab.CooldownOption("Cleaner Cooldown", CleanerSpawnRate);
        
        WarlockSpawnRate = Header(ImpostorTab.SpawnRateOption(Cs(Warlock.color, "Warlock"), ActivateRoles));
        WarlockCooldown = ImpostorTab.CooldownOption("Warlock Cooldown", WarlockSpawnRate);
        WarlockFirstCooldown = ImpostorTab.CooldownOption("Warlock First Cooldown", WarlockSpawnRate);
        WarlockRootTime = ImpostorTab.FloatOption("Warlock Root Time", 0f, 15f, 1f, 5f, WarlockSpawnRate, suffix: "s");

        BountyHunterSpawnRate =
            Header(ImpostorTab.SpawnRateOption(Cs(BountyHunter.color, "Bounty Hunter"), ActivateRoles));
        BountyHunterBountyDuration = ImpostorTab.FloatOption("Duration After Which Bounty Changes", 10f, 180f, 10f, 60f,
            BountyHunterSpawnRate, suffix: "s");
        BountyHunterReducedCooldown = ImpostorTab.FloatOption("Cooldown After Killing Bounty", 0f, 30f, 2.5f, 2.5f,
            BountyHunterSpawnRate, suffix: "s");
        BountyHunterPunishmentTime = ImpostorTab.FloatOption("Additional Cooldown After Killing Others", 0f, 60f, 2.5f,
            15f, BountyHunterSpawnRate, suffix: "s", prefix: "+");
        BountyHunterShowArrow =
            ImpostorTab.BoolOption("Show Arrow Pointing Towards The Bounty", true, BountyHunterSpawnRate);
        BountyHunterArrowUpdateInterval = ImpostorTab.FloatOption("Arrow Update Interval", 2.5f, 60f, 2.5f, 15f,
            BountyHunterShowArrow, suffix: "s");

        WitchSpawnRate = Header(ImpostorTab.SpawnRateOption(Cs(Witch.color, "Witch"), ActivateRoles));
        WitchCooldown = ImpostorTab.CooldownOption("Witch Spell Casting Cooldown", WitchSpawnRate);
        WitchFirstCooldown = ImpostorTab.CooldownOption("Witch First Spell Casting Cooldown", WitchSpawnRate);
        WitchAdditionalCooldown = ImpostorTab.FloatOption("Witch Additional Cooldown", 0f, 60f, 5f, 10f, WitchSpawnRate,
            suffix: "s", prefix: "+");
        WitchCanSpellAnyone = ImpostorTab.BoolOption("Witch Can Spell Anyone", false, WitchSpawnRate);
        WitchSpellCastingDuration = ImpostorTab.ApplyDurationOption("Spell Casting Duration", WitchSpawnRate);
        WitchTriggerBothCooldown = ImpostorTab.BoolOption("Trigger Both Cooldowns", true, WitchSpawnRate);
        WitchVoteSavesTargets = ImpostorTab.BoolOption("Voting The Witch Saves All The Targets", true, WitchSpawnRate);

        NinjaSpawnRate = Header(ImpostorTab.SpawnRateOption(Cs(Ninja.color, "Ninja"), ActivateRoles));
        NinjaCooldown = ImpostorTab.CooldownOption("Ninja Mark Cooldown", NinjaSpawnRate);
        NinjaFirstCooldown = ImpostorTab.CooldownOption("Ninja First Mark Cooldown", NinjaSpawnRate);
        NinjaKnowsTargetLocation = ImpostorTab.BoolOption("Ninja Knows Location Of Target", true, NinjaSpawnRate);
        NinjaTraceTime = ImpostorTab.FloatOption("Trace Duration", 1f, 20f, 0.5f, 5f, NinjaSpawnRate, suffix: "s");
        NinjaTraceColorTime = ImpostorTab.FloatOption("Time Till Trace Color Has Faded", 0f, 20f, 0.5f, 2f,
            NinjaSpawnRate, suffix: "s");
        NinjaInvisibleDuration =
            ImpostorTab.FloatOption("Time The Ninja Is Invisible", 0f, 20f, 1f, 3f, NinjaSpawnRate, suffix: "s");

        BomberSpawnRate = Header(ImpostorTab.SpawnRateOption(Cs(Bomber.color, "Bomber"), ActivateRoles));
        BomberBombDestructionTime =
            ImpostorTab.FloatOption("Bomb Destruction Time", 2.5f, 120f, 2.5f, 20f, BomberSpawnRate);
        BomberBombDestructionRange =
            ImpostorTab.FloatOption("Bomb Destruction Range", 5f, 150f, 5f, 50f, BomberSpawnRate, suffix: "x");
        BomberBombHearRange =
            ImpostorTab.FloatOption("Bomb Hear Range", 5f, 150f, 5f, 60f, BomberSpawnRate, suffix: "x");
        BomberDefuseDuration =
            ImpostorTab.FloatOption("Bomb Defuse Duration", 0.5f, 30f, 0.5f, 3f, BomberSpawnRate, suffix: "s");
        BomberBombCooldown =
            ImpostorTab.FloatOption("Bomb Cooldown", 2.5f, 30f, 2.5f, 15f, BomberSpawnRate, suffix: "s");
        BomberBombActiveAfter =
            ImpostorTab.FloatOption("Bomb Is Active After", 0.5f, 15f, 0.5f, 3f, BomberSpawnRate, suffix: "s");

        UndertakerSpawnRate = Header(ImpostorTab.SpawnRateOption(Cs(Undertaker.Color, "Undertaker"), ActivateRoles));
        UndertakerSpeedModifier = ImpostorTab.FloatOption("Speed Modifier While Dragging", -100f, 100f, 5f, 0f, UndertakerSpawnRate, suffix: "%");
        UndertakerDisableVent = ImpostorTab.BoolOption("Disable Vent While Dragging", true, UndertakerSpawnRate);
        
        StickyBomberSpawnRate = Header(ImpostorTab.SpawnRateOption(Cs(StickyBomber.Color, "Sticky Bomber"), ActivateRoles));
        StickyBomberCooldown = ImpostorTab.CooldownOption("Sticky Bomb Cooldown", StickyBomberSpawnRate);
        StickyBomberFirstCooldown = ImpostorTab.CooldownOption("Sticky Bomb First Cooldown", StickyBomberSpawnRate);
        StickyBomberFirstDelay = ImpostorTab.FloatOption("Sticky Bomb First Delay", 0f, 20f, 1f, 5f, StickyBomberSpawnRate, suffix: "s");
        StickyBomberOtherDelay = ImpostorTab.FloatOption("Sticky Bomb Other Delay", 0f, 20f, 1f, 5f, StickyBomberSpawnRate, suffix: "s");
        StickyBomberDuration = ImpostorTab.FloatOption("Sticky Bomb Timer", 5f, 60f, 2.5f, 30f, StickyBomberSpawnRate, suffix: "s");
        StickyBomberShowTimer = ImpostorTab.BoolOption("Display Remaining Time", true, StickyBomberSpawnRate);
        StickyBomberCanReceiveBomb = ImpostorTab.BoolOption("Sticky Bomber Can Receive His Own Bomb", false, StickyBomberSpawnRate);
        StickyBomberCanGiveBombToShielded = ImpostorTab.BoolOption("Shielded Players Can Receive Bomb", false, StickyBomberSpawnRate);
        StickyBomberEnableKillButton = ImpostorTab.BoolOption("Has Kill Button", false, StickyBomberSpawnRate);
        StickyBomberTriggerAllCooldowns = ImpostorTab.BoolOption("Trigger Both Cooldown", false, StickyBomberEnableKillButton);
        
        GuesserSpawnRate = Header(NeutralTab.SpawnRateOption(Cs(Guesser.color, "Guesser"), ActivateRoles));
        GuesserIsImpGuesserRate =
            NeutralTab.SpawnRateOption("Chance That The Guesser Is An Impostor", GuesserSpawnRate);
        GuesserNumberOfShots = NeutralTab.FloatOption("Guesser Number Of Shots", 1f, 15f, 1f, 2f, GuesserSpawnRate);
        GuesserHasMultipleShotsPerMeeting =
            NeutralTab.BoolOption("Guesser Can Shoot Multiple Times Per Meeting", false, GuesserSpawnRate);
        GuesserKillsThroughShield = NeutralTab.BoolOption("Guesses Ignore The Medic Shield", true, GuesserSpawnRate);
        GuesserEvilCanKillSpy = NeutralTab.BoolOption("Evil Guesser Can Guess The Spy", true, GuesserSpawnRate);
        GuesserSpawnBothRate = NeutralTab.SpawnRateOption("Both Guesser Spawn Rate", GuesserSpawnRate);
        GuesserCantGuessSnitchIfTasksDone =
            NeutralTab.BoolOption("Guesser Can't Guess Snitch When Tasks Completed", true, GuesserSpawnRate);

        JesterSpawnRate = Header(NeutralTab.SpawnRateOption(Cs(Jester.color, "Jester"), ActivateRoles));
        JesterCanCallEmergency = NeutralTab.BoolOption("Jester Can Call Emergency Meeting", true, JesterSpawnRate);
        JesterHasImpostorVision = NeutralTab.BoolOption("Jester Has Impostor Vision", false, JesterSpawnRate);

        ArsonistSpawnRate = Header(NeutralTab.SpawnRateOption(Cs(Arsonist.color, "Arsonist"), ActivateRoles));
        ArsonistCooldown = NeutralTab.CooldownOption("Arsonist Cooldown", ArsonistSpawnRate, 2.5f, 12.5f);
        ArsonistFirstCooldown = NeutralTab.CooldownOption("Arsonist First Cooldown", ArsonistSpawnRate, 2.5f, 12.5f);
        ArsonistDuration = NeutralTab.ApplyDurationOption("Douse Duration", ArsonistSpawnRate);

        JackalSpawnRate = Header(NeutralTab.SpawnRateOption(Cs(Jackal.color, "Jackal"), ActivateRoles));
        JackalKillCooldown = NeutralTab.CooldownOption("Jackal/Sidekick Kill Cooldown", JackalSpawnRate);
        JackalCanUseVents = NeutralTab.BoolOption("Jackal Can Use Vents", true, JackalSpawnRate);
        JackalAndSidekickHaveImpostorVision =
            NeutralTab.BoolOption("Jackal And Sidekick Have Impostor Vision", false, JackalSpawnRate);
        JackalCanCreateSidekick = NeutralTab.BoolOption("Jackal Can Create A Sidekick", false, JackalSpawnRate);
        JackalCreateSidekickCooldown =
            NeutralTab.CooldownOption("Jackal Create Sidekick Cooldown", JackalCanCreateSidekick);
        SidekickPromoteToJackal = NeutralTab.BoolOption("Sidekick Gets Promoted To Jackal On Jackal Death", false,
            JackalCanCreateSidekick);
        JackalPromotedFromSidekickCanCreateSidekick =
            NeutralTab.BoolOption("Jackals Promoted From Sidekick Can Create A Sidekick", true,
                SidekickPromoteToJackal);
        SidekickCanKill = NeutralTab.BoolOption("Sidekick Can Kill", false, JackalCanCreateSidekick);
        SidekickCanUseVents = NeutralTab.BoolOption("Sidekick Can Use Vents", true, JackalCanCreateSidekick);
        JackalCanCreateSidekickFromImpostor = NeutralTab.BoolOption("Jackals Can Make An Impostor To His Sidekick",
            true, JackalCanCreateSidekick);

        VultureSpawnRate = Header(NeutralTab.SpawnRateOption(Cs(Vulture.color, "Vulture"), ActivateRoles));
        VultureCooldown = NeutralTab.CooldownOption("Vulture Cooldown", VultureSpawnRate, defaultValue: 15f);
        VultureNumberToWin =
            NeutralTab.FloatOption("Number Of Corpses Needed To Be Eaten", 1f, 10f, 1f, 4f, VultureSpawnRate);
        VultureCanUseVents = NeutralTab.BoolOption("Vulture Can Use Vents", true, VultureSpawnRate);
        VultureShowArrow = NeutralTab.BoolOption("Show Arrows Pointing Towards The Corpses", true, VultureSpawnRate);

        LawyerSpawnRate = Header(NeutralTab.SpawnRateOption(Cs(Lawyer.color, "Lawyer"), ActivateRoles));
        LawyerIsProsecutorChance = NeutralTab.SpawnRateOption("Chance That The Lawyer Is Prosecutor", LawyerSpawnRate);
        LawyerVision = NeutralTab.FloatOption("Vision", 0.25f, 3f, 0.25f, 1f, LawyerSpawnRate, suffix: "x");
        LawyerKnowsRole = NeutralTab.BoolOption("Lawyer/Prosecutor Knows Target Role", false, LawyerSpawnRate);
        LawyerCanCallEmergency =
            NeutralTab.BoolOption("Lawyer/Prosecutor Can Call Emergency Meeting", true, LawyerSpawnRate);
        LawyerTargetCanBeJester = NeutralTab.BoolOption("Lawyer Target Can Be The Jester", true, LawyerSpawnRate);
        PursuerCooldown = NeutralTab.CooldownOption("Pursuer Blank Cooldown", LawyerSpawnRate);
        PursuerBlanksNumber = NeutralTab.FloatOption("Pursuer Number Of Blanks", 1f, 20f, 1f, 5f, LawyerSpawnRate);

        ThiefSpawnRate = Header(NeutralTab.SpawnRateOption(Cs(Thief.color, "Thief"), ActivateRoles));
        ThiefCooldown = NeutralTab.CooldownOption("Thief Cooldown", ThiefSpawnRate);
        ThiefCanKillSheriff = NeutralTab.BoolOption("Thief Can Kill Sheriff", true, ThiefSpawnRate);
        ThiefHasImpVision = NeutralTab.BoolOption("Thief Has Impostor Vision", true, ThiefSpawnRate);
        ThiefCanUseVents = NeutralTab.BoolOption("Thief Can Use Vents", true, ThiefSpawnRate);
        ThiefCanStealWithGuess = NeutralTab.BoolOption("Thief Can Guess To Steal A Role (If Guesser)", false, ThiefSpawnRate);
        ThiefStolenPlayerKeepsHisTeam = NeutralTab.BoolOption("Stolen Player Keeps His Team", false, ThiefSpawnRate);

        MayorSpawnRate = Header(CrewmateTab.SpawnRateOption(Cs(Mayor.color, "Mayor"), ActivateRoles));
        MayorCanSeeVoteColors = CrewmateTab.BoolOption("Mayor Can See Vote Colors", false, MayorSpawnRate);
        MayorTasksNeededToSeeVoteColors = CrewmateTab.FloatOption("Completed Tasks Needed To See Vote Colors", 0f, 20f,
            1f, 5f, MayorCanSeeVoteColors);
        MayorMeetingButton = CrewmateTab.BoolOption("Mobile Emergency Button", true, MayorSpawnRate);
        MayorMaxRemoteMeetings =
            CrewmateTab.FloatOption("Number Of Remote Meetings", 1f, 5f, 1f, 1f, MayorMeetingButton);
        MayorChooseSingleVote = CrewmateTab.StringOption("Mayor Can Choose Single Vote",
            MayorChooseSingleVoteSelections, MayorChooseSingleVoteSelections[0], MayorSpawnRate);

        EngineerSpawnRate = Header(CrewmateTab.SpawnRateOption(Cs(Engineer.color, "Engineer"), ActivateRoles));
        EngineerNumberOfFixes = CrewmateTab.FloatOption("Number Of Sabotage Fixes", 1f, 3f, 1f, 1f, EngineerSpawnRate);
        EngineerHighlightForImpostors =
            CrewmateTab.BoolOption("Impostors See Vents Highlighted", true, EngineerSpawnRate);
        EngineerHighlightForTeamJackal =
            CrewmateTab.BoolOption("Jackal and Sidekick See Vents Highlighted", true, EngineerSpawnRate);

        SheriffSpawnRate = Header(CrewmateTab.SpawnRateOption(Cs(Sheriff.color, "Sheriff"), ActivateRoles));
        SheriffCooldown = CrewmateTab.CooldownOption("Sheriff Cooldown", SheriffSpawnRate);
        SheriffCanKillNeutrals = CrewmateTab.BoolOption("Sheriff Can Kill Neutrals", false, SheriffSpawnRate);
        DeputySpawnRate = CrewmateTab.SpawnRateOption("Sheriff Has A Deputy", SheriffSpawnRate);
        DeputyNumberOfHandcuffs =
            CrewmateTab.FloatOption("Deputy Number Of Handcuffs", 1f, 10f, 1f, 3f, DeputySpawnRate);
        DeputyHandcuffCooldown = CrewmateTab.CooldownOption("Handcuff Cooldown", DeputySpawnRate);
        DeputyHandcuffDuration =
            CrewmateTab.FloatOption("Handcuff Duration", 5f, 60f, 2.5f, 15f, DeputySpawnRate, suffix: "s");
        DeputyKnowsSheriff = CrewmateTab.BoolOption("Sheriff And Deputy Know Each Other", true, DeputySpawnRate);
        DeputyGetsPromoted = CrewmateTab.StringOption("Deputy Gets Promoted To Sheriff",
            DeputyGetsPromotedSelections,
            DeputyGetsPromotedSelections[0], DeputySpawnRate);
        DeputyKeepsHandcuffs = CrewmateTab.BoolOption("Deputy Keeps Handcuffs When Promoted", true, DeputyGetsPromoted);

        LighterSpawnRate = Header(CrewmateTab.SpawnRateOption(Cs(Lighter.color, "Lighter"), ActivateRoles));
        LighterModeLightsOnVision =
            CrewmateTab.FloatOption("Vision On Lights On", 0.25f, 5f, 0.25f, 1.5f, LighterSpawnRate, suffix: "x");
        LighterModeLightsOffVision = CrewmateTab.FloatOption("Vision On Lights Off", 0.25f, 5f, 0.25f, 0.5f,
            LighterSpawnRate, suffix: "x");
        LighterFlashlightWidth =
            CrewmateTab.FloatOption("Flashlight Width", 0.1f, 1f, 0.1f, 0.3f, LighterSpawnRate, suffix: "x");

        DetectiveSpawnRate = Header(CrewmateTab.SpawnRateOption(Cs(Detective.color, "Detective"), ActivateRoles));
        DetectiveAnonymousFootprints = CrewmateTab.BoolOption("Anonymous Footprints", false, DetectiveSpawnRate);
        DetectiveFootprintInterval = CrewmateTab.FloatOption("Footprint Interval", 0.25f, 10f, 0.25f, 0.5f,
            DetectiveSpawnRate, suffix: "s");
        DetectiveFootprintDuration = CrewmateTab.FloatOption("Footprint Duration", 0.25f, 10f, 0.25f, 5f,
            DetectiveSpawnRate, suffix: "s");
        DetectiveReportNameDuration = CrewmateTab.FloatOption("Time Where Detective Reports Will Have Name", 0f, 60,
            2.5f, 0f, DetectiveSpawnRate);
        DetectiveReportColorDuration = CrewmateTab.FloatOption("Time Where Detective Reports Will Have Color Type", 0f,
            120, 2.5f, 20f, DetectiveSpawnRate);

        TimeMasterSpawnRate = Header(CrewmateTab.SpawnRateOption(Cs(TimeMaster.color, "Time Master"), ActivateRoles));
        TimeMasterCooldown = CrewmateTab.CooldownOption("Time Master Cooldown", TimeMasterSpawnRate);
        TimeMasterRewindTime =
            CrewmateTab.FloatOption("Rewind Time", 1f, 10f, 1f, 3f, TimeMasterSpawnRate, suffix: "s");
        TimeMasterShieldDuration = CrewmateTab.FloatOption("Time Master Shield Duration", 1f, 20f, 1f, 3f,
            TimeMasterSpawnRate, suffix: "s");

        MedicSpawnRate = Header(CrewmateTab.SpawnRateOption(Cs(Medic.color, "Medic"), ActivateRoles));
        MedicShowShielded = CrewmateTab.StringOption("Show Shielded Player",
            MedicShowShieldedSelections,
            MedicShowShieldedSelections[0], MedicSpawnRate);
        MedicShowAttemptToMedic =
            CrewmateTab.BoolOption("Medic Sees Murder Attempt On Shielded Player", false, MedicSpawnRate);
        MedicShowAttemptToShielded =
            CrewmateTab.BoolOption("Shielded Player Sees Murder Attempt", false, MedicSpawnRate);
        MedicSetOrShowShieldAfterMeeting = CrewmateTab.StringOption("Shield Will Be Activated",
            MedicSetOrShowShieldAfterMeetingSelections, MedicSetOrShowShieldAfterMeetingSelections[0], MedicSpawnRate);
        
        SwapperSpawnRate = Header(CrewmateTab.SpawnRateOption(Cs(Swapper.color, "Swapper"), ActivateRoles));
        SwapperCanCallEmergency = CrewmateTab.BoolOption("Swapper Can Call Emergency Meeting", false, SwapperSpawnRate);
        SwapperCanOnlySwapOther = CrewmateTab.BoolOption("Swapper Can Only Swap Others", false, SwapperSpawnRate);
        SwapperSwapsNumber = CrewmateTab.FloatOption("Initial Swap Charges", 0f, 5f, 1f, 1f, SwapperSpawnRate);
        SwapperRechargeTasksNumber = CrewmateTab.FloatOption("Number Of Tasks Needed For Recharging", 1f, 10f, 1f, 2f, SwapperSpawnRate);

        SeerSpawnRate = Header(CrewmateTab.SpawnRateOption(Cs(Seer.color, "Seer"), ActivateRoles));
        SeerMode = CrewmateTab.StringOption("Seer Mode", SeerModeSelections, SeerModeSelections[0], SeerSpawnRate);
        SeerLimitSoulDuration = CrewmateTab.BoolOption("Seer Limit Soul Duration", false, SeerSpawnRate);
        SeerSoulDuration = CrewmateTab.FloatOption("Soul Duration", 0f, 120f, 5f, 15f, SeerLimitSoulDuration, suffix: "s");
        
        HackerSpawnRate = Header(CrewmateTab.SpawnRateOption(Cs(Hacker.color, "Hacker"), ActivateRoles));
        HackerCooldown = CrewmateTab.CooldownOption("Hacker Cooldown", HackerSpawnRate);
        HackerHackeringDuration = CrewmateTab.FloatOption("Hacker Duration", 2.5f, 60f, 2.5f, 10f, HackerSpawnRate, suffix: "s");
        HackerOnlyColorType = CrewmateTab.BoolOption("Hacker Only Sees Color Type", false, HackerSpawnRate);
        HackerToolsNumber = CrewmateTab.FloatOption("Max Mobile Gadget Charges", 1f, 30f, 1f, 5f, HackerSpawnRate);
        HackerRechargeTasksNumber = CrewmateTab.FloatOption("Number Of Tasks Needed For Recharging", 1f, 5f, 1f, 2f, HackerSpawnRate);
        HackerNoMove = CrewmateTab.BoolOption("Cant Move During Mobile Gadget Duration", true, HackerSpawnRate);

        TrackerSpawnRate = Header(CrewmateTab.SpawnRateOption(Cs(Tracker.color, "Tracker"), ActivateRoles));
        TrackerUpdateInterval = CrewmateTab.FloatOption("Tracker Update Interval", 1f, 30f, 1f, 5f, TrackerSpawnRate, suffix: "s");
        TrackerResetTargetAfterMeeting = CrewmateTab.BoolOption("Tracker Reset Target After Meeting", false, TrackerSpawnRate);
        TrackerCanTrackCorpses = CrewmateTab.BoolOption("Tracker Can Track Corpses", true, TrackerSpawnRate);
        TrackerCorpsesTrackingCooldown = CrewmateTab.CooldownOption("Corpses Tracking Cooldown", TrackerCanTrackCorpses);
        TrackerCorpsesTrackingDuration = CrewmateTab.FloatOption("Corpses Tracking Duration", 2.5f, 30f, 2.5f, 5f, TrackerCanTrackCorpses, suffix: "s");

        SnitchSpawnRate = Header(CrewmateTab.SpawnRateOption(Cs(Snitch.color, "Snitch"), ActivateRoles));
        SnitchLeftTasksForReveal = CrewmateTab.FloatOption("Task Count Where The Snitch Will Be Revealed", 0f, 25f, 1f, 5f, SnitchSpawnRate);
        SnitchMode = CrewmateTab.StringOption("Information Mode", SnitchModeSelections, SnitchModeSelections[0], SnitchSpawnRate);
        SnitchTargets = CrewmateTab.StringOption("Targets", SnitchTargetSelections, SnitchTargetSelections[0], SnitchSpawnRate);
        
        SpySpawnRate = Header(CrewmateTab.SpawnRateOption(Cs(Spy.color, "Spy"), ActivateRoles));
        SpyCanDieToSheriff = CrewmateTab.BoolOption("Spy Can Die To Sheriff", false, SpySpawnRate);
        SpyImpostorsCanKillAnyone = CrewmateTab.BoolOption("Impostors Can Kill Anyone If There Is A Spy", true, SpySpawnRate);
        SpyCanEnterVents = CrewmateTab.BoolOption("Spy Can Enter Vents", false, SpySpawnRate);
        SpyHasImpostorVision = CrewmateTab.BoolOption("Spy Has Impostor Vision", false, SpySpawnRate);
        
        PortalmakerSpawnRate = Header(CrewmateTab.SpawnRateOption(Cs(Portalmaker.color, "Portal Maker"), ActivateRoles));
        PortalmakerCooldown = CrewmateTab.CooldownOption("Portalmaker Cooldown", PortalmakerSpawnRate);
        PortalmakerUsePortalCooldown = CrewmateTab.CooldownOption("Use Portalmaker Cooldown", PortalmakerSpawnRate);
        PortalmakerLogOnlyColorType = CrewmateTab.BoolOption("Portalmaker Log Only Shows Color Type", true, PortalmakerSpawnRate);
        PortalmakerLogHasTime = CrewmateTab.BoolOption("Log Shows Time", true, PortalmakerSpawnRate);
        PortalmakerCanPortalFromAnywhere = CrewmateTab.BoolOption("Can Port To Portal From Everywhere", true, PortalmakerSpawnRate);

        SecurityGuardSpawnRate = Header(CrewmateTab.SpawnRateOption(Cs(SecurityGuard.color, "Security Guard"), ActivateRoles));
        SecurityGuardCooldown = CrewmateTab.CooldownOption("Security Guard Cooldown", SecurityGuardSpawnRate);
        SecurityGuardTotalScrews = CrewmateTab.FloatOption("Security Guard Number Of Screws", 1f, 15f, 1f, 7f, SecurityGuardSpawnRate);
        SecurityGuardCamPrice = CrewmateTab.FloatOption("Number Of Screws Per Cam", 1f, 15f, 1f, 2f, SecurityGuardSpawnRate);
        SecurityGuardVentPrice = CrewmateTab.FloatOption("Number Of Screws Per Vent", 1f, 15f, 1f, 1f, SecurityGuardSpawnRate);
        SecurityGuardCamDuration = CrewmateTab.FloatOption("Security Guard Duration", 2.5f, 60f, 2.5f, 10f, SecurityGuardSpawnRate, suffix: "s");
        SecurityGuardCamMaxCharges = CrewmateTab.FloatOption("Gadget Max Charges", 1f, 30f, 1f, 5f, SecurityGuardSpawnRate);
        SecurityGuardCamRechargeTasksNumber = CrewmateTab.FloatOption("Number Of Tasks Needed For Recharging", 1f, 10f, 1f, 3f, SecurityGuardSpawnRate);
        SecurityGuardNoMove = CrewmateTab.BoolOption("Cant Move During Cam Duration", true, SecurityGuardSpawnRate);
        
        MediumSpawnRate = Header(CrewmateTab.SpawnRateOption(Cs(Medium.color, "Medium"), ActivateRoles));
        MediumCooldown = CrewmateTab.CooldownOption("Medium Questioning Cooldown", MediumSpawnRate);
        MediumDuration = CrewmateTab.ApplyDurationOption("Medium Questioning Duration", MediumSpawnRate);
        MediumOneTimeUse = CrewmateTab.BoolOption("Each Soul Can Only Be Questioned Once", false, MediumSpawnRate);
        MediumChanceAdditionalInfo = CrewmateTab.SpawnRateOption("Chance To Have Additional Information", MediumSpawnRate);

        TrapperSpawnRate = Header(CrewmateTab.SpawnRateOption(Cs(Trapper.color, "Trapper"), ActivateRoles));
        TrapperCooldown = CrewmateTab.CooldownOption("Trapper Cooldown", TrapperSpawnRate);
        TrapperMaxCharges = CrewmateTab.FloatOption("Max Traps Charges", 1f, 15f, 1f, 5f, TrapperSpawnRate);
        TrapperRechargeTasksNumber = CrewmateTab.FloatOption("Number Of Tasks Needed For Recharging", 1f, 15f, 1f, 2f, TrapperSpawnRate);
        TrapperTrapNeededTriggerToReveal = CrewmateTab.FloatOption("Trap Needed Trigger To Reveal", 2f, 10f, 1f, 3f, TrapperSpawnRate);
        TrapperAnonymousMap = CrewmateTab.BoolOption("Show Anonymous Map", false, TrapperSpawnRate);
        TrapperInfoType = CrewmateTab.StringOption("Trap Information Type", TrapperInfoTypeSelections, TrapperInfoTypeSelections[0], TrapperSpawnRate);
        TrapperTrapDuration = CrewmateTab.FloatOption("Time before trap is active", 1f, 15f, 1f, 5f, TrapperSpawnRate, suffix: "s");

        var modifierColor = Color.yellow;
        ModifiersAreHidden = Header(ModifierTab.BoolOption(Cs(modifierColor, "Hide Death Related Modifiers"), true, ActivateRoles));

        ModifierBloody = Header(ModifierTab.SpawnRateOption(Cs(modifierColor, "Bloody"), ActivateRoles));
        ModifierBloodyQuantity = ModifierTab.QuantityOption("Bloody Quantity", ModifierBloody);
        ModifierBloodyDuration = ModifierTab.FloatOption("Trail Duration", 3f, 60f, 1f, 10f, ModifierBloody, suffix: "s");
        
        ModifierAntiTeleport = Header(ModifierTab.SpawnRateOption(Cs(modifierColor, "Anti Teleport"), ActivateRoles));
        ModifierAntiTeleportQuantity = ModifierTab.QuantityOption("Anti Teleport Quantity", ModifierAntiTeleport);
        
        ModifierTieBreaker = Header(ModifierTab.SpawnRateOption(Cs(modifierColor, "Tie Breaker"), ActivateRoles));
        
        ModifierBait = Header(ModifierTab.SpawnRateOption(Cs(modifierColor, "Bait"), ActivateRoles));
        ModifierBaitQuantity = ModifierTab.QuantityOption("Bait Quantity", ModifierBait);
        ModifierBaitReportDelayMin = ModifierTab.FloatOption("Bait Report Delay Min", 0f, 10f, 1f, 0f, ModifierBait, suffix: "s");
        ModifierBaitReportDelayMax = ModifierTab.FloatOption("Bait Report Delay Max", 0f, 10f, 1f, 0f, ModifierBait, suffix: "s");
        ModifierBaitShowKillFlash = ModifierTab.BoolOption("Warn The Killer With A Flash", true, ModifierBait);
        
        ModifierLover = Header(ModifierTab.SpawnRateOption(Cs(modifierColor, "Lovers"), ActivateRoles));
        ModifierLoverImpLoverRate = ModifierTab.SpawnRateOption("Chance That One Lover Is Impostor", ModifierLover);
        ModifierLoverBothDie = ModifierTab.BoolOption("Both Lovers Die", false, ModifierLover);
        ModifierLoverEnableChat = ModifierTab.BoolOption("Enable Lover Chat", true, ModifierLover);
        
        ModifierSunglasses = Header(ModifierTab.SpawnRateOption(Cs(modifierColor, "Sunglasses"), ActivateRoles));
        ModifierSunglassesQuantity = ModifierTab.QuantityOption("Sunglasses Quantity", ModifierSunglasses);
        ModifierSunglassesVision = ModifierTab.FloatOption("Vision With Sunglasses", 10f, 50f, 10f, 30f, ModifierSunglasses, "-", "%");
        
        ModifierMini = Header(ModifierTab.SpawnRateOption(Cs(modifierColor, "Mini"), ActivateRoles));
        ModifierMiniGrowingUpDuration = ModifierTab.FloatOption("Mini Growing Up Duration", 100f, 1500f, 100f, 400f, ModifierMini, suffix: "s");
        ModifierMiniGrowingUpInMeeting = ModifierTab.BoolOption("Mini Grows Up In Meeting", true, ModifierMini);
        
        ModifierVip = Header(ModifierTab.SpawnRateOption(Cs(modifierColor, "Vip"), ActivateRoles));
        ModifierVipQuantity = ModifierTab.QuantityOption("Vip Quantity", ModifierVip);
        ModifierVipShowColor = ModifierTab.BoolOption("Show Team Color", true, ModifierVip);
        
        ModifierInvert = Header(ModifierTab.SpawnRateOption(Cs(modifierColor, "Invert"), ActivateRoles));
        ModifierInvertQuantity = ModifierTab.QuantityOption("Invert Quantity", ModifierInvert);
        ModifierInvertDuration = ModifierTab.FloatOption("Number Of Meetings Inverted", 1f, 15f, 1f, 3f, ModifierInvert);
        
        ModifierChameleon = Header(ModifierTab.SpawnRateOption(Cs(modifierColor, "Chameleon"), ActivateRoles));
        ModifierChameleonQuantity = ModifierTab.QuantityOption("Chameleon Quantity", ModifierChameleon);
        ModifierChameleonHoldDuration = ModifierTab.FloatOption("Time Until Fading Starts", 1f, 10f, 0.5f, 3f, ModifierChameleon, suffix: "s");
        ModifierChameleonFadeDuration = ModifierTab.FloatOption("Fade Duration", 0.25f, 10f, 0.25f, 1f, ModifierChameleon, suffix: "s");
        ModifierChameleonMinVisibility = ModifierTab.FloatOption("Minimum Visibility", 0f, 50f, 10f, 0f, ModifierChameleon, suffix: "%");

        ModifierShifter = Header(ModifierTab.SpawnRateOption(Cs(modifierColor, "Shifter"), ActivateRoles));

        GuesserModeCrewNumber = Header(GuesserTab.FloatOption("Number of Crew Guessers", 0f, 15f, 1f, 1f, GuesserMode));
        
        GuesserModeNeutralNumber = Header(GuesserTab.FloatOption("Number of Neutral Guessers", 0f, 15f, 1f, 1f, GuesserMode));
        GuesserModeForceJackalGuesser = GuesserTab.BoolOption("Force Jackal Guesser", false, GuesserModeNeutralNumber);
        GuesserModeForceThiefGuesser = GuesserTab.BoolOption("Force Thief Guesser", false, GuesserModeNeutralNumber);
        
        GuesserModeImpNumber = Header(GuesserTab.FloatOption("Number of Impostor Guessers", 0f, 15f, 1f, 1f, GuesserMode));

        GuesserModeHaveModifier = Header(GuesserTab.BoolOption("Guessers Can Have A Modifier", true, GuesserMode));

        GuesserModeNumberOfShots = Header(GuesserTab.FloatOption("Guesser Number Of Shots", 1f, 15f, 1f, 3f, GuesserMode));

        GuesserModeMultipleShotsPerMeeting = Header(GuesserTab.BoolOption("Guesser Can Shoot Multiple Times Per Meeting", false, GuesserMode));
        
        GuesserModeKillsThroughShield = Header(GuesserTab.BoolOption("Guesses Ignore The Medic Shield", true, GuesserMode));
        
        GuesserModeEvilCanKillSpy = Header(GuesserTab.BoolOption("Evil Guesser Can Guess The Spy", true, GuesserMode));
        
        GuesserModeCantGuessSnitchIfTasksDone = Header(GuesserTab.BoolOption("Guesser Can't Guess Snitch When Tasks Completed", true, GuesserMode));
    }
}