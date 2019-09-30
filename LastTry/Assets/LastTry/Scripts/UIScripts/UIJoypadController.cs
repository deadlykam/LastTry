using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJoypadController : MonoBehaviour
{
    [Header("UI Joypad Controller Properties")]
    public PlayerCharacter Player;

    /// <summary>
    /// This method sends the button A command to the player.
    /// </summary>
    public void ButtonA() { Player.ButtonA(); }
}
