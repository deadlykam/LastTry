using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIEquipmentPopUpController : UIItemPopUpController
{
    [Header("UI Equipment Popup Properties")]
    public TextMeshProUGUI Item2Name;
    public TextMeshProUGUI Item2Description;

    /// <summary>
    /// This method shows the menu and sets up all the attributes of the menu.
    /// </summary>
    /// <param name="item1Name">The name of the item 1, of type string</param>
    /// <param name="item2Name">The name of the item 2, of type string</param>
    /// <param name="item1Description">The description of item 1, of type string</param>
    /// <param name="item2Description">The description of item 2, of type string</param>
    public void ShowMenu(string item1Name, string item2Name, 
                         string item1Description, string item2Description)
    {
        base.ShowMenu(item1Name, item1Description);

        Item2Name.text = item2Name;
        Item2Description.text = item2Description;
    }
}
