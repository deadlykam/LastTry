using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopEquipItem : UIShopItem
{
    [Header("Shop Item Properties")]
    /*public Image ItemIcon;
    public TextMeshProUGUI Attribute;*/
    public Image ItemUpgradeBar;
    /*public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemCost;

    public GameObject Upgrade;
    public GameObject Max;

    private UpgradableItem _item;*/
    

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
    /// This method sets up the ItemInfo.
    /// </summary>
    /// <param name="item">The item from which data are taken,
    ///                    of type UpgradableItem</param>
    public override void SetupInfo(UpgradableItem item)
    {
        /*_item = item; // Setting the item
        UpdateItemValues(); // Updating item values
        ItemName.text = _item.name; // Setting the item name
        ItemCost.text = _item.UpgradeCost.ToString(); // Setting upgrade cost*/
        base.SetupInfo(item);
        SetupCost(item.UpgradeCost); // Setting the cost of the item
        UpdateItemValues(); // Updating item values
    }

    /*/// <summary>
    /// This method updates the upgradable item.
    /// </summary>
    public void BtnUpgrade()
    {
        if (_item as WeaponItem) ((WeaponItem)_item).UpgradeItem();
        else if (_item as WearableItem) ((WearableItem)_item).UpgradeItem();
        UpdateItemValues(); // Updating item values
    }*/

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
