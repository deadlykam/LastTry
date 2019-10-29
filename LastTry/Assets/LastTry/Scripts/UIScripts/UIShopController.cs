using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShopController : UIMenuController
{
    [Header("UIShopController Properties")]
    public static UIShopController Instance;

    public UIShopItemInfo[] ItemInfos;

    private int _pointer; // Pointer to look at the current UIItemInfo

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    /// <summary>
    /// This method updates the UIItemInfo and shows the UIItemInfo.
    /// </summary>
    /// <param name="item">The upgradable item's info to show,
    ///                    of type UpgradableItem</param>
    private void SetItemInfo(UpgradableItem item)
    {
        // Condition to check if the item is not null
        // and updating the UIItemInfo
        if (item != null)
        {
            ItemInfos[_pointer].gameObject.SetActive(true);
            ItemInfos[_pointer].SetupInfo(item);
        }

        _pointer++;
    }

    /// <summary>
    /// This method opens the shop menu.
    /// </summary>
    public override void ShowMenu()
    {
        base.ShowMenu();

        _pointer = 0; // Resetting the pointer to use from the beginning
                      // of the ItemInfos

        // Loop for hiding all the UIItemInfos
        for (int i = 0; i < ItemInfos.Length; i++) ItemInfos[i].gameObject.SetActive(false);

        for(int i = 0; i < GameWorldManager.Instance.Player.WearableItemsLength; i++)
        {
            // Checking if the item is an UpgradableItem and is within the
            // ItemInfos range and also showing the item
            if(GameWorldManager.Instance.Player.GetWearableItem(i) as UpgradableItem
               && _pointer < ItemInfos.Length)
                SetItemInfo(GameWorldManager.Instance.Player.GetWearableItem(i));
        }
    }
}
