using System.Collections.Generic;

namespace BetterOtherRoles.Options;

public static partial class CustomOptionHolder
{
    public const string MayorChooseSingleVote_Off = "Off";
    public const string MayorChooseSingleVote_BeforeVoting = "On (Before Voting)";
    public const string MayorChooseSingleVote_UntilMeetingEnds = "On (Until Meeting Ends)";
    public static readonly List<string> MayorChooseSingleVoteSelections = [MayorChooseSingleVote_Off, MayorChooseSingleVote_BeforeVoting, MayorChooseSingleVote_UntilMeetingEnds];
    
    public const string DeputyGetsPromoted_Off = "Off";
    public const string DeputyGetsPromoted_Immediately = "On (Immediately)";
    public const string DeputyGetsPromoted_AfterMeeting = "On (After Meeting)";
    public static readonly List<string> DeputyGetsPromotedSelections = [DeputyGetsPromoted_Off, DeputyGetsPromoted_Immediately, DeputyGetsPromoted_AfterMeeting];
    
    public const string MedicShowShielded_Everyone = "Everyone";
    public const string MedicShowShielded_ShieldedAndMedic = "Shielded + Medic";
    public const string MedicShowShielded_Medic = "Medic";
    public static readonly List<string> MedicShowShieldedSelections = [MedicShowShielded_Everyone, MedicShowShielded_ShieldedAndMedic, MedicShowShielded_Medic];
    
    public const string MedicSetOrShowShieldAfterMeeting_Instantly = "Instantly";
    public const string MedicSetOrShowShieldAfterMeeting_InstantlyVisibleAfterMeeting = "Instantly, Visible \nAfter Meeting";
    public const string MedicSetOrShowShieldAfterMeeting_AfterMeeting = "After Meeting";
    public static readonly List<string> MedicSetOrShowShieldAfterMeetingSelections = [MedicSetOrShowShieldAfterMeeting_Instantly, MedicSetOrShowShieldAfterMeeting_InstantlyVisibleAfterMeeting, MedicSetOrShowShieldAfterMeeting_AfterMeeting];

    public const string SeerMode_DeathFlashAndSouls = "Show Death Flash + Souls";
    public const string SeerMode_DeathFlash = "Show Death Flash";
    public const string SeerMode_Souls = "Show Souls";
    public static readonly List<string> SeerModeSelections = [SeerMode_DeathFlashAndSouls, SeerMode_DeathFlash, SeerMode_Souls];

    public const string SnitchMode_Chat = "Chat";
    public const string SnitchMode_Map = "Map";
    public const string SnitchMode_ChatAndMap = "Chat & Map";
    public static readonly List<string> SnitchModeSelections = [SnitchMode_Chat, SnitchMode_Map, SnitchMode_ChatAndMap];
    
    public const string SnitchTarget_Evil = "All Evil Players";
    public const string SnitchTarget_Killer = "Killing Players";
    public static readonly List<string> SnitchTargetSelections = [SnitchTarget_Evil, SnitchTarget_Killer];

    public const string TrapperInfoType_Role = "Role";
    public const string TrapperInfoType_GoodEvil = "Good/Evil Role";
    public const string TrapperInfoType_Name = "Name";
    public static readonly List<string> TrapperInfoTypeSelections = [TrapperInfoType_Role, TrapperInfoType_GoodEvil, TrapperInfoType_Name];
}