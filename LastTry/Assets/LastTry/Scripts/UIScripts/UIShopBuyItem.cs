using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopBuyItem : UIShopItem
{

    [Header("UIShopBuyItem Properties")]
    public GameObject Highlight;

    /// <summary>
    /// This method updates the buy item value.
    /// </summary>
    protected override void UpdateItemValues()
    {
        base.UpdateItemValues();

        Button.SetActive(true);     // Showing buy button
        NonButton.SetActive(false); // Hiding sold button
    }

    /// <summary>
    /// This method sets up the UIShopBuyItem.
    /// </summary>
    /// <param name="item">The item from which data are taken,
    ///                    of type UpgradableItem</param>
    public override void SetupInfo(UpgradableItem item)
    {
        base.SetupInfo(item); // Basic item setup
        SetupCost(item.Cost); // Setting the cost of the item
        UpdateItemValues();   // Updating item values
        Highlight.SetActive(false); // Hiding the highlighter
    }

    /// <summary>
    /// This method buys the item.
    /// </summary>
    public override void BtnAction()
    {
        // Condition to check if item is buyable
        if (Item.IsBuyable())
        {
            Button.SetActive(false);  // Hiding buy button
            NonButton.SetActive(true);// Showing sold button

            Item.BuyItem(); // Buying the item
            Highlight.SetActive(true); // Showing the bought
                                       // Highlighter

            // Removing the item because it has been
            // bought
            UIShopController.Instance.RemoveBuyItem(Item);
        }
    }
}
