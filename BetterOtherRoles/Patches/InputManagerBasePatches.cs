using System;
using HarmonyLib;
using Il2CppSystem.Collections;
using Rewired;
using Rewired.Data;
using UnityEngine;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(InputManager_Base))]
public static class InputManagerBasePatches
{
    [HarmonyPatch(nameof(InputManager_Base.Awake))]
    [HarmonyPrefix]
    private static void AwakePrefix(InputManager_Base __instance)
    {
        __instance.userData.RegisterBind("ActionUsePortal", "Use A Portal", KeyboardKeyCode.P);
        __instance.userData.RegisterBind("ActionZoomOut", "Zoom Out", KeyboardKeyCode.KeypadPlus);
        __instance.userData.RegisterBind("ActionModifier", "Modifier Ability", KeyboardKeyCode.M);
        __instance.userData.RegisterBind("ActionPlaceGarlic", "Place Garlic", KeyboardKeyCode.G);
        __instance.userData.RegisterBind("ActionDefuseBomb", "Defuse Bomb", KeyboardKeyCode.R);
        __instance.userData.RegisterBind("ActionTransferBomb", "Transfer sticky Bomb", KeyboardKeyCode.T);
        __instance.userData.RegisterBind("ActionToggleUpdater", "Toggle mod updater", KeyboardKeyCode.U);
    }
    
    private static int RegisterBind(this UserData self, string name, string description, KeyboardKeyCode keycode, int elementIdentifierId = -1, int category = 0, InputActionType type = InputActionType.Button)
    {
        self.AddAction(category);
        var action = self.GetAction(self.actions.Count - 1)!;

        action.name = name;
        action.descriptiveName = description;
        action.categoryId = category;
        action.type = type;
        action.userAssignable = true;

        var a = new ActionElementMap();
        a._elementIdentifierId = elementIdentifierId;
        a._actionId = action.id;
        a._elementType = ControllerElementType.Button;
        a._axisContribution = Pole.Positive;
        a._keyboardKeyCode = keycode;
        a._modifierKey1 = ModifierKey.None;
        a._modifierKey2 = ModifierKey.None;
        a._modifierKey3 = ModifierKey.None;
        self.keyboardMaps._items[0].actionElementMaps.Add(a);
        self.joystickMaps._items[0].actionElementMaps.Add(a);
            
        return action.id;
    }
}