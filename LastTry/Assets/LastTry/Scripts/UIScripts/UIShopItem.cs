using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour
{
    [Header("UIShopItem Properties")]
    public Image ItemIcon;
    public TextMeshProUGUI Attribute;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemCost;

    public GameObject Button;
    public GameObject NonButton;

    private UpgradableItem _item;
    public UpgradableItem Item { get { return _item; } }

    /// <summary>
    /// This method updates the item values.
    /// </summary>
    protected virtual void UpdateItemValues()
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
    }

    /// <summary>
    /// This method sets up the UIShopItem.
    /// </summary>
    /// <param name="item">The item from which data are taken,
    ///                    of type UpgradableItem</param>
    public virtual void SetupInfo(UpgradableItem item)
    {
        _item = item; // Setting the item
        ItemName.text = _item.name; // Setting the item name
    }

    /// <summary>
    /// This method sets up the cost of the item.
    /// </summary>
    /// <param name="amount">The cost of the item, of type int</param>
    public virtual void SetupCost(int amount) { ItemCost.text = amount.ToString(); }

    /// <summary>
    /// This method does a button action, this method must be overriden by
    /// child class.
    /// </summary>
    public virtual void BtnAction() { }
}
