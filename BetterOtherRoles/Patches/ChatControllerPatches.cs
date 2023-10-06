using System;
using System.Collections.Generic;
using System.Linq;
using AmongUs.Data;
using BetterOtherRoles.Modules;
using BetterOtherRoles.Players;
using HarmonyLib;
using InnerNet;
using UnityEngine;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(ChatController))]
public static class ChatControllerPatches
{
    private const string CommandPrefix = "/";
    private static readonly Dictionary<string, Action<List<string>>> Commands = new()
    {
        { "kick", KickCommand },
        { "ban", BanCommand },
        { "shield", ShieldCommand },
        { "tp", TpCommand }
    };

    private static void KickCommand(List<string> arguments)
    {
        if (arguments.Count == 0) return;
        var playerName = string.Join(" ", arguments);
        var target = CachedPlayer.AllPlayers.FirstOrDefault(x => x.Data.PlayerName.Equals(playerName));
        if (target == null || AmongUsClient.Instance == null || !AmongUsClient.Instance.CanBan()) return;
        var client = AmongUsClient.Instance.GetClient(target.PlayerControl.OwnerId);
        if (client == null) return; 
        AmongUsClient.Instance.KickPlayer(client.Id, false);
    }
    
    private static void BanCommand(List<string> arguments)
    {
        if (arguments.Count == 0) return;
        var playerName = string.Join(" ", arguments);
        var target = CachedPlayer.AllPlayers.FirstOrDefault(x => x.Data.PlayerName.Equals(playerName));
        if (target == null || AmongUsClient.Instance == null || !AmongUsClient.Instance.CanBan()) return;
        var client = AmongUsClient.Instance.GetClient(target.PlayerControl.OwnerId);
        if (client == null) return; 
        AmongUsClient.Instance.KickPlayer(client.Id, true);
    }
    
    private static void ShieldCommand(List<string> arguments)
    {
        if (arguments.Count == 0) return;
        var playerName = string.Join(" ", arguments);
        var target = CachedPlayer.AllPlayers.FirstOrDefault(x => x.Data.PlayerName.Equals(playerName));
        if (target == null || AmongUsClient.Instance == null || !AmongUsClient.Instance.CanBan()) return;
        FirstKillShield.FirstKilledPlayerName = target.Data.PlayerName;
    }
    
    private static void TpCommand(List<string> arguments)
    {
        if (arguments.Count == 0) return;
        if (!DevConfig.HasFlag("DEV_MODE") && !CachedPlayer.LocalPlayer.Data.IsDead) return;
        var playerName = string.Join(" ", arguments);
        var target = CachedPlayer.AllPlayers.FirstOrDefault(x => x.Data.PlayerName.Equals(playerName));
        if (target == null || AmongUsClient.Instance == null || !AmongUsClient.Instance.CanBan()) return;
        CachedPlayer.LocalPlayer.transform.position = target.transform.position;
    }
    
    [HarmonyPatch(nameof(ChatController.SendFreeChat))]
    [HarmonyPrefix]
    private static bool SendFreeChat(ChatController __instance)
    {
        var message = __instance.freeChatField.Text;
        if (message.StartsWith(CommandPrefix))
        {
            var command = message[1..].Split(" ").ToList();
            if (Commands.TryGetValue(command[0].ToLowerInvariant(), out var handler))
            {
                command.RemoveAt(0);
                handler(command);
                __instance.freeChatField.Clear();
            }
            else
            {
                System.Console.WriteLine($"Unknown command: {CommandPrefix}{command[0]}");
            }

            return false;
        }
        
        if (!DevConfig.HasFlag("CHAT_ALLOW_URLS") && UrlFinder.TryFindUrl(message.ToCharArray(), out _, out _))
        {
            ChatController.Logger.Warning($"{nameof(SendFreeChat)}() :: ABORTED, URL was found. Showing {StringNames.FreeChatLinkWarning} instead!");
            __instance.AddChatWarning(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.FreeChatLinkWarning));
        }
        else
        {
            ChatController.Logger.Debug($"SendFreeChat () :: Sending message: '{message}'");
            PlayerControl.LocalPlayer.RpcSendChat(message);
        }

        return false;
    }
    
    [HarmonyPatch(nameof(ChatController.Awake))]
    [HarmonyPrefix]
    private static void AwakePrefix()
    {
        if (!EOSManager.Instance.isKWSMinor) {
            DataManager.Settings.Multiplayer.ChatMode = QuickChatModes.FreeChatOrQuickChat;
        }
    }
    
    [HarmonyPatch(nameof(ChatController.SendChat))]
    [HarmonyPrefix]
    private static bool SendChatPrefix(ChatController __instance)
    {
        var num = (DevConfig.HasFlag("NO_CHAT_COOLDOWN") ? 0f : 3f) - __instance.timeSinceLastMessage;
        if (num > 0f)
        {
            __instance.sendRateMessageText.gameObject.SetActive(true);
            __instance.sendRateMessageText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.ChatRateLimit, Mathf.CeilToInt(num));
        }
        else
        {
            if (__instance.quickChatMenu.CanSend)
            {
                __instance.SendQuickChat();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(__instance.freeChatField.Text) || DataManager.Settings.Multiplayer.ChatMode != QuickChatModes.FreeChatOrQuickChat) return false;
                __instance.SendFreeChat();
            }
            __instance.timeSinceLastMessage = 0.0f;
            __instance.freeChatField.Clear();
            __instance.quickChatMenu.Clear();
            __instance.quickChatField.Clear();
            __instance.UpdateChatMode();
        }

        return false;
    }
}