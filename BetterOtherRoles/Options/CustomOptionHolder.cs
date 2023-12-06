using AmongUsSpecimen.ModOptions;

namespace BetterOtherRoles.Options;

public static partial class CustomOptionHolder
{
    public static readonly ModOptionTab GuesserTab;
    public static readonly ModOptionTab CrewmateTab;
    public static readonly ModOptionTab NeutralTab;
    public static readonly ModOptionTab ImpostorTab;
    public static readonly ModOptionTab ModifierTab;
    
    
    public static readonly ModBoolOption RandomMap;
    public static readonly RandomMapModOptionMap TheSkeldMap;
    public static readonly RandomMapModOptionMap PolusMap;
    public static readonly RandomMapModOptionMap MiraHqMap;
    public static readonly RandomMapModOptionMap AirshipMap;
    public static readonly RandomMapModOptionMap TheFungleMap;

    public static readonly ModBoolOption EnableBetterPolus;
    public static readonly ModFloatOption PolusReactorCountdown;

    public static readonly ModBoolOption EnableBetterSkeld;
    public static readonly ModBoolOption BetterSkeldEnableAdmin;
    public static readonly ModBoolOption BetterSkeldEnableVitals;

    public static readonly ModBoolOption EnableCodenameHorseMode;
    public static readonly ModBoolOption EnableCodenameDisableHorses;

    public static readonly ModBoolOption ActivateRoles;
    public static readonly ModFloatOption MinCrewmateRoles;
    public static readonly ModFloatOption MaxCrewmateRoles;
    public static readonly ModBoolOption FillCrewmateRoles;
    public static readonly ModFloatOption MinNeutralRoles;
    public static readonly ModFloatOption MaxNeutralRoles;
    public static readonly ModFloatOption MinImpostorRoles;
    public static readonly ModFloatOption MaxImpostorRoles;
    public static readonly ModFloatOption MinModifiers;
    public static readonly ModFloatOption MaxModifiers;
    
    public static readonly ModBoolOption GuesserMode;
    
    public static readonly ModFloatOption MaxNumberOfMeetings;
    public static readonly ModBoolOption RandomizePlayersInMeeting;
    public static readonly ModBoolOption BlockSkippingInEmergencyMeetings;
    public static readonly ModBoolOption NoVoteIsSelfVote;
    
    public static readonly ModBoolOption RandomizeWireTaskPositions;
    
    public static readonly ModBoolOption RandomizeUploadTaskPosition;
    
    public static readonly ModBoolOption HidePlayerNames;
    
    public static readonly ModBoolOption AllowParallelMedBayScans;
    public static readonly ModBoolOption RandomizePositionDuringScan;
    
    public static readonly ModBoolOption ShieldFirstKill;
    public static readonly ModBoolOption ExpireFirstKillShield;
    public static readonly ModFloatOption FirstKillShieldDuration;
    
    public static readonly ModBoolOption FinishTasksBeforeHauntingOrZoomingOut;
    
    public static readonly ModBoolOption CamsNightVision;
    public static readonly ModBoolOption CamsNoNightVisionIfImpVision;

    // IMPOSTOR ROLES OPTIONS

    public static readonly ModFloatOption MafiaSpawnRate;
    public static readonly ModFloatOption JanitorCooldown;

    public static readonly ModFloatOption MorphlingSpawnRate;
    public static readonly ModFloatOption MorphlingCooldown;
    public static readonly ModFloatOption MorphlingDuration;

    public static readonly ModFloatOption CamouflagerSpawnRate;
    public static readonly ModFloatOption CamouflagerCooldown;
    public static readonly ModFloatOption CamouflagerDuration;

    public static readonly ModFloatOption VampireSpawnRate;
    public static readonly ModFloatOption VampireKillDelay;
    public static readonly ModFloatOption VampireCooldown;
    public static readonly ModFloatOption VampireFirstCooldown;
    public static readonly ModBoolOption VampireCanKillNearGarlics;

    public static readonly ModFloatOption EraserSpawnRate;
    public static readonly ModFloatOption EraserCooldown;
    public static readonly ModBoolOption EraserCanEraseAnyone;

