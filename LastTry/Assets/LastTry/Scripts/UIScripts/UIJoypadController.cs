using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJoypadController : MonoBehaviour
{
    public static UIJoypadController Instance;

    [Header("UI Joypad Controller Properties")]
    public UIButtonHold ButtonAHold;

    private PlayerCharacter _player
    { get { return GameWorldManager.Instance.Player; } }

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
        // Condition for giving the fill amount for the Item Popup menus
        if (ButtonAHold.IsButtonPressed && _player.IsHoverObject)
            UIInGameUIController.Instance
                .SetAllBar(_player.HoverObject, ButtonAHold.GetHoldTimerPercentage());
        else if (!ButtonAHold.IsButtonPressed // Condition for resetting the bar
                && UIInGameUIController.Instance.IsBarNotDone())
            UIInGameUIController.Instance.ResetAllBar();
    }

    /// <summary>
    /// This method checks if the player is hovering over an item.
    /// </summary>
    /// <returns>True means the player is hovering over an item,
    ///          false otherwise, of type bool</returns>
    public bool IsHoverObject() { return _player.IsHoverObject; }

    /// <summary>
    /// This method picks up an item.
    /// </summary>
    public void PickUpObject() { _player.PickUpObjectInstant(); }

    /// <summary>
    /// This method returns the button A hold percentage.
    /// </summary>
    /// <returns>The hold percentage of button A, of type float</returns>
    public float GetButtonAHoldPercentage() { return ButtonAHold.GetHoldTimerPercentage(); }

    #region Button Methods
    /// <summary>
    /// This method sends the button A command to the player.
    /// </summary>
    public void ButtonA() { _player.ButtonA(); }

    /// <summary>
    /// This method sends the button B command to the player.
    /// </summary>
    public void ButtonB() { _player.ButtonB(); }
    #endregion Button Methods
}
