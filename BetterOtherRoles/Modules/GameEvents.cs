using BetterOtherRoles.Players;
using JetBrains.Annotations;

namespace BetterOtherRoles.Modules;

public static class GameEvents
{
    public static event GameStartedHandler? OnGameStarted;

    public delegate void GameStartedHandler();

    public static void TriggerGameStarted() => OnGameStarted?.Invoke();


    public static event TaskCompletedHandler? OnTaskCompleted;

    public delegate void TaskCompletedHandler(PlayerControl player, PlayerTask task);

    public static void TriggerTaskCompleted(PlayerControl player, PlayerTask task) =>
        OnTaskCompleted?.Invoke(player, task);

    
    public static event PlayerLeftHandler? OnPlayerLeft;

    public delegate void PlayerLeftHandler(int ownerId);

    public static void TriggerPlayerLeft(int ownerId) => OnPlayerLeft?.Invoke(ownerId);


    public static event GameEndedHandler? OnGameEnded;

    public delegate void GameEndedHandler();

    public static void TriggerEndGame() => OnGameEnded?.Invoke();

    public static event MeetingStarted? OnMeetingStarted;

    public delegate void MeetingStarted();

    public static void TriggerMeetingStarted() => OnMeetingStarted?.Invoke();


    public static event MeetingEndedHandler? OnMeetingEnded;

    public delegate void MeetingEndedHandler(CachedPlayer? playerExiled);

    public static void TriggerMeetingEnded(CachedPlayer? playerExiled) => OnMeetingEnded?.Invoke(playerExiled);

    public static event VotingCompleted? OnVotingCompleted;
    public delegate void VotingCompleted();

    public static void TriggerVotingCompleted() => OnVotingCompleted?.Invoke();
}