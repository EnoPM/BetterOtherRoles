using System;
using System.Linq;
using BetterOtherRoles.Options;
using BetterOtherRoles.Players;
using BetterOtherRoles.Utilities.Attributes;

namespace BetterOtherRoles.Modules;

[Autoload]
public static class MeetingRandomizer
{
    private static Random _random = new();
    
    static MeetingRandomizer()
    {
        GameEvents.OnGameStarted += GenerateRandomSeed;
        GameEvents.OnVotingCompleted += GenerateRandomSeed;
        GameEvents.OnMeetingStarted += Start;
    }

    public static void SetSeed(int seed)
    {
        _random = new Random(seed);
    }

    private static void Start()
    {
        if (!CustomOptionHolder.RandomizePlayersInMeeting.GetBool()) return;
        var meetingHud = MeetingHud.Instance;
        if (!meetingHud) return;
        var alivePlayers = meetingHud.playerStates
            .Where(area => !area.AmDead).ToList();
        alivePlayers.Sort(SortListByNames);
        var playerPositions = alivePlayers.Select(area => area.transform.localPosition).ToList();
        var playersList = alivePlayers
            .OrderBy(_ => _random.Next())
            .ToList();

        for (var i = 0; i < playersList.Count; i++)
        {
            playersList[i].transform.localPosition = playerPositions[i];
        }
    }
    
    private static int SortListByNames(PlayerVoteArea a, PlayerVoteArea b)
    {
        return string.CompareOrdinal(a.NameText.text, b.NameText.text);
    }

    private static void GenerateRandomSeed()
    {
        if (CachedPlayer.LocalPlayer == null || !AmongUsClient.Instance.AmHost) return;
        var seed = BetterOtherRoles.Rnd.Next();
        var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ShareMeetingRandomizerSeed, Hazel.SendOption.Reliable, -1);
        writer.Write(seed);
        AmongUsClient.Instance.FinishRpcImmediately(writer);
        SetSeed(seed);
    }
}