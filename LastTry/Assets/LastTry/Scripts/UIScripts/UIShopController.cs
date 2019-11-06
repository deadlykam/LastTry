using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShopController : UIMenuController
{
    public static UIShopController Instance;

    [Header("UIShopController Properties")]
    public Transform BuyItemTransforms;
    private List<UpgradableItem> _buyItemList;
    public Canvas EquipmentsCanvas;
    public Canvas BuyCanvas;

    [Header("Item Menus Properties")]
    public UIShopEquipItem[] EquipmentItems;
    public UIShopBuyItem[] BuyItems;

    [Header("Highlighter Properties")]
    ///<summary>
    /// index 0 = Equipment
    /// index 1 = Buy
    /// index 2 = Sell
    /// </summary>
    public GameObject[] Highlighters;

    private int _pointer; // Pointer to look at the current UIItemInfo

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        // Initializing the list of buy items
        _buyItemList = new List<UpgradableItem>();

        // Loop for storing all the buyable upgradable items
        for(int i = 0; i < BuyItemTransforms.childCount; i++)
        {
            // Adding the buyable upgradable item
            _buyItemList.Add(BuyItemTransforms.GetChild(i).GetComponent<UpgradableItem>());
        }
    }

    /// <summary>
    /// This method shows the selected highlighter
    /// </summary>
    /// <param name="index">The index of the highlighter to show,
    ///                     of type, int
    ///                     <para>index 0 = Equipment</para>
    ///                     <para>index 1 = Buy</para>
    ///                     <para>index 2 = Sell,</para></param>
    private void ShowHighlighter(int index)
    {
        // Condition for highlighter being already shown
        if (Highlighters[index].activeSelf) return;

        // Loop for showing index highlighter and hiding
        // the other highlighters
        for(int i = 0; i < Highlighters.Length; i++)
        {
            // Conditions for showing the index highlighter
            // and hiding the non index highlighters
            if (i == index) Highlighters[i].SetActive(true);
            else Highlighters[i].SetActive(false);
        }
    }

    /// <summary>
    /// [Depricated]This method updates the UIItemInfo and shows the UIItemInfo.
    /// </summary>
    /// <param name="item">The upgradable item's info to show,
    ///                    of type UpgradableItem</param>
    private void SetItemInfo(UpgradableItem item)
    {
        // Condition to check if the item is not null
        // and updating the UIItemInfo
        if (item != null)
        {
            EquipmentItems[_pointer].gameObject.SetActive(true);
            EquipmentItems[_pointer].SetupInfo(item);
        }

        _pointer++;
    }

    /// <summary>
    /// This method shows all the buyable items.
    /// </summary>
    private void SetBuyItemInfo()
    {
        _pointer = 0; // Resetting the pointer to use from the beginning
                      // of the BuyItems

        // Loop for hiding all the UIShopBuyItem
        for (int i = 0; i < BuyItems.Length; i++)
            BuyItems[i].gameObject.SetActive(false);

        // Loop for adding all the buyable items to the shop
        for(int i = 0; i < _buyItemList.Count; i++)
        {
            // Adding buyable item to the shop
            BuyItems[_pointer].gameObject.SetActive(true);
            BuyItems[_pointer].SetupInfo(_buyItemList[i]);
            _pointer++;
        }
    }

    /// <summary>
    /// This method sets up the EquipmentItemInfos.
    /// </summary>
    private void SetEquipmentItemInfo()
    {
        _pointer = 0; // Resetting the pointer to use from the beginning
                      // of the EquipmentItems

        // Loop for hiding all the UIShopEquipItem
        for (int i = 0; i < EquipmentItems.Length; i++)
            EquipmentItems[i].gameObject.SetActive(false);

        // Checking if the weapon is not null and showing the weapon item
        if (GameWorldManager.Instance.Player.GetDefaultWeapon() != null)
            SetItemInfo(GameWorldManager.Instance.Player.GetDefaultWeapon());

        // Loop for the equipment items
        for (int i = 0; i < GameWorldManager.Instance.Player.WearableItemsLength; i++)
        {
            // Checking if the item is an UpgradableItem and is within the
            // EquipmentItems range and also showing the item
            if (GameWorldManager.Instance.Player.GetWearableItem(i) as UpgradableItem
               && _pointer < EquipmentItems.Length)
            {
                if (GameWorldManager.Instance.Player.GetWearableItem(i) != null)
                {
                    EquipmentItems[_pointer].gameObject.SetActive(true);
                    EquipmentItems[_pointer].SetupInfo(GameWorldManager
                        .Instance.Player.GetWearableItem(i));

                    _pointer++;
                }
            }
        }
    }

    /// <summary>
    /// This method opens the shop menu.
    /// </summary>
    public override void ShowMenu()
    {
        base.ShowMenu();

        EquipmentsCanvas.enabled = true;

        SetEquipmentItemInfo(); // Showing equipment items
        SetBuyItemInfo(); // Showing buyable items

        ShowHighlighter(0); // Showing the equipment highlighter
    }

    /// <summary>
    /// This method removes the bought item from the list.
    /// </summary>
    /// <param name="item">The item to remove, of type Upgradable</param>
    public void RemoveBuyItem(UpgradableItem item)
    {
        _buyItemList.Remove(item);
    }

    #region Buttons
    /// <summary>
    /// This method closes the menu.
    /// </summary>
    public override void BtnClose()
    {
        EquipmentsCanvas.enabled = false;
        BuyCanvas.enabled = false;
        base.BtnClose();
    }

    /// <summary>
    /// This method shows the item buy list.
    /// </summary>
    public void BtnOpenBuyMenu()
    {
        EquipmentsCanvas.enabled = false;
        BuyCanvas.enabled = true;

        ShowHighlighter(1); // Showing the buy highlighter
    }

    /// <summary>
    /// This method shows the equipment list.
    /// </summary>
    public void BtnOpenEquipmentMenu()
    {
        EquipmentsCanvas.enabled = true;
        BuyCanvas.enabled = false;
        SetEquipmentItemInfo();

        ShowHighlighter(0); // Showing the equipment highlighter
    }
    #endregion Buttons
}
