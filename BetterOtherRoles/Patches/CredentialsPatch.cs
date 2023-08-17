using HarmonyLib;
using System;
using BetterOtherRoles;
using BetterOtherRoles.CustomGameModes;
using BetterOtherRoles.Modules;
using BetterOtherRoles.Players;
using BetterOtherRoles.Utilities;
using TMPro;
using UnityEngine;

namespace BetterOtherRoles.Patches
{
    [HarmonyPatch]
    public static class CredentialsPatch
    {
        public const string ColoredLogo = "<color=#fcba03>Better</color><color=#ff351f>OtherRoles</color>";
        public const string BasedCopyright = "Based on <color=#ff351f>TheOtherRoles</color>";
        public const string CreatorsCopyright = "Created by <color=#7897d6ff>EnoPM</color>";
        public const string DingusRelease = "<color=#f779efff><b>Dingus special edition</b></color>";
        public const string EndOfLine = "\n";

        public static string fullCredentialsVersion =
            $@"<size=130%>{ColoredLogo}</size> v{BetterOtherRolesPlugin.Version.ToString()}";

        public static string fullCredentials =
            $@"{(DevConfig.IsDingusRelease ? $"<size=70%>{DingusRelease}</size>{EndOfLine}" : string.Empty)}<size=70%>{BasedCopyright}</size>{EndOfLine}<size=80%><b>{CreatorsCopyright}</b></size>";

        public static string mainMenuCredentials =
            $@"{(DevConfig.IsDingusRelease ? $"<size=90%>{DingusRelease}</size>{EndOfLine}" : string.Empty)}{BasedCopyright}{EndOfLine}<b>{CreatorsCopyright}</b>";

        [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
        internal static class PingTrackerPatch
        {
            public static GameObject modStamp;
            /*static void Prefix(PingTracker __instance) {
                if (modStamp == null) {
                    modStamp = new GameObject("ModStamp");
                    var rend = modStamp.AddComponent<SpriteRenderer>();
                    rend.sprite = TheOtherRolesPlugin.GetModStamp();
                    rend.color = new Color(1, 1, 1, 0.5f);
                    modStamp.transform.parent = __instance.transform.parent;
                    modStamp.transform.localScale *= SubmergedCompatibility.Loaded ? 0 : 0.6f;
                }
                float offset = (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started) ? 0.75f : 0f;
                modStamp.transform.position = FastDestroyableSingleton<HudManager>.Instance.MapButton.transform.position + Vector3.down * offset;
            }*/

            static void Postfix(PingTracker __instance)
            {
                __instance.text.alignment = TMPro.TextAlignmentOptions.TopRight;
                if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started)
                {
                    string gameModeText = $"";
                    if (HideNSeek.isHideNSeekGM) gameModeText = $"Hide 'N Seek";
                    else if (HandleGuesser.isGuesserGm) gameModeText = $"Guesser";
                    if (gameModeText != "") gameModeText = " - " + Helpers.cs(Color.yellow, gameModeText);
                    var needEol = gameModeText != string.Empty || DevConfig.IsDingusRelease;
                    __instance.text.text =
                        $"<size=130%>{ColoredLogo}</size> v{BetterOtherRolesPlugin.Version.ToString()}\n{(DevConfig.IsDingusRelease ? $"<size=70%>{DingusRelease}</size>" : string.Empty)}{gameModeText}{(needEol ? EndOfLine : string.Empty)}" +
                        __instance.text.text;
                    if (CachedPlayer.LocalPlayer.Data.IsDead || (!(CachedPlayer.LocalPlayer.PlayerControl == null) &&
                                                                 (CachedPlayer.LocalPlayer.PlayerControl ==
                                                                  Lovers.lover1 ||
                                                                  CachedPlayer.LocalPlayer.PlayerControl ==
                                                                  Lovers.lover2)))
                    {
                        __instance.transform.localPosition = new Vector3(3.45f, __instance.transform.localPosition.y,
                            __instance.transform.localPosition.z);
                    }
                    else
                    {
                        __instance.transform.localPosition = new Vector3(4.2f, __instance.transform.localPosition.y,
                            __instance.transform.localPosition.z);
                    }
                }
                else
                {
                    string gameModeText = $"";
                    if (TORMapOptions.gameMode == CustomGamemodes.HideNSeek) gameModeText = $"Hide 'N Seek";
                    else if (TORMapOptions.gameMode == CustomGamemodes.Guesser) gameModeText = $"Guesser";
                    if (gameModeText != "") gameModeText = Helpers.cs(Color.yellow, gameModeText) + "\n";

                    __instance.text.text =
                        $"{fullCredentialsVersion}\n  {gameModeText + fullCredentials}\n {__instance.text.text}";
                    __instance.transform.localPosition = new Vector3(3.5f, __instance.transform.localPosition.y,
                        __instance.transform.localPosition.z);
                }
            }
        }

        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
        public static class LogoPatch
        {
            public static SpriteRenderer renderer;
            public static Sprite bannerSprite;
            public static Sprite horseBannerSprite;
            public static Sprite banner2Sprite;
            private static PingTracker instance;

            static void Postfix(PingTracker __instance)
            {
                var torLogo = new GameObject("bannerLogo_TOR");
                torLogo.transform.SetParent(GameObject.Find("RightPanel").transform, false);
                torLogo.transform.localPosition = new Vector3(-0.4f, 0.5f, 5f);

                renderer = torLogo.AddComponent<SpriteRenderer>();
                loadSprites();
                renderer.sprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.Banner.png", 150f);

                instance = __instance;
                loadSprites();
                // renderer.sprite = TORMapOptions.enableHorseMode ? horseBannerSprite : bannerSprite;
                renderer.sprite = EventUtility.isEnabled ? banner2Sprite : bannerSprite;
                var credentialObject = new GameObject("credentialsTOR");
                var credentials = credentialObject.AddComponent<TextMeshPro>();
                credentials.SetText(
                    $"v{BetterOtherRolesPlugin.Version.ToString()}\n<size=30f%>\n</size>{mainMenuCredentials}\n");
                credentials.alignment = TMPro.TextAlignmentOptions.Center;
                credentials.fontSize *= 0.05f;

                credentials.transform.SetParent(torLogo.transform);
                credentials.transform.localPosition = Vector3.down * 1.2f;
            }

            public static void loadSprites()
            {
                if (bannerSprite == null)
                    bannerSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.Banner.png", 150f);
                if (banner2Sprite == null)
                    banner2Sprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.Banner2.png", 300f);
                if (horseBannerSprite == null)
                    horseBannerSprite =
                        Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.bannerTheHorseRoles.png", 300f);
            }

            public static void updateSprite()
            {
                loadSprites();
                if (renderer != null)
                {
                    float fadeDuration = 1f;
                    instance.StartCoroutine(Effects.Lerp(fadeDuration, new Action<float>((p) =>
                    {
                        renderer.color = new Color(1, 1, 1, 1 - p);
                        if (p == 1)
                        {
                            renderer.sprite = TORMapOptions.enableHorseMode ? horseBannerSprite : bannerSprite;
                            instance.StartCoroutine(Effects.Lerp(fadeDuration,
                                new Action<float>((p) => { renderer.color = new Color(1, 1, 1, p); })));
                        }
                    })));
                }
            }
        }
    }
}