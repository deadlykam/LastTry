using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemPopUpController : MonoBehaviour
{
    [Header("UI Item Popup Properties")]
    public Canvas[] AllCanvas;
    public TextMeshProUGUI Item1Name;
    public TextMeshProUGUI Item2Name;
    public TextMeshProUGUI Item1Description;
    public TextMeshProUGUI Item2Description;
    public Image HoldBar;

    public bool IsBarNotFinished { get { return HoldBar.fillAmount != 0f; } }

    /// <summary>
    /// This method hides/shows the menu.
    /// </summary>
    /// <param name="isShow"></param>
    private void SetUI(bool isShow)
    {
        // Loop for setting all the canvas
        for (int i = 0; i < AllCanvas.Length; i++) AllCanvas[i].enabled = isShow;
    }
    
    /// <summary>
    /// This method shows the menu and sets up all the attributes of the menu.
    /// </summary>
    /// <param name="item1Name">The name of the item 1, of type string</param>
    /// <param name="item2Name">The name of the item 2, of type string</param>
    /// <param name="item1Description">The description of item 1, of type string</param>
    /// <param name="item2Description">The description of item 2, of type string</param>
    public virtual void ShowMenu(string item1Name, string item2Name,
                         string item1Description, string item2Description)
    {
        SetUI(true);
        Item1Name.text = item1Name;
        Item2Name.text = item2Name;
        Item1Description.text = item1Description;
        Item2Description.text = item2Description;

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