    public static readonly ModFloatOption TricksterSpawnRate;
    public static readonly ModFloatOption TricksterPlaceBoxCooldown;
    public static readonly ModFloatOption TricksterLightsOutCooldown;
    public static readonly ModFloatOption TricksterLightsOutDuration;

    public static readonly ModFloatOption CleanerSpawnRate;
    public static readonly ModFloatOption CleanerCooldown;
    
    public static readonly ModFloatOption WarlockSpawnRate;
    public static readonly ModFloatOption WarlockCooldown;
    public static readonly ModFloatOption WarlockFirstCooldown;
    public static readonly ModFloatOption WarlockRootTime;

    public static readonly ModFloatOption BountyHunterSpawnRate;
    public static readonly ModFloatOption BountyHunterBountyDuration;
    public static readonly ModFloatOption BountyHunterReducedCooldown;
    public static readonly ModFloatOption BountyHunterPunishmentTime;
    public static readonly ModBoolOption BountyHunterShowArrow;
    public static readonly ModFloatOption BountyHunterArrowUpdateInterval;

    public static readonly ModFloatOption WitchSpawnRate;
    public static readonly ModFloatOption WitchCooldown;
    public static readonly ModFloatOption WitchFirstCooldown;
    public static readonly ModFloatOption WitchAdditionalCooldown;
    public static readonly ModBoolOption WitchCanSpellAnyone;
    public static readonly ModFloatOption WitchSpellCastingDuration;
    public static readonly ModBoolOption WitchTriggerBothCooldown;
    public static readonly ModBoolOption WitchVoteSavesTargets;

    public static readonly ModFloatOption NinjaSpawnRate;
    public static readonly ModFloatOption NinjaCooldown;
    public static readonly ModFloatOption NinjaFirstCooldown;
    public static readonly ModBoolOption NinjaKnowsTargetLocation;
    public static readonly ModFloatOption NinjaTraceTime;
    public static readonly ModFloatOption NinjaTraceColorTime;
    public static readonly ModFloatOption NinjaInvisibleDuration;

    public static readonly ModFloatOption BomberSpawnRate;
    public static readonly ModFloatOption BomberBombDestructionTime;
    public static readonly ModFloatOption BomberBombDestructionRange;
    public static readonly ModFloatOption BomberBombHearRange;
    public static readonly ModFloatOption BomberDefuseDuration;
    public static readonly ModFloatOption BomberBombCooldown;
    public static readonly ModFloatOption BomberBombActiveAfter;
    
    public static readonly ModFloatOption UndertakerSpawnRate;
    public static readonly ModFloatOption UndertakerSpeedModifier;
    public static readonly ModBoolOption UndertakerDisableVent;
    
    public static readonly ModFloatOption StickyBomberSpawnRate;
    public static readonly ModFloatOption StickyBomberCooldown;
    public static readonly ModFloatOption StickyBomberFirstCooldown;
    public static readonly ModFloatOption StickyBomberFirstDelay;
    public static readonly ModFloatOption StickyBomberOtherDelay;
    public static readonly ModFloatOption StickyBomberDuration;
    public static readonly ModBoolOption StickyBomberShowTimer;
    public static readonly ModBoolOption StickyBomberCanReceiveBomb;
    public static readonly ModBoolOption StickyBomberCanGiveBombToShielded;
    public static readonly ModBoolOption StickyBomberEnableKillButton;
    public static readonly ModBoolOption StickyBomberTriggerAllCooldowns;

    // NEUTRAL ROLES OPTIONS

    public static readonly ModFloatOption GuesserSpawnRate;
    public static readonly ModFloatOption GuesserIsImpGuesserRate;
    public static readonly ModFloatOption GuesserNumberOfShots;
    public static readonly ModBoolOption GuesserHasMultipleShotsPerMeeting;
    public static readonly ModBoolOption GuesserKillsThroughShield;
    public static readonly ModBoolOption GuesserEvilCanKillSpy;
    public static readonly ModFloatOption GuesserSpawnBothRate;
    public static readonly ModBoolOption GuesserCantGuessSnitchIfTasksDone;

