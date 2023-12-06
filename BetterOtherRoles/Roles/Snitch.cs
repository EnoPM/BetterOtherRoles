using System.Collections.Generic;
using BetterOtherRoles.Options;
using UnityEngine;

namespace BetterOtherRoles.Roles;

public static class Snitch
{
    public static PlayerControl snitch;
    public static Color color = new Color32(184, 251, 79, byte.MaxValue);

    public enum Mode
    {
        Chat = 0,
        Map = 1,
        ChatAndMap = 2
    }

    public enum Targets
    {
        EvilPlayers = 0,
        Killers = 1
    }

    public static Mode mode = Mode.Chat;
    public static Targets targets = Targets.EvilPlayers;
    public static int taskCountForReveal = 1;

    public static bool isRevealed = false;
    public static Dictionary<byte, byte> playerRoomMap = new Dictionary<byte, byte>();
    public static TMPro.TextMeshPro text = null;
    public static bool needsUpdate = true;

    public static void clearAndReload()
    {
        taskCountForReveal = Mathf.RoundToInt(CustomOptionHolder.SnitchLeftTasksForReveal.GetFloat());
        snitch = null;
        isRevealed = false;
        playerRoomMap = new Dictionary<byte, byte>();
        if (text != null) UnityEngine.Object.Destroy(text);
        text = null;
        needsUpdate = true;
        mode = (Mode)CustomOptionHolder.SnitchMode.CurrentSelection;
        targets = (Targets)CustomOptionHolder.SnitchTargets.CurrentSelection;
    }
}