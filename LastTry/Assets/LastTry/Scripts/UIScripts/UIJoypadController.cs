﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJoypadController : MonoBehaviour
{
    public static UIJoypadController Instance;

    [Header("UI Joypad Controller Properties")]
    public PlayerCharacter Player;
    public UIButtonHold ButtonAHold;

    private void Awake()
    {
        // Condition for initializing the singleton
        if (Instance == null)
        {
            Instance = this; // Singleton initialized
            DontDestroyOnLoad(gameObject); // Not destroying on load
        }
        else Destroy(gameObject); // Already initialized so
                                  // destroying
    }

    private void Update()
    {
        // Remove this comment section is no future problem arises
        // from commenting this out
        /*// Condition for giving the fill amount for the Weapon Popup menu
        if (ButtonAHold.IsButtonPressed)
            UIInGameUIController.Instance
                .SetWeaponBar(ButtonAHold.GetHoldTimerPercentage());
        else if (!ButtonAHold.IsButtonPressed // Condition for resetting the bar
                && UIInGameUIController.Instance.IsWeaponBarNotDone())
            UIInGameUIController.Instance.SetWeaponBar(0);*/

        // Condition for giving the fill amount for the Weapon Popup menu
        if (ButtonAHold.IsButtonPressed && Player.IsHoverItem)
            UIInGameUIController.Instance
                .SetAllBar(Player.HoverItem, ButtonAHold.GetHoldTimerPercentage());
        else if (!ButtonAHold.IsButtonPressed // Condition for resetting the bar
                && UIInGameUIController.Instance.IsWeaponBarNotDone())
            UIInGameUIController.Instance.ResetAllBar();
    }

    /// <summary>
    /// This method checks if the player is hovering over an item.
    /// </summary>
    /// <returns>True means the player is hovering over an item,
    ///          false otherwise, of type bool</returns>
    public bool IsHoverItem() { return Player.IsHoverItem; }

    /// <summary>
    /// This method picks up an item.
    /// </summary>
    public void PickUpItem() { Player.PickUpItemInstant(); }

    /// <summary>
    /// This method returns the button A hold percentage.
    /// </summary>
    /// <returns>The hold percentage of button A, of type float</returns>
    public float GetButtonAHoldPercentage() { return ButtonAHold.GetHoldTimerPercentage(); }

    #region Button Methods
    /// <summary>
    /// This method sends the button A command to the player.
    /// </summary>
    public void ButtonA() { Player.ButtonA(); }
    #endregion Button Methods
}