    public static readonly ModFloatOption JesterSpawnRate;
    public static readonly ModBoolOption JesterCanCallEmergency;
    public static readonly ModBoolOption JesterHasImpostorVision;

    public static readonly ModFloatOption ArsonistSpawnRate;
    public static readonly ModFloatOption ArsonistCooldown;
    public static readonly ModFloatOption ArsonistFirstCooldown;
    public static readonly ModFloatOption ArsonistDuration;

    public static readonly ModFloatOption JackalSpawnRate;
    public static readonly ModFloatOption JackalKillCooldown;
    public static readonly ModBoolOption JackalCanUseVents;
    public static readonly ModBoolOption JackalCanCreateSidekick;
    public static readonly ModFloatOption JackalCreateSidekickCooldown;
    public static readonly ModBoolOption SidekickPromoteToJackal;
    public static readonly ModBoolOption SidekickCanKill;
    public static readonly ModBoolOption SidekickCanUseVents;
    public static readonly ModBoolOption JackalPromotedFromSidekickCanCreateSidekick;
    public static readonly ModBoolOption JackalCanCreateSidekickFromImpostor;
    public static readonly ModBoolOption JackalAndSidekickHaveImpostorVision;

    public static readonly ModFloatOption VultureSpawnRate;
    public static readonly ModFloatOption VultureCooldown;
    public static readonly ModFloatOption VultureNumberToWin;
    public static readonly ModBoolOption VultureCanUseVents;
    public static readonly ModBoolOption VultureShowArrow;

    public static readonly ModFloatOption LawyerSpawnRate;
    public static readonly ModFloatOption LawyerIsProsecutorChance;
    public static readonly ModFloatOption LawyerVision;
    public static readonly ModBoolOption LawyerKnowsRole;
    public static readonly ModBoolOption LawyerCanCallEmergency;
    public static readonly ModBoolOption LawyerTargetCanBeJester;
    public static readonly ModFloatOption PursuerCooldown;
    public static readonly ModFloatOption PursuerBlanksNumber;
    
    public static readonly ModFloatOption ThiefSpawnRate;
    public static readonly ModFloatOption ThiefCooldown;
    public static readonly ModBoolOption ThiefCanKillSheriff;
    public static readonly ModBoolOption ThiefHasImpVision;
    public static readonly ModBoolOption ThiefCanUseVents;
    public static readonly ModBoolOption ThiefCanStealWithGuess;
    public static readonly ModBoolOption ThiefStolenPlayerKeepsHisTeam;

    // CREWMATE ROLES OPTIONS

    public static readonly ModFloatOption MayorSpawnRate;
    public static readonly ModBoolOption MayorCanSeeVoteColors;
    public static readonly ModFloatOption MayorTasksNeededToSeeVoteColors;
    public static readonly ModBoolOption MayorMeetingButton;
    public static readonly ModFloatOption MayorMaxRemoteMeetings;
    public static readonly ModStringOption MayorChooseSingleVote;

    public static readonly ModFloatOption EngineerSpawnRate;
    public static readonly ModFloatOption EngineerNumberOfFixes;
    public static readonly ModBoolOption EngineerHighlightForImpostors;
    public static readonly ModBoolOption EngineerHighlightForTeamJackal;

    public static readonly ModFloatOption SheriffSpawnRate;
    public static readonly ModFloatOption SheriffCooldown;
    public static readonly ModBoolOption SheriffCanKillNeutrals;
    public static readonly ModFloatOption DeputySpawnRate;
    public static readonly ModFloatOption DeputyNumberOfHandcuffs;
    public static readonly ModFloatOption DeputyHandcuffCooldown;
    public static readonly ModFloatOption DeputyHandcuffDuration;
    public static readonly ModBoolOption DeputyKnowsSheriff;
    public static readonly ModStringOption DeputyGetsPromoted;
    public static readonly ModBoolOption DeputyKeepsHandcuffs;

    public static readonly ModFloatOption LighterSpawnRate;
    public static readonly ModFloatOption LighterModeLightsOnVision;
    public static readonly ModFloatOption LighterModeLightsOffVision;
    public static readonly ModFloatOption LighterFlashlightWidth;

    public static readonly ModFloatOption DetectiveSpawnRate;
    public static readonly ModBoolOption DetectiveAnonymousFootprints;
    public static readonly ModFloatOption DetectiveFootprintInterval;
    public static readonly ModFloatOption DetectiveFootprintDuration;
    public static readonly ModFloatOption DetectiveReportNameDuration;
    public static readonly ModFloatOption DetectiveReportColorDuration;

    public static readonly ModFloatOption TimeMasterSpawnRate;
    public static readonly ModFloatOption TimeMasterCooldown;
    public static readonly ModFloatOption TimeMasterRewindTime;
    public static readonly ModFloatOption TimeMasterShieldDuration;

    public static readonly ModFloatOption MedicSpawnRate;
    public static readonly ModStringOption MedicShowShielded;
    public static readonly ModBoolOption MedicShowAttemptToMedic;
    public static readonly ModBoolOption MedicShowAttemptToShielded;
    public static readonly ModStringOption MedicSetOrShowShieldAfterMeeting;
    
    public static readonly ModFloatOption SwapperSpawnRate;
    public static readonly ModBoolOption SwapperCanCallEmergency;
    public static readonly ModBoolOption SwapperCanOnlySwapOther;
    public static readonly ModFloatOption SwapperSwapsNumber;
    public static readonly ModFloatOption SwapperRechargeTasksNumber;
    
    public static readonly ModFloatOption SeerSpawnRate;
    public static readonly ModStringOption SeerMode;
    public static readonly ModBoolOption SeerLimitSoulDuration;
    public static readonly ModFloatOption SeerSoulDuration;
    
    public static readonly ModFloatOption HackerSpawnRate;
    public static readonly ModFloatOption HackerCooldown;
    public static readonly ModFloatOption HackerHackeringDuration;
    public static readonly ModBoolOption HackerOnlyColorType;
    public static readonly ModFloatOption HackerToolsNumber;
    public static readonly ModFloatOption HackerRechargeTasksNumber;
    public static readonly ModBoolOption HackerNoMove;
    
    public static readonly ModFloatOption TrackerSpawnRate;
    public static readonly ModFloatOption TrackerUpdateInterval;
    public static readonly ModBoolOption TrackerResetTargetAfterMeeting;
    public static readonly ModBoolOption TrackerCanTrackCorpses;
    public static readonly ModFloatOption TrackerCorpsesTrackingCooldown;
    public static readonly ModFloatOption TrackerCorpsesTrackingDuration;
    
    public static readonly ModFloatOption SnitchSpawnRate;
    public static readonly ModFloatOption SnitchLeftTasksForReveal;
    public static readonly ModStringOption SnitchMode;
    public static readonly ModStringOption SnitchTargets;
    
    public static readonly ModFloatOption SpySpawnRate;
    public static readonly ModBoolOption SpyCanDieToSheriff;
    public static readonly ModBoolOption SpyImpostorsCanKillAnyone;
    public static readonly ModBoolOption SpyCanEnterVents;
    public static readonly ModBoolOption SpyHasImpostorVision;
    
    public static readonly ModFloatOption PortalmakerSpawnRate;
    public static readonly ModFloatOption PortalmakerCooldown;
    public static readonly ModFloatOption PortalmakerUsePortalCooldown;
    public static readonly ModBoolOption PortalmakerLogOnlyColorType;
    public static readonly ModBoolOption PortalmakerLogHasTime;
    public static readonly ModBoolOption PortalmakerCanPortalFromAnywhere;
    
    public static readonly ModFloatOption SecurityGuardSpawnRate;
    public static readonly ModFloatOption SecurityGuardCooldown;
    public static readonly ModFloatOption SecurityGuardTotalScrews;
    public static readonly ModFloatOption SecurityGuardCamPrice;
    public static readonly ModFloatOption SecurityGuardVentPrice;
    public static readonly ModFloatOption SecurityGuardCamDuration;
    public static readonly ModFloatOption SecurityGuardCamMaxCharges;
    public static readonly ModFloatOption SecurityGuardCamRechargeTasksNumber;
    public static readonly ModBoolOption SecurityGuardNoMove;
    
    public static readonly ModFloatOption MediumSpawnRate;
    public static readonly ModFloatOption MediumCooldown;
    public static readonly ModFloatOption MediumDuration;
    public static readonly ModBoolOption MediumOneTimeUse;
    public static readonly ModFloatOption MediumChanceAdditionalInfo;
    
    public static readonly ModFloatOption TrapperSpawnRate;
    public static readonly ModFloatOption TrapperCooldown;
    public static readonly ModFloatOption TrapperMaxCharges;
    public static readonly ModFloatOption TrapperRechargeTasksNumber;
    public static readonly ModFloatOption TrapperTrapNeededTriggerToReveal;
    public static readonly ModBoolOption TrapperAnonymousMap;
    public static readonly ModStringOption TrapperInfoType;
    public static readonly ModFloatOption TrapperTrapDuration;
    
    // MODIFIERS OPTIONS

    public static readonly ModBoolOption ModifiersAreHidden;
    
    public static readonly ModFloatOption ModifierBloody;
    public static readonly ModFloatOption ModifierBloodyQuantity;
    public static readonly ModFloatOption ModifierBloodyDuration;
    
    public static readonly ModFloatOption ModifierAntiTeleport;
    public static readonly ModFloatOption ModifierAntiTeleportQuantity;
    
    public static readonly ModFloatOption ModifierTieBreaker;
    
    public static readonly ModFloatOption ModifierBait;
    public static readonly ModFloatOption ModifierBaitQuantity;
    public static readonly ModFloatOption ModifierBaitReportDelayMin;
    public static readonly ModFloatOption ModifierBaitReportDelayMax;
    public static readonly ModBoolOption ModifierBaitShowKillFlash;
    
    public static readonly ModFloatOption ModifierLover;
    public static readonly ModFloatOption ModifierLoverImpLoverRate;
    public static readonly ModBoolOption ModifierLoverBothDie;
    public static readonly ModBoolOption ModifierLoverEnableChat;
    
    public static readonly ModFloatOption ModifierSunglasses;
    public static readonly ModFloatOption ModifierSunglassesQuantity;
    public static readonly ModFloatOption ModifierSunglassesVision;
    
    public static readonly ModFloatOption ModifierMini;
    public static readonly ModFloatOption ModifierMiniGrowingUpDuration;
    public static readonly ModBoolOption ModifierMiniGrowingUpInMeeting;
    
    public static readonly ModFloatOption ModifierVip;
    public static readonly ModFloatOption ModifierVipQuantity;
    public static readonly ModBoolOption ModifierVipShowColor;
    
    public static readonly ModFloatOption ModifierInvert;
    public static readonly ModFloatOption ModifierInvertQuantity;
    public static readonly ModFloatOption ModifierInvertDuration;
    
    public static readonly ModFloatOption ModifierChameleon;
    public static readonly ModFloatOption ModifierChameleonQuantity;
    public static readonly ModFloatOption ModifierChameleonHoldDuration;
    public static readonly ModFloatOption ModifierChameleonFadeDuration;
    public static readonly ModFloatOption ModifierChameleonMinVisibility;
    
    public static readonly ModFloatOption ModifierShifter;
    
    // GUESSER MODE OPTIONS

    public static readonly ModFloatOption GuesserModeCrewNumber;
    public static readonly ModFloatOption GuesserModeNeutralNumber;
    public static readonly ModFloatOption GuesserModeImpNumber;
    public static readonly ModBoolOption GuesserModeForceJackalGuesser;
    public static readonly ModBoolOption GuesserModeForceThiefGuesser;
    public static readonly ModBoolOption GuesserModeHaveModifier;
    public static readonly ModFloatOption GuesserModeNumberOfShots;
    public static readonly ModBoolOption GuesserModeMultipleShotsPerMeeting;
    public static readonly ModBoolOption GuesserModeKillsThroughShield;
    public static readonly ModBoolOption GuesserModeEvilCanKillSpy;
    public static readonly ModBoolOption GuesserModeCantGuessSnitchIfTasksDone;
}