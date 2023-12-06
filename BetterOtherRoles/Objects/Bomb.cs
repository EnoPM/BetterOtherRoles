using Hazel;
using System.Collections;
using BepInEx.Unity.IL2CPP.Utils;
using BetterOtherRoles.Players;
using BetterOtherRoles.Roles;
using BetterOtherRoles.Utilities;
using UnityEngine;

namespace BetterOtherRoles.Objects {
    public class Bomb {
        public GameObject bomb;
        public GameObject background;

        private static Sprite bombSprite;
        private static Sprite backgroundSprite;
        private static Sprite defuseSprite;
        public static bool canDefuse = false;

        public static Sprite getBombSprite() {
            if (bombSprite) return bombSprite;
            bombSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.Bomb.png", 300f);
            return bombSprite;
        }
        public static Sprite getBackgroundSprite() {
            if (backgroundSprite) return backgroundSprite;
            backgroundSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.BombBackground.png", 110f / Bomber.hearRange);
            return backgroundSprite;
        }

        public static Sprite getDefuseSprite() {
            if (defuseSprite) return defuseSprite;
            defuseSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.Bomb_Button_Defuse.png", 115f);
            return defuseSprite;
        }

        public Bomb(Vector2 p) {
            bomb = new GameObject("Bomb") { layer = 11 };
            bomb.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
            Vector3 position = new Vector3(p.x, p.y, p.y / 1000 + 0.001f); // just behind player
            bomb.transform.position = position;

            background = new GameObject("Background") { layer = 11 };
            background.transform.SetParent(bomb.transform);
            background.transform.localPosition = new Vector3(0, 0, -1f); // before player
            background.transform.position = position;

            var bombRenderer = bomb.AddComponent<SpriteRenderer>();
            bombRenderer.sprite = getBombSprite();
            var backgroundRenderer = background.AddComponent<SpriteRenderer>();
            backgroundRenderer.sprite = getBackgroundSprite();

            bomb.SetActive(false);
            background.SetActive(false);
            if (CachedPlayer.LocalPlayer.PlayerControl == Bomber.bomber) {
                bomb.SetActive(true);
            }
            Bomber.bomb = this;
            backgroundRenderer.color = Color.white;
            Bomber.isActive = false;

            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(CoPlantBomb(backgroundRenderer, p));
        }
        
        private IEnumerator CoPlantBomb(SpriteRenderer backgroundRenderer, Vector2 p)
        {
            var timer = 0f;
            while (timer < Bomber.bombActiveAfter)
            {
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            timer = 0f;
            bomb.SetActive(true);
            background.SetActive(true);
            SoundEffectsManager.playAtPosition("bombFuseBurning", p, Bomber.destructionTime, Bomber.hearRange, true);
            Bomber.isActive = true;
            while (timer < Bomber.destructionTime)
            {
                timer += Time.deltaTime;
                var x = timer / Bomber.destructionTime;
                if (backgroundRenderer)
                {
                    backgroundRenderer.color = Mathf.Clamp01(x) * Color.red + Mathf.Clamp01(1 - x) * Color.white;
                }
                yield return new WaitForEndOfFrame();
            }
            if (bomb) explode(this);
        }
        
        public static void explode(Bomb b) {
            if (b == null) return;
            if (Bomber.bomber != null) {
                var position = b.bomb.transform.position;
                var distance = Vector2.Distance(position, CachedPlayer.LocalPlayer.transform.position);  // every player only checks that for their own client (desynct with positions sucks)
                if (distance < Bomber.destructionRange && !CachedPlayer.LocalPlayer.Data.IsDead) {
                    Helpers.checkMurderAttemptAndKill(Bomber.bomber, CachedPlayer.LocalPlayer.PlayerControl, false, false, true, true);
                    
                    MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ShareGhostInfo, Hazel.SendOption.Reliable, -1);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    writer.Write((byte)RPCProcedure.GhostInfoTypes.DeathReasonAndKiller);
                    writer.Write(CachedPlayer.LocalPlayer.PlayerId);
                    writer.Write((byte)DeadPlayer.CustomDeathReason.Bomb);
                    writer.Write(Bomber.bomber.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                    GameHistory.overrideDeathReasonAndKiller(CachedPlayer.LocalPlayer, DeadPlayer.CustomDeathReason.Bomb, killer: Bomber.bomber);
                }
                SoundEffectsManager.playAtPosition("bombExplosion", position, range: Bomber.hearRange) ;
            }
            Bomber.clearBomb();
            canDefuse = false;
            Bomber.isActive = false;
        }

        public static void update() {
            if (Bomber.bomb == null || !Bomber.isActive) {
                canDefuse = false;
                return;
            }
            Bomber.bomb.background.transform.Rotate(Vector3.forward * 50 * Time.fixedDeltaTime);

            if (MeetingHud.Instance && Bomber.bomb != null) {
                Bomber.clearBomb();
            }

            if (Vector2.Distance(CachedPlayer.LocalPlayer.PlayerControl.GetTruePosition(), Bomber.bomb.bomb.transform.position) > 1f) canDefuse = false;
            else canDefuse = true;
        }

        public static void clearBackgroundSprite() {
            backgroundSprite = null;
        }
    }
}