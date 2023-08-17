using System;
using BetterOtherRoles.Utilities.Attributes;
using UnityEngine;
using UniverseLib.Input;

namespace BetterOtherRoles.UI;

[RegisterInIl2Cpp]
public class BetterUIBehaviour : MonoBehaviour
{
    private void Update()
    {
        if (InputManager.GetKeyDown(KeyCode.F2))
        {
            UIManager.CustomOptionsPanel?.Toggle();
        }
#if DEBUG
        if (InputManager.GetKeyDown(KeyCode.F3))
        {
            UIManager.LocalOptionsPanel?.Toggle();
        }

        if (InputManager.GetKeyDown(KeyCode.F4))
        {
            UIManager.CreditsPanel?.Toggle();
        }
#endif
    }
}