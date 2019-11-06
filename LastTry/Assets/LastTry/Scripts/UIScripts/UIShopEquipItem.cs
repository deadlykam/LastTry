using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopEquipItem : UIShopItem
{
    [Header("Shop Item Properties")]
    public Image ItemUpgradeBar;
    

    /// <summary>
    /// This method updates the item values.
    /// </summary>
    protected override void UpdateItemValues()
    {
        base.UpdateItemValues();

        // Setting the upgrade bar fill amount
        ItemUpgradeBar.fillAmount = Item.GetUpgradePercentage();

        // Condition to show item upgrade button because item
        // upgrade is available
        if (Item.IsUpgradable)
        {
            Button.SetActive(true);
            NonButton.SetActive(false);
        }
        // Conition to hide item upgrade button because item
        // upgrade is not available any more
        else
        {
            Button.SetActive(false);
            NonButton.SetActive(true);
        }
    }

    /// <summary>
    /// This method sets up the UIShopEquipItem.
    /// </summary>
    /// <param name="item">The item from which data are taken,
    ///                    of type UpgradableItem</param>
    public override void SetupInfo(UpgradableItem item)
    {
        base.SetupInfo(item); // Basic item setup
        SetupCost(item.UpgradeCost); // Setting the cost of the item
        UpdateItemValues(); // Updating item values
    }

    /// <summary>
    /// This method upgrades the upgradable item.
    /// </summary>
    public override void BtnAction()
    {
        if (Item as WeaponItem) ((WeaponItem)Item).UpgradeItem();
        else if (Item as WearableItem) ((WearableItem)Item).UpgradeItem();
        UpdateItemValues(); // Updating item values
    }
}
