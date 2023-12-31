using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using Hazel;
using BetterOtherRoles.Players;
using BetterOtherRoles.Utilities;
using System.Linq;
using AmongUsSpecimen;
using BetterOtherRoles.Modules;
using BetterOtherRoles.Options;

namespace BetterOtherRoles.Patches {
    public class GameStartManagerPatch  {
        public static float timer = 600f;
        private static float kickingTimer = 0f;
        private static string lobbyCodeText = "";

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start))]
        public class GameStartManagerStartPatch {
            public static void Postfix(GameStartManager __instance) {
                // Trigger version refresh
                // Reset lobby countdown timer
                timer = 600f; 
                // Reset kicking timer
                kickingTimer = 0f;
                // Copy lobby code
                string code = InnerNet.GameCode.IntToGameName(AmongUsClient.Instance.GameId);
                GUIUtility.systemCopyBuffer = code;
                lobbyCodeText = FastDestroyableSingleton<TranslationController>.Instance.GetString(StringNames.RoomCode, new Il2CppReferenceArray<Il2CppSystem.Object>(0)) + "\r\n" + code;
            }
        }

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
        public class GameStartManagerUpdatePatch {
            public static float startingTimer = 0;
            private static bool update = false;
            private static string currentText = "";
        
            public static void Prefix(GameStartManager __instance) {
                if (!GameData.Instance ) return; // No instance
                update = GameData.Instance.PlayerCount != __instance.LastPlayerCount;
                if (DevConfig.DisablePlayerRequirementToLaunch)
                {
                    __instance.MinPlayers = 1;
                }
        
                if (__instance.startState == GameStartManager.StartingStates.Countdown)
                {
                    if (AmongUsClient.Instance.AmHost)
                    {
                        __instance.startLabelText.text = "Stop";
                        var pos = __instance.GameStartText.transform.localPosition;
                        pos.y = 0.6f;
                        __instance.GameStartText.transform.localPosition = pos;
                        __instance.StartButton.gameObject.SetActive(true);
                    }
                }
                else if (__instance.startState == GameStartManager.StartingStates.NotStarting)
                {
                    __instance.startLabelText.text =
                        DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.StartLabel);
                }
            }

            public static void Postfix(GameStartManager __instance) {
                // Start Timer
                if (startingTimer > 0) {
                    startingTimer -= Time.deltaTime;
                }
                // Lobby timer
                if (!GameData.Instance) return; // No instance

                if (update) currentText = __instance.PlayerCounter.text;

                timer = Mathf.Max(0f, timer -= Time.deltaTime);
                int minutes = (int)timer / 60;
                int seconds = (int)timer % 60;
                string suffix = $" ({minutes:00}:{seconds:00})";

                __instance.PlayerCounter.text = currentText + suffix;
                __instance.PlayerCounter.autoSizeTextContainer = true;

                if (AmongUsClient.Instance.AmHost) {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ShareGamemode, Hazel.SendOption.Reliable, -1);
                    writer.Write((byte) TORMapOptions.gameMode);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.shareGamemode((byte) TORMapOptions.gameMode);
                }
            }
        }

        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.BeginGame))]
        public class GameStartManagerBeginGame {
            public static bool Prefix(GameStartManager __instance) {
                // Block game start if not everyone has the same mod version
                var continueStart = true;

                if (__instance.startState != GameStartManager.StartingStates.NotStarting)
                {
                    __instance.ResetStartState();
                    return false;
                }

                if (AmongUsClient.Instance.AmHost) {
                    if (CustomOptionHolder.RandomMap.GetBool() && continueStart) {
                        // 0 = Skeld
                        // 1 = Mira HQ
                        // 2 = Polus
                        // 3 = Airship
                        // 4 = The Fungle
                        byte chosenMapId = 0;
                        var maps = new List<CustomOptionHolder.RandomMapModOptionMap>
                        {
                            CustomOptionHolder.TheSkeldMap,
                            CustomOptionHolder.PolusMap,
                            CustomOptionHolder.MiraHqMap,
                            CustomOptionHolder.AirshipMap,
                            CustomOptionHolder.TheFungleMap
                        };
                        var probabilities = new List<float>
                        {
                            CustomOptionHolder.TheSkeldMap.Percentage.GetFloat() / 100f,
                            CustomOptionHolder.PolusMap.Percentage.GetFloat() / 100f,
                            CustomOptionHolder.MiraHqMap.Percentage.GetFloat() / 100f,
                            CustomOptionHolder.AirshipMap.Percentage.GetFloat() / 100f,
                            CustomOptionHolder.TheFungleMap.Percentage.GetFloat() / 100f
                        };

                        // if any map is at 100%, remove all maps that are not!
                        if (probabilities.Contains(1.0f)) {
                            for (int i=0; i < probabilities.Count; i++) {
                                if (probabilities[i] != 1.0) probabilities[i] = 0;
                            }
                        }

                        float sum = probabilities.Sum();
                        if (sum == 0) return continueStart;  // All maps set to 0, why are you doing this???
                        for (int i = 0; i < probabilities.Count; i++) {  // Normalize to [0,1]
                            probabilities[i] /= sum;
                        }
                        float selection = (float)BetterOtherRoles.Rnd.NextDouble();
                        float cumsum = 0;
                        for (byte i = 0; i < probabilities.Count; i++) {
                            cumsum += probabilities[i];
                            if (cumsum > selection) {
                                chosenMapId = i;
                                break;
                            }
                        }

                        // Translate chosen map to presets page and use that maps random map preset page
                        var chosenMap = maps.Count > chosenMapId ? maps[chosenMapId] : null;
                        if (chosenMap != null && chosenMap.ShouldUseSpecificPreset.GetBool())
                        {
                            CoreOptions.PresetSelection.CurrentSelection = chosenMap.PresetName.CurrentSelection;
                        }
                        if (chosenMapId >= 3) chosenMapId++;  // Skip dlekS
                                                              
                        var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.DynamicMapOption, Hazel.SendOption.Reliable, -1);
                        writer.Write(chosenMapId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                        RPCProcedure.dynamicMapOption(chosenMapId);
                    }
                }
                
                return continueStart;
            }
        }
        
        [HarmonyPatch(typeof(GameStartManager))]
        public class GameStartManagerPatches
        {
            [HarmonyPatch(nameof(GameStartManager.ReallyBegin))]
            [HarmonyPostfix]
            private static void ReallyBeginPostfix(GameStartManager __instance)
            {
                __instance.StartButton.gameObject.SetActive(true);
            }
        }
    }
}
