using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopItemInfo : MonoBehaviour
{
    [Header("Shop Item Properties")]
    public Image ItemIcon;
    public TextMeshProUGUI Attribute;
    public Image ItemUpgradeBar;
    public TextMeshProUGUI ItemName;

    public GameObject Upgrade;
    public GameObject Max;

    private UpgradableItem _item;
    

    /// <summary>
    /// This method updates the item values.
    /// </summary>
    private void UpdateItemValues()
    {
        if (_item as WeaponItem) // Checking if item is WeaponItem
        {
            // Updating the attribute field
            Attribute.text = ((WeaponItem)_item).GetAttributeDescription();
        }
        else if (_item as WearableItem) // Checking if the item is WearableItem
        {
            // Updating the attribute field
            Attribute.text = ((WearableItem)_item).GetAttributeDescription();
        }

        // Setting the upgrade bar fill amount
        ItemUpgradeBar.fillAmount = _item.GetUpgradePercentage();

        // Condition to show item upgrade button because item
        // upgrade is available
        if (_item.IsUpgradable)
        {
            Upgrade.SetActive(true);
            Max.SetActive(false);
        }
        // Conition to hide item upgrade button because item
        // upgrade is not available any more
        else
        {
            Upgrade.SetActive(false);
            Max.SetActive(true);
        }
    }

    /// <summary>
    /// This method sets up the ItemInfo.
    /// </summary>
    /// <param name="item">The item from which data are taken,
    ///                    of type UpgradableItem</param>
    public void SetupInfo(UpgradableItem item)
    {
        _item = item; // Setting the item
        UpdateItemValues(); // Updating item values
        ItemName.text = _item.name; // Setting the item name
    }

    /// <summary>
    /// This method updates the upgradable item.
    /// </summary>
    public void BtnUpgrade()
    {
        if (_item as WeaponItem) ((WeaponItem)_item).UpgradeItem();
        else if (_item as WearableItem) ((WearableItem)_item).UpgradeItem();
        UpdateItemValues(); // Updating item values
    }
}
