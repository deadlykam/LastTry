using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInGameUIController : MonoBehaviour
{
    public static UIInGameUIController Instance;

    [Header("Popup Menu Properties")]
    public UIEquipmentPopUpController EquipmentItemPopup;
    public UIItemPopUpController ConsumableItemPopup;

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
                                            _playerHealthBarTimer);
        }
    }

    /// <summary>
    /// This method shows the consumable popup menu.
    /// </summary>
    /// <param name="itemName">The name of the item, of type string</param>
    /// <param name="itemDescription">The description of the item, of type string</param>
    public void ShowConsumablePopup(string itemName, string itemDescription)
    {
        HideAllPopUp(); // Hiding any shown popup
        // Showing the popup menu
        ConsumableItemPopup.ShowMenu(itemName, itemDescription);
    }

    /// <summary>
    /// This method shows the equipment popup menu.
    /// </summary>
    /// <param name="item1Name">The name of the item 1, of type string</param>
    /// <param name="item2Name">The name of the item 2, of type string</param>
    /// <param name="item1Description">The description of item 1, of type string</param>
    /// <param name="item2Description">The description of item 2, of type string</param>
    public void ShowEquipmentPopup(string item1Name, string item2Name,
                         string item1Description, string item2Description)
    {
        HideAllPopUp(); // Hiding any shown popup
        // Showing the popup menu
        EquipmentItemPopup.ShowMenu(item1Name, item2Name, 
                                    item1Description, item2Description);
    }

    /// <summary>
    /// This method hides all the pop up menu.
    /// </summary>
    public void HideAllPopUp()
    {
        if(EquipmentItemPopup.IsMenuShown) EquipmentItemPopup.HideMenu();
        if (ConsumableItemPopup.IsMenuShown) ConsumableItemPopup.HideMenu();
    }

    /// <summary>
    /// This method sets the bar for the appropriate pop up menu.
    /// </summary>
    /// <param name="interactive">To check the type of object, of type 
    ///                           Interactive</param>
    /// <param name="amount">The fill amount for the popup bar in the range from
    ///                      0 - 1, of type float</param>
    public void SetAllBar(Interactive interactive, float amount)
    {
        // Condition for setting the weapon pop up bar
        if (interactive as WeaponItem)
            EquipmentItemPopup.SetFillAmount(amount);
        // Condition for setting the consumable
        else if (interactive as ConsumableItem)
            ConsumableItemPopup.SetFillAmount(amount);
        // Condition for setting the wearable 
        else if(interactive as WearableItem)
        {
            EquipmentItemPopup.SetFillAmount(amount);
            ConsumableItemPopup.SetFillAmount(amount);
        }
    }

    /// <summary>
    /// Resets all the non-resetted bars.
    /// </summary>
    public void ResetAllBar()
    {
        // Conditions to check if fill amount not resetted for all menus
        if (EquipmentItemPopup.IsBarNotFinished) EquipmentItemPopup.SetFillAmount(0);
        if (ConsumableItemPopup.IsBarNotFinished) ConsumableItemPopup.SetFillAmount(0);
    }

    /// <summary>
    /// [Depricated]This method checks if the weapon bar is not finished.
    /// </summary>
    /// <returns></returns>
    public bool IsEquipmentBarNotDone() { return EquipmentItemPopup.IsBarNotFinished; }

    /// <summary>
    /// This method checks if the bar is not finished in any of the pop up menus.
    /// </summary>
    /// <returns>True means not finished, false otherwise, of type bool</returns>
    public bool IsBarNotDone()
    {
        return EquipmentItemPopup.IsBarNotFinished || ConsumableItemPopup.IsBarNotFinished;
    }

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
