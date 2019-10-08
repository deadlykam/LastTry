using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInGameUIController : MonoBehaviour
{
    public static UIInGameUIController Instance;

    [Header("Popup Menu Properties")]
    public UIItemPopUpController WeaponItemPopup;

    [Header("Player Health Bar Properties")]
    public Image PlayerHealthBar;
    public float PlayerHealthBarTimer;
    private float _playerHealthBarTimer;
    private float _playerHealth = 1f;

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
        // Condition for transitioning the health bar
        if (_playerHealthBarTimer != 1f)
        {
            _playerHealthBarTimer = (_playerHealthBarTimer 
                                    + (PlayerHealthBarTimer * Time.deltaTime)) >= 
                                    1f ? 
                                    1 : 
                                    _playerHealthBarTimer + 
                                    (PlayerHealthBarTimer * Time.deltaTime);

            // Lerping the health bar
            PlayerHealthBar.fillAmount = Mathf.Lerp(
                                            PlayerHealthBar.fillAmount,
                                            _playerHealth,
                                            PlayerHealthBarTimer);
        }
    }

    /// <summary>
    /// This method shows the weapon popup menu.
    /// </summary>
    /// <param name="item1Name">The name of the item 1, of type string</param>
    /// <param name="item2Name">The name of the item 2, of type string</param>
    /// <param name="item1Description">The description of item 1, of type string</param>
    /// <param name="item2Description">The description of item 2, of type string</param>
    public void ShowWeaponPopup(string item1Name, string item2Name,
                         string item1Description, string item2Description)
    {
        // Showing the popup menu
        WeaponItemPopup.ShowMenu(item1Name, item2Name, item1Description, item2Description);
    }

    /// <summary>
    /// This method hides the menu popup.
    /// </summary>
    public void HideWeaponPopup() { WeaponItemPopup.HideMenu(); }

    /// <summary>
    /// This method sets bar value for the Weapon Popup menu.
    /// </summary>
    /// <param name="amount">The fill amount for the popup bar in the range from
    ///                      0 - 1, of type float</param>
    public void SetWeaponBar(float amount) { WeaponItemPopup.SetFillAmount(amount); }

    /// <summary>
    /// This method checks if the weapon bar is not finished.
    /// </summary>
    /// <returns></returns>
    public bool IsWeaponBarNotDone() { return WeaponItemPopup.IsBarNotFinished; }

    /// <summary>
    /// This method sets the fill amount for the player health bar.
    /// </summary>
    /// <param name="amount">The amount to set for the player health bar,
    ///                      of type float</param>
    public void SetPlayerHealthBar(float amount)
    {
        _playerHealth = amount;
        _playerHealthBarTimer = 0;
    }
}
