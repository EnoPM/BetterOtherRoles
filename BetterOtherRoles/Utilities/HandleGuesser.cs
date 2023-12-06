using System;
using System.Collections.Generic;
using System.Text;
using BetterOtherRoles.CustomGameModes;
using BetterOtherRoles.Options;
using BetterOtherRoles.Roles;
using UnityEngine;

namespace BetterOtherRoles.Utilities {
    public static class HandleGuesser {
        private static Sprite targetSprite;
        public static bool isGuesserGm = false;
        public static bool hasMultipleShotsPerMeeting = false;
        public static bool killsThroughShield = true;
        public static bool evilGuesserCanGuessSpy = true;
        public static bool guesserCantGuessSnitch = false;

        public static Sprite getTargetSprite() {
            if (targetSprite) return targetSprite;
            targetSprite = Helpers.loadSpriteFromResources("BetterOtherRoles.Resources.TargetIcon.png", 150f);
            return targetSprite;
        }

        public static bool isGuesser(byte playerId) {
            if (isGuesserGm) return GuesserGM.isGuesser(playerId);
            return Guesser.isGuesser(playerId);
        }

        public static void clear(byte playerId) {
            if (isGuesserGm) GuesserGM.clear(playerId);
            else Guesser.clear(playerId);
        }

        public static int remainingShots(byte playerId, bool shoot = false) {
            if (isGuesserGm) return GuesserGM.remainingShots(playerId, shoot);
            return Guesser.remainingShots(playerId, shoot);
        }

        public static void clearAndReload() {
            Guesser.clearAndReload();
            GuesserGM.clearAndReload();
            isGuesserGm = TORMapOptions.gameMode == CustomGamemodes.Guesser;
            if (isGuesserGm) {
                guesserCantGuessSnitch = CustomOptionHolder.GuesserModeCantGuessSnitchIfTasksDone.GetBool();
                hasMultipleShotsPerMeeting = CustomOptionHolder.GuesserModeMultipleShotsPerMeeting.GetBool();
                killsThroughShield = CustomOptionHolder.GuesserModeKillsThroughShield.GetBool();
                evilGuesserCanGuessSpy = CustomOptionHolder.GuesserModeEvilCanKillSpy.GetBool();
            } else {
                guesserCantGuessSnitch = CustomOptionHolder.GuesserCantGuessSnitchIfTasksDone.GetBool();
                hasMultipleShotsPerMeeting = CustomOptionHolder.GuesserHasMultipleShotsPerMeeting.GetBool();
                killsThroughShield = CustomOptionHolder.GuesserKillsThroughShield.GetBool();
                evilGuesserCanGuessSpy = CustomOptionHolder.GuesserEvilCanKillSpy.GetBool();
            }

        }
    }
}
