using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq;
using static BetterOtherRoles.BetterOtherRoles;
using BetterOtherRoles.Modules;
using HarmonyLib;
using Hazel;
using BetterOtherRoles.Players;
using BetterOtherRoles.Utilities;
using System.Threading.Tasks;
using System.Net;
using BetterOtherRoles.CustomGameModes;
using AmongUs.GameOptions;
using BetterOtherRoles.Modifiers;
using BetterOtherRoles.Roles;
using Innersloth.Assets;

namespace BetterOtherRoles {

    public enum MurderAttemptResult {
        PerformKill,
        SuppressKill,
        BlankKill,
    }

    public enum CustomGamemodes {
        Classic,
        Guesser,
    }
    public static class Helpers
    {

        public static Dictionary<string, Sprite> CachedSprites = new();

        public static Sprite loadSpriteFromResources(string path, float pixelsPerUnit) {
            try
            {
                if (CachedSprites.TryGetValue(path + pixelsPerUnit, out var sprite)) return sprite;
                Texture2D texture = loadTextureFromResources(path);
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
                sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
                return CachedSprites[path + pixelsPerUnit] = sprite;
            } catch {
                System.Console.WriteLine("Error loading sprite from path: " + path);
            }
            return null;
        }

        public static unsafe Texture2D loadTextureFromResources(string path) {
            try {
                Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(path);
                var length = stream.Length;
                var byteTexture = new Il2CppStructArray<byte>(length);
                stream.Read(new Span<byte>(IntPtr.Add(byteTexture.Pointer, IntPtr.Size * 4).ToPointer(), (int) length));
                if (path.Contains("HorseHats")) {
                    byteTexture = new Il2CppStructArray<byte>(byteTexture.Reverse().ToArray());
                }
                ImageConversion.LoadImage(texture, byteTexture, false);
                return texture;
            } catch {
                System.Console.WriteLine("Error loading texture from resources: " + path);
            }
            return null;
        }

        public static Texture2D loadTextureFromDisk(string path) {
            try {          
                if (File.Exists(path))     {
                    Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, true);
                    var byteTexture = Il2CppSystem.IO.File.ReadAllBytes(path);
                    ImageConversion.LoadImage(texture, byteTexture, false);
                    return texture;
                }
            } catch {
                BetterOtherRolesPlugin.Logger.LogError("Error loading texture from disk: " + path);
            }
            return null;
        }

        public static AudioClip loadAudioClipFromResources(string path, string clipName = "UNNAMED_TOR_AUDIO_CLIP") {
            // must be "raw (headerless) 2-channel signed 32 bit pcm (le)" (can e.g. use Audacity� to export)
            try {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(path);
                var byteAudio = new byte[stream.Length];
                _ = stream.Read(byteAudio, 0, (int)stream.Length);
                float[] samples = new float[byteAudio.Length / 4]; // 4 bytes per sample
                int offset;
                for (int i = 0; i < samples.Length; i++) {
                    offset = i * 4;
                    samples[i] = (float)BitConverter.ToInt32(byteAudio, offset) / Int32.MaxValue;
                }
                int channels = 2;
                int sampleRate = 48000;
                AudioClip audioClip = AudioClip.Create(clipName, samples.Length / 2, channels, sampleRate, false);
                audioClip.SetData(samples, 0);
                return audioClip;
            } catch {
                System.Console.WriteLine("Error loading AudioClip from resources: " + path);
            }
            return null;

            /* Usage example:
            AudioClip exampleClip = Helpers.loadAudioClipFromResources("BetterOtherRoles.Resources.exampleClip.raw");
            if (Constants.ShouldPlaySfx()) SoundManager.Instance.PlaySound(exampleClip, false, 0.8f);
            */
        }
        public static PlayerControl playerById(byte id)
        {
            foreach (PlayerControl player in CachedPlayer.AllPlayers)
                if (player.PlayerId == id)
                    return player;
            return null;
        }
        
        public static Dictionary<byte, PlayerControl> allPlayersById()
        {
            Dictionary<byte, PlayerControl> res = new Dictionary<byte, PlayerControl>();
            foreach (PlayerControl player in CachedPlayer.AllPlayers)
                res.Add(player.PlayerId, player);
            return res;
        }

        public static void HandleStickyBomberExplodeOnBodyReport()
        {
            if (StickyBomber.Player == null || StickyBomber.StuckPlayer == null) return;
            Helpers.checkMurderAttemptAndKill(StickyBomber.Player, StickyBomber.StuckPlayer, showAnimation: false);
            var writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ShareGhostInfo, Hazel.SendOption.Reliable, -1);
            writer.Write(CachedPlayer.LocalPlayer.PlayerId);
            writer.Write((byte)RPCProcedure.GhostInfoTypes.DeathReasonAndKiller);
            writer.Write(StickyBomber.StuckPlayer.PlayerId);
            writer.Write((byte)DeadPlayer.CustomDeathReason.StickyBomb);
            writer.Write(StickyBomber.Player.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            GameHistory.overrideDeathReasonAndKiller(StickyBomber.StuckPlayer, DeadPlayer.CustomDeathReason.StickyBomb, killer: StickyBomber.Player);
            StickyBomber.RpcGiveBomb(byte.MaxValue);
        }

        public static void handleVampireBiteOnBodyReport() {
            // Murder the bitten player and reset bitten (regardless whether the kill was successful or not)
            Helpers.checkMurderAttemptAndKill(Vampire.vampire, Vampire.bitten, true, false);
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.VampireSetBitten, Hazel.SendOption.Reliable, -1);
            writer.Write(byte.MaxValue);
            writer.Write(byte.MaxValue);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            RPCProcedure.vampireSetBitten(byte.MaxValue, byte.MaxValue);
        }

        public static void refreshRoleDescription(PlayerControl player) {
            List<RoleInfo> infos = RoleInfo.getRoleInfoForPlayer(player); 
            List<string> taskTexts = new(infos.Count); 

            foreach (var roleInfo in infos)
            {
                taskTexts.Add(getRoleString(roleInfo));
            }
            
            var toRemove = new List<PlayerTask>();
            foreach (PlayerTask t in player.myTasks.GetFastEnumerator()) 
            {
                var textTask = t.TryCast<ImportantTextTask>();
                if (textTask == null) continue;
                
                var currentText = textTask.Text;
                
                if (taskTexts.Contains(currentText)) taskTexts.Remove(currentText); // TextTask for this RoleInfo does not have to be added, as it already exists
                else toRemove.Add(t); // TextTask does not have a corresponding RoleInfo and will hence be deleted
            }   

            foreach (PlayerTask t in toRemove) {
                t.OnRemove();
                player.myTasks.Remove(t);
                UnityEngine.Object.Destroy(t.gameObject);
            }

            // Add TextTask for remaining RoleInfos
            foreach (string title in taskTexts) {
                var task = new GameObject("RoleTask").AddComponent<ImportantTextTask>();
                task.transform.SetParent(player.transform, false);
                task.Text = title;
                player.myTasks.Insert(0, task);
            }
        }

        internal static string getRoleString(RoleInfo roleInfo)
        {
            if (roleInfo.name == "Jackal") 
            {
                var getSidekickText = Jackal.canCreateSidekick ? " and recruit a Sidekick" : "";
                return cs(roleInfo.color, $"{roleInfo.name}: Kill everyone{getSidekickText}");  
            }

            if (roleInfo.name == "Invert") 
            {
                return cs(roleInfo.color, $"{roleInfo.name}: {roleInfo.shortDescription} ({Invert.meetings})");
            }
            
            return cs(roleInfo.color, $"{roleInfo.name}: {roleInfo.shortDescription}");
        }
        
        public static bool isLighterColor(int colorId) {
            return CustomColors.lighterColors.Contains(colorId);
        }

        public static bool isCustomServer() {
            if (FastDestroyableSingleton<ServerManager>.Instance == null) return false;
            StringNames n = FastDestroyableSingleton<ServerManager>.Instance.CurrentRegion.TranslateName;
            return n != StringNames.ServerNA && n != StringNames.ServerEU && n != StringNames.ServerAS;
        }

        public static bool hasFakeTasks(this PlayerControl player) {
            return (player == Jester.jester || player == Jackal.jackal || player == Sidekick.sidekick || player == Arsonist.arsonist || player == Vulture.vulture || Jackal.formerJackals.Any(x => x == player));
        }

        public static bool canBeErased(this PlayerControl player) {
            return (player != Jackal.jackal && player != Sidekick.sidekick && !Jackal.formerJackals.Any(x => x == player));
        }

        public static bool shouldShowGhostInfo() {
            return CachedPlayer.LocalPlayer.PlayerControl != null && CachedPlayer.LocalPlayer.PlayerControl.Data.IsDead && TORMapOptions.ghostsSeeInformation || AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Ended;
        }

        public static void clearAllTasks(this PlayerControl player) {
            if (player == null) return;
            foreach (var playerTask in player.myTasks.GetFastEnumerator())
            {
                playerTask.OnRemove();
                UnityEngine.Object.Destroy(playerTask.gameObject);
            }
            player.myTasks.Clear();
            
            if (player.Data != null && player.Data.Tasks != null)
                player.Data.Tasks.Clear();
        }

        public static void setSemiTransparent(this PoolablePlayer player, bool value) {
            float alpha = value ? 0.25f : 1f;
            foreach (SpriteRenderer r in player.gameObject.GetComponentsInChildren<SpriteRenderer>())
                r.color = new Color(r.color.r, r.color.g, r.color.b, alpha);
            player.cosmetics.nameText.color = new Color(player.cosmetics.nameText.color.r, player.cosmetics.nameText.color.g, player.cosmetics.nameText.color.b, alpha);
        }

        public static string GetString(this TranslationController t, StringNames key, params Il2CppSystem.Object[] parts) {
            return t.GetString(key, parts);
        }

        public static string cs(Color c, string s) {
            return string.Format("<color=#{0:X2}{1:X2}{2:X2}{3:X2}>{4}</color>", ToByte(c.r), ToByte(c.g), ToByte(c.b), ToByte(c.a), s);
        }

        public static int lineCount(string text) {
            return text.Count(c => c == '\n');
        }

        private static byte ToByte(float f) {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);
        }

        public static KeyValuePair<byte, int> MaxPair(this Dictionary<byte, int> self, out bool tie) {
            tie = true;
            KeyValuePair<byte, int> result = new KeyValuePair<byte, int>(byte.MaxValue, int.MinValue);
            foreach (KeyValuePair<byte, int> keyValuePair in self)
            {
                if (keyValuePair.Value > result.Value)
                {
                    result = keyValuePair;
                    tie = false;
                }
                else if (keyValuePair.Value == result.Value)
                {
                    tie = true;
                }
            }
            return result;
        }

        public static bool hidePlayerName(PlayerControl source, PlayerControl target) {
            if (Camouflager.camouflageTimer > 0f) return true; // No names are visible
            if (Patches.SurveillanceMinigamePatch.nightVisionIsActive) return true;
            else if (Ninja.isInvisble && Ninja.ninja == target) return true;
            else if (!TORMapOptions.hidePlayerNames) return false; // All names are visible
            else if (source == null || target == null) return true;
            else if (source == target) return false; // Player sees his own name
            else if (source.Data.Role.IsImpostor && (target.Data.Role.IsImpostor || target == Spy.spy || target == Sidekick.sidekick && Sidekick.wasTeamRed || target == Jackal.jackal && Jackal.wasTeamRed)) return false; // Members of team Impostors see the names of Impostors/Spies
            else if ((source == Lovers.lover1 || source == Lovers.lover2) && (target == Lovers.lover1 || target == Lovers.lover2)) return false; // Members of team Lovers see the names of each other
            else if ((source == Jackal.jackal || source == Sidekick.sidekick) && (target == Jackal.jackal || target == Sidekick.sidekick || target == Jackal.fakeSidekick)) return false; // Members of team Jackal see the names of each other
            else if (Deputy.knowsSheriff && (source == Sheriff.sheriff || source == Deputy.deputy) && (target == Sheriff.sheriff || target == Deputy.deputy)) return false; // Sheriff & Deputy see the names of each other
            return true;
        }

        public static void setDefaultLook(this PlayerControl target, bool enforceNightVisionUpdate = true) {
            target.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId, enforceNightVisionUpdate);
        }

        public static void setLook(this PlayerControl target, String playerName, int colorId, string hatId, string visorId, string skinId, string petId, bool enforceNightVisionUpdate = true) {
            target.RawSetColor(colorId);
            target.RawSetVisor(visorId, colorId);
            target.RawSetHat(hatId, colorId);
            target.RawSetName(hidePlayerName(CachedPlayer.LocalPlayer.PlayerControl, target) ? "" : playerName);


            SkinViewData nextSkin = null;
            try { nextSkin = ShipStatus.Instance.CosmeticsCache.GetSkin(skinId); } catch { return; };
            
            PlayerPhysics playerPhysics = target.MyPhysics;
            AnimationClip clip = null;
            var spriteAnim = playerPhysics.myPlayer.cosmetics.skin.animator;
            var currentPhysicsAnim = playerPhysics.Animations.Animator.GetCurrentAnimation();


            if (currentPhysicsAnim == playerPhysics.Animations.group.RunAnim) clip = nextSkin.RunAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.SpawnAnim) clip = nextSkin.SpawnAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.EnterVentAnim) clip = nextSkin.EnterVentAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.ExitVentAnim) clip = nextSkin.ExitVentAnim;
            else if (currentPhysicsAnim == playerPhysics.Animations.group.IdleAnim) clip = nextSkin.IdleAnim;
            else clip = nextSkin.IdleAnim;
            float progress = playerPhysics.Animations.Animator.m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            playerPhysics.myPlayer.cosmetics.skin.skin = nextSkin;
            playerPhysics.myPlayer.cosmetics.skin.UpdateMaterial();

            spriteAnim.Play(clip, 1f);
            spriteAnim.m_animator.Play("a", 0, progress % 1);
            spriteAnim.m_animator.Update(0f);

            target.RawSetPet(petId, colorId);

            if (enforceNightVisionUpdate) Patches.SurveillanceMinigamePatch.enforceNightVision(target);
            Chameleon.update();  // so that morphling and camo wont make the chameleons visible
        }

        public static void showFlash(Color color, float duration=1f, string message="") {
            if (FastDestroyableSingleton<HudManager>.Instance == null || FastDestroyableSingleton<HudManager>.Instance.FullScreen == null) return;
            FastDestroyableSingleton<HudManager>.Instance.FullScreen.gameObject.SetActive(true);
            FastDestroyableSingleton<HudManager>.Instance.FullScreen.enabled = true;
            // Message Text
            TMPro.TextMeshPro messageText = GameObject.Instantiate(FastDestroyableSingleton<HudManager>.Instance.KillButton.cooldownTimerText, FastDestroyableSingleton<HudManager>.Instance.transform);
            messageText.text = message;
            messageText.enableWordWrapping = false;
            messageText.transform.localScale = Vector3.one * 0.5f;
            messageText.transform.localPosition += new Vector3(0f, 2f, -69f);
            messageText.gameObject.SetActive(true);
            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(duration, new Action<float>((p) => {
                var renderer = FastDestroyableSingleton<HudManager>.Instance.FullScreen;

                if (p < 0.5) {
                    if (renderer != null)
                        renderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01(p * 2 * 0.75f));
                } else {
                    if (renderer != null)
                        renderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01((1 - p) * 2 * 0.75f));
                }
                if (p == 1f && renderer != null) renderer.enabled = false;
                if (p == 1f) UnityEngine.Object.Destroy(messageText.gameObject);
            })));
        }

        public static bool roleCanUseVents(this PlayerControl player) {
            bool roleCouldUse = false;
            if (Engineer.engineer != null && Engineer.engineer == player)
                roleCouldUse = true;
            else if (Jackal.canUseVents && Jackal.jackal != null && Jackal.jackal == player)
                roleCouldUse = true;
            else if (Sidekick.canUseVents && Sidekick.sidekick != null && Sidekick.sidekick == player)
                roleCouldUse = true;
            else if (Spy.canEnterVents && Spy.spy != null && Spy.spy == player)
                roleCouldUse = true;
            else if (Vulture.canUseVents && Vulture.vulture != null && Vulture.vulture == player)
                roleCouldUse = true;
            else if (Thief.canUseVents &&  Thief.thief != null && Thief.thief == player)
                roleCouldUse = true;
            else if (player.Data?.Role != null && player.Data.Role.CanVent)  {
                if (Janitor.janitor != null && Janitor.janitor == CachedPlayer.LocalPlayer.PlayerControl)
                    roleCouldUse = false;
                else if (Mafioso.mafioso != null && Mafioso.mafioso == CachedPlayer.LocalPlayer.PlayerControl && Godfather.godfather != null && !Godfather.godfather.Data.IsDead)
                    roleCouldUse = false;
                else
                    roleCouldUse = true;
            }
            return roleCouldUse;
        }

        private static MurderAttemptResult LogAttempt(MurderAttemptResult result, string report = "")
        {
            BetterOtherRolesPlugin.Logger.LogMessage($"CheckMurderAttempt: {result.ToString()} {report}");
            return result;
        }

        public static MurderAttemptResult checkMuderAttempt(PlayerControl killer, PlayerControl target, bool blockRewind = false, bool ignoreBlank = false, bool ignoreIfKillerIsDead = false, bool showShieldAnimation = true, bool ignoreShield = false) {
            var targetRole = RoleInfo.getRoleInfoForPlayer(target, false).FirstOrDefault();

            var report = string.Empty;

            // Modified vanilla checks
            if (AmongUsClient.Instance.IsGameOver) return LogAttempt(MurderAttemptResult.SuppressKill, "game is over");
            if (killer == null || killer.Data == null || (killer.Data.IsDead && !ignoreIfKillerIsDead) || killer.Data.Disconnected) return LogAttempt(MurderAttemptResult.SuppressKill, "no killer data or dead killer"); // Allow non Impostor kills compared to vanilla code
            if (target == null || target.Data == null || target.Data.IsDead || target.Data.Disconnected) return LogAttempt(MurderAttemptResult.SuppressKill, "no target data or data dead"); // Allow killing players in vents compared to vanilla code

            if (GameOptionsManager.Instance.currentGameOptions.GameMode == GameModes.HideNSeek) return MurderAttemptResult.PerformKill;

            // Handle first kill attempt
            if (!ignoreShield && FirstKillShield.Enabled && FirstKillShield.ShieldedPlayer == target) return LogAttempt(MurderAttemptResult.SuppressKill, "target are shielded");

            // Handle blank shot
            if (!ignoreBlank && Pursuer.blankedList.Any(x => x.PlayerId == killer.PlayerId)) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.SetBlanked, Hazel.SendOption.Reliable, -1);
                writer.Write(killer.PlayerId);
                writer.Write((byte)0);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.setBlanked(killer.PlayerId, 0);

                return MurderAttemptResult.BlankKill;
            }

            // Block impostor shielded kill
            if (!ignoreShield && Medic.shielded != null && Medic.shielded == target) {
                if (showShieldAnimation)
                {
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(killer.NetId, (byte)CustomRPC.ShieldedMurderAttempt, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.shieldedMurderAttempt();
                    SoundEffectsManager.play("fail");
                }
                
                return LogAttempt(MurderAttemptResult.SuppressKill, "target are medic shielded");
            }

            // Block impostor not fully grown mini kill
            else if (Mini.mini != null && target == Mini.mini && !Mini.isGrownUp()) {
                return LogAttempt(MurderAttemptResult.SuppressKill, "target are mini");
            }

            // Block Time Master with time shield kill
            else if (!ignoreShield && TimeMaster.shieldActive && TimeMaster.timeMaster != null && TimeMaster.timeMaster == target) {
                if (!blockRewind) { // Only rewind the attempt was not called because a meeting startet 
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(killer.NetId, (byte)CustomRPC.TimeMasterRewindTime, Hazel.SendOption.Reliable, -1);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    RPCProcedure.timeMasterRewindTime();
                }
                return LogAttempt(MurderAttemptResult.SuppressKill, "target are timemaster shielded");
            }

            // Thief if hit crew only kill if setting says so, but also kill the thief.
            else if (Thief.isFailedThiefKill(target, killer, targetRole)) {
                Thief.suicideFlag = true;
                return LogAttempt(MurderAttemptResult.SuppressKill, "thief failed");
            }

            return MurderAttemptResult.PerformKill;
        }

        public static MurderAttemptResult checkMurderAttemptAndKill(PlayerControl killer, PlayerControl target, bool isMeetingStart = false, bool showAnimation = true, bool ignoreBlank = false, bool ignoreIfKillerIsDead = false)  {
            // The local player checks for the validity of the kill and performs it afterwards (different to vanilla, where the host performs all the checks)
            // The kill attempt will be shared using a custom RPC, hence combining modded and unmodded versions is impossible

            MurderAttemptResult murder = checkMuderAttempt(killer, target, isMeetingStart, ignoreBlank, ignoreIfKillerIsDead);
            if (murder == MurderAttemptResult.PerformKill) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UncheckedMurderPlayer, Hazel.SendOption.Reliable, -1);
                writer.Write(killer.PlayerId);
                writer.Write(target.PlayerId);
                writer.Write(showAnimation ? Byte.MaxValue : 0);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.uncheckedMurderPlayer(killer.PlayerId, target.PlayerId, showAnimation ? Byte.MaxValue : (byte)0);
            }
            return murder;            
        }

        public static List<PlayerControl> getKillerTeamMembers(PlayerControl player) {
            List<PlayerControl> team = new List<PlayerControl>();
            foreach(PlayerControl p in CachedPlayer.AllPlayers) {
                if (player.Data.Role.IsImpostor && p.Data.Role.IsImpostor && player.PlayerId != p.PlayerId && team.All(x => x.PlayerId != p.PlayerId)) team.Add(p);
                else if (player == Jackal.jackal && p == Sidekick.sidekick) team.Add(p); 
                else if (player == Sidekick.sidekick && p == Jackal.jackal) team.Add(p);
            }
            
            return team;
        }

        public static bool isNeutral(PlayerControl player) {
            RoleInfo roleInfo = RoleInfo.getRoleInfoForPlayer(player, false).FirstOrDefault();
            if (roleInfo != null)
                return roleInfo.isNeutral;
            return false;
        }

        public static bool isKiller(PlayerControl player) {
            return player.Data.Role.IsImpostor || 
                (isNeutral(player) && 
                player != Jester.jester && 
                player != Arsonist.arsonist && 
                player != Vulture.vulture && 
                player != Lawyer.lawyer && 
                player != Pursuer.pursuer);

        }

        public static bool isEvil(PlayerControl player) {
            return player.Data.Role.IsImpostor || isNeutral(player);
        }

        public static bool zoomOutStatus = false;
        public static void toggleZoom(bool reset=false) {
            float orthographicSize = reset || zoomOutStatus ? 3f : 12f;

            zoomOutStatus = !zoomOutStatus && !reset;
            Camera.main.orthographicSize = orthographicSize;
            foreach (var cam in Camera.allCameras) {
                if (cam != null && cam.gameObject.name == "UI Camera") cam.orthographicSize = orthographicSize;  // The UI is scaled too, else we cant click the buttons. Downside: map is super small.
            }

            if (HudManagerStartPatch.zoomOutButton != null) {
                HudManagerStartPatch.zoomOutButton.Sprite = zoomOutStatus ? Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.PlusButton.png", 75f) : Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.MinusButton.png", 150f);
                HudManagerStartPatch.zoomOutButton.PositionOffset = zoomOutStatus ? new Vector3(0f, 3f, 0) : new Vector3(0.4f, 2.8f, 0);
            }
            ResolutionManager.ResolutionChanged.Invoke((float)Screen.width / Screen.height, Screen.width, Screen.height, Screen.fullScreen); // This will move button positions to the correct position.
        }

        public static bool hasImpVision(GameData.PlayerInfo player) {
            return player.Role.IsImpostor
                || ((Jackal.jackal != null && Jackal.jackal.PlayerId == player.PlayerId || Jackal.formerJackals.Any(x => x.PlayerId == player.PlayerId)) && Jackal.hasImpostorVision)
                || (Sidekick.sidekick != null && Sidekick.sidekick.PlayerId == player.PlayerId && Sidekick.hasImpostorVision)
                || (Spy.spy != null && Spy.spy.PlayerId == player.PlayerId && Spy.hasImpostorVision)
                || (Jester.jester != null && Jester.jester.PlayerId == player.PlayerId && Jester.hasImpostorVision)
                || (Thief.thief != null && Thief.thief.PlayerId == player.PlayerId && Thief.hasImpostorVision);
        }
        
        public static object TryCast(this Il2CppObjectBase self, Type type)
        {
            return AccessTools.Method(self.GetType(), nameof(Il2CppObjectBase.TryCast)).MakeGenericMethod(type).Invoke(self, Array.Empty<object>());
        }

        public static void SetDeadBodyOutline(DeadBody target, Color? color)
        {
            if (target == null || target.bodyRenderers[0] == null) return;
            target.bodyRenderers[0].material.SetFloat("_Outline", color == null ? 0f : 1f);
            if (color != null) target.bodyRenderers[0].material.SetColor("_OutlineColor", color.Value);
        }
        
        public static DeadBody setDeadTarget(float maxDistance = 0f, PlayerControl targetingPlayer = null)
        {
            DeadBody result = null;
            float closestDistance = float.MaxValue;

            if (!MapUtilities.CachedShipStatus) return null;

            if (targetingPlayer == null) targetingPlayer = CachedPlayer.LocalPlayer.PlayerControl;
            if (targetingPlayer.Data.IsDead) return null;

            maxDistance = maxDistance == 0f ? 1f : maxDistance + 0.1f;

            Vector2 truePosition = targetingPlayer.GetTruePosition() - new Vector2(-0.2f, -0.22f);

            bool flag = GameOptionsManager.Instance.currentNormalGameOptions.GhostsDoTasks
                        && (!AmongUsClient.Instance || !AmongUsClient.Instance.IsGameOver);

            Collider2D[] allocs = Physics2D.OverlapCircleAll(truePosition, maxDistance,
                LayerMask.GetMask("Players", "Ghost"));


            foreach (Collider2D collider2D in allocs)
            {
                if (!flag || collider2D.tag != "DeadBody") continue;
                DeadBody component = collider2D.GetComponent<DeadBody>();

                if (!(Vector2.Distance(truePosition, component.TruePosition) <=
                      maxDistance)) continue;

                float distance = Vector2.Distance(truePosition, component.TruePosition);

                if (!(distance < closestDistance)) continue;

                result = component;
                closestDistance = distance;
            }

            if (result && Undertaker.Player == targetingPlayer)
                SetDeadBodyOutline(result, Undertaker.Color);

            return result;
        }

        public static void HandleUndertakerDropOnBodyReport()
        {
            if (Undertaker.Player == null) return;
            var position = Undertaker.DraggedBody != null
                ? Undertaker.DraggedBody.transform.position
                : Vector3.zero;
            Undertaker.DropBody(position);
        }
        
        public static Color ColorFromHex(string hexColor)
        {
            if (hexColor.IndexOf('#') != -1) hexColor = hexColor.Replace("#", string.Empty);

            var red = 0;
            var green = 0;
            var blue = 0;
            var alpha = 255;

            switch (hexColor.Length)
            {
                case 8:
                    red = int.Parse(hexColor.AsSpan(0, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                    green = int.Parse(hexColor.AsSpan(2, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                    blue = int.Parse(hexColor.AsSpan(4, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                    alpha = int.Parse(hexColor.AsSpan(6, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                    break;
                case 6:
                    red = int.Parse(hexColor.AsSpan(0, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                    green = int.Parse(hexColor.AsSpan(2, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                    blue = int.Parse(hexColor.AsSpan(4, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                    break;
                case 3:
                    red = int.Parse(
                        hexColor[0] + hexColor[0].ToString(),
                        NumberStyles.AllowHexSpecifier,
                        CultureInfo.InvariantCulture);
                    green = int.Parse(
                        hexColor[1] + hexColor[1].ToString(),
                        NumberStyles.AllowHexSpecifier,
                        CultureInfo.InvariantCulture);
                    blue = int.Parse(
                        hexColor[2] + hexColor[2].ToString(),
                        NumberStyles.AllowHexSpecifier,
                        CultureInfo.InvariantCulture);
                    break;
            }

            return new Color(red / 255f, green / 255f, blue / 255f, alpha / 255f);
        }
    }
    
    public static class Direction
    {
        public static Vector2 up = Vector2.up;
        public static Vector2 down = Vector2.down;
        public static Vector2 left = Vector2.left;
        public static Vector2 right = Vector2.right;
        public static Vector2 upleft = new Vector2(-0.70710677f, 0.70710677f);
        public static Vector2 upright = new Vector2(0.70710677f, 0.70710677f);
        public static Vector2 downleft = new Vector2(-0.70710677f, -0.70710677f);
        public static Vector2 downright = new Vector2(0.70710677f, -0.70710677f);
    }
}
