using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJoypadController : MonoBehaviour
{
    [Header("UI Joypad Controller Properties")]
    public PlayerCharacter Player;

    /// <summary>
    /// This method checks if the player is hovering over an item.
    /// </summary>
    /// <returns>True means the player is hovering over an item,
    ///          false otherwise, of type bool</returns>
    public bool IsHoverItem() { return Player.IsHoverWeapon; }

    /// <summary>
    /// This method picks up an item.
    /// </summary>
    public void PickUpItem() { Player.PickUpItemInstant(); }

    #region Button Methods
    /// <summary>
    /// This method sends the button A command to the player.
    /// </summary>
    public void ButtonA() { Player.ButtonA(); }
    #endregion Button Methods
}
