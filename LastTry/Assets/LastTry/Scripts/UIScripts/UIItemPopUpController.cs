using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemPopUpController : MonoBehaviour
{
    [Header("UI Item Popup Properties")]
    public Canvas[] AllCanvas;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemDescription;
    public Image HoldBar;

    public bool IsBarNotFinished { get { return HoldBar.fillAmount != 0f; } }
    public bool IsMenuShown { get { return AllCanvas[0].enabled; } }

    /// <summary>
    /// This method hides/shows the menu.
    /// </summary>
    /// <param name="isShow">The flag to hide or show the menu, of type bool</param>
    private void SetUI(bool isShow)
    {
        // Loop for setting all the canvas
        for (int i = 0; i < AllCanvas.Length; i++) AllCanvas[i].enabled = isShow;
    }
    
    /// <summary>
    /// This method shows the menu and sets up all the attributes of the menu.
    /// </summary>
    /// <param name="itemName">The name of the item, of type string</param>
    /// <param name="itemDescription">The description of item, of type string</param>
    public virtual void ShowMenu(string itemName, string itemDescription)
    {
        SetUI(true);
        ItemName.text = itemName;
        ItemDescription.text = itemDescription;

        HoldBar.fillAmount = 0; // Resetting the fill amount;
    }

    /// <summary>
    /// This method hides the menu.
    /// </summary>
    public virtual void HideMenu() { SetUI(false); }

    /// <summary>
    /// This method sets the fill amount for the HoldBar which must be in the
    /// range from 0 - 1 and positive.
    /// </summary>
    /// <param name="amount">The amount for the fill amount in the hold bar,
    ///                      of type float</param>
    public void SetFillAmount(float amount) { HoldBar.fillAmount = amount; }
}
