using HarmonyLib;
using System;
using BetterOtherRoles.Utilities;
using BetterOtherRoles.CustomGameModes;
using BetterOtherRoles.Modifiers;
using BetterOtherRoles.Roles;

namespace BetterOtherRoles;

[HarmonyPatch]
public static class BetterOtherRoles
{
    public static readonly Random Rnd = new((int)DateTime.Now.Ticks);

    public static void clearAndReloadRoles()
    {
        Jester.clearAndReload();
        Mayor.clearAndReload();
        Portalmaker.clearAndReload();
        Engineer.clearAndReload();
        Sheriff.clearAndReload();
        Deputy.clearAndReload();
        Lighter.clearAndReload();
        Godfather.clearAndReload();
        Mafioso.clearAndReload();
        Janitor.clearAndReload();
        Detective.clearAndReload();
        TimeMaster.clearAndReload();
        Medic.clearAndReload();
        Shifter.clearAndReload();
        Swapper.clearAndReload();
        Lovers.clearAndReload();
        Seer.clearAndReload();
        Morphling.clearAndReload();
        Camouflager.clearAndReload();
        Hacker.clearAndReload();
        Tracker.clearAndReload();
        Vampire.clearAndReload();
        Snitch.clearAndReload();
        Jackal.clearAndReload();
        Sidekick.clearAndReload();
        Eraser.clearAndReload();
        Spy.clearAndReload();
        Trickster.clearAndReload();
        Cleaner.clearAndReload();
        Warlock.clearAndReload();
        SecurityGuard.clearAndReload();
        Arsonist.clearAndReload();
        BountyHunter.clearAndReload();
        Vulture.clearAndReload();
        Medium.clearAndReload();
        Lawyer.clearAndReload();
        Pursuer.clearAndReload();
        Witch.clearAndReload();
        Ninja.clearAndReload();
        Thief.clearAndReload();
        Trapper.clearAndReload();
        Bomber.clearAndReload();
        Fallen.ClearAndReload();
        Undertaker.ClearAndReload();
        StickyBomber.ClearAndReload();

        // Modifier
        Bait.clearAndReload();
        Bloody.clearAndReload();
        AntiTeleport.clearAndReload();
        Tiebreaker.clearAndReload();
        Sunglasses.clearAndReload();
        Mini.clearAndReload();
        Vip.clearAndReload();
        Invert.clearAndReload();
        Chameleon.clearAndReload();

        // Gamemodes
        HandleGuesser.clearAndReload();
    }
}