using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorldManager : MonoBehaviour
{
    public static GameWorldManager Instance;

    public Transform Root;
    public Transform Equipments;
    public Transform Consumables;

    [Range(0, 100)]
    public int DropRateEquipment;
    [Range(0, 100)]
    public int DropRateConsumables;

    private List<WeaponItem> _equipments;
    private List<ConsumableItem> _consumables;
    private Queue<ItemDropInfo> _requestItemDrops;

    private bool _isProcessing = false;
    private int _itemIndex = -1;

    private void Start()
    {
        // Helping garbage collector if there are any previous
        // GameWorldManager referenced
        Instance = null;

        // The instance will be replaced each time a new world is
        // loaded so that basic game world attributes can be used
        // again
        Instance = this;

        _equipments = new List<WeaponItem>();
        _consumables = new List<ConsumableItem>();
        _requestItemDrops = new Queue<ItemDropInfo>();

        // Loop for storing all equipment items
        for (int i = 0; i < Equipments.childCount; i++)
            _equipments.Add(Equipments.GetChild(i).GetComponent<WeaponItem>());

        // Loop for storing all consumable items
        for (int i = 0; i < Consumables.childCount; i++)
            _consumables.Add(Consumables.GetChild(i).GetComponent<ConsumableItem>());

        _isProcessing = false;
    }

    private void Update()
    {
        // Condition for processing an item drop
        if(_requestItemDrops.Count != 0 && !_isProcessing)
        {
            _isProcessing = true;
            ProcessRequest(); // Processing an item request
        }
    }

    /// <summary>
    /// This method processes the drop requests.
    /// </summary>
    private void ProcessRequest()
    {
        ItemDropInfo processDrop = _requestItemDrops.Dequeue();
        
        // Condition for dropping an equipment
        if(_equipments.Count != 0 && Random.Range(0, 100) < DropRateEquipment)
        {
            // Condition for dropping an item
            if (Random.Range(0, 100) < processDrop.DropRate)
            {
                _itemIndex = Random.Range(0, _equipments.Count); // Getting the item index

                _equipments[_itemIndex].gameObject.SetActive(true); // Enabling the item

                // Dropping the item
                _equipments[_itemIndex].SetParentToWorld(Equipments, processDrop.Position);

                _equipments.RemoveAt(_itemIndex); // Removing the item from index
            }
        }
        // Condition for dropping a consumable item
        else if (_consumables.Count != 0 && Random.Range(0, 100) < DropRateConsumables)
        {
            // Condition for dropping an item
            if (Random.Range(0, 100) < processDrop.DropRate)
            {
                _itemIndex = Random.Range(0, _consumables.Count); // Getting the item index

                _consumables[_itemIndex].gameObject.SetActive(true); // Enabling the item

                // Dropping the item
                _consumables[_itemIndex].SetParentToWorld(Consumables,
                                                          processDrop.Position);

                _consumables.RemoveAt(_itemIndex); // Removing the item from index
            }
        }

        _isProcessing = false; // Processing done
    }

    /// <summary>
    /// This method requests for an item to be dropped based on the drop rate.
    /// </summary>
    /// <param name="dropRate">The percentage at which an item will be dropped,
    ///                        of type int</param>
    /// <param name="position">The position at which the item will be dropped,
    ///                        of type Vector3</param>
    public void RequestItemDrop(int dropRate, Vector3 position)
    {
        _requestItemDrops.Enqueue(new ItemDropInfo(dropRate, position));
    }

    /// <summary>
    /// This method adds back an item for being dropped again.
    /// </summary>
    /// <param name="item">The item to add back, of type Items</param>
    public void ReAddItem(Items item)
    {
        // Conditions for adding back items
        if (item as WeaponItem) _equipments.Add(((WeaponItem)item));
        else if (item as ConsumableItem) _consumables.Add(((ConsumableItem)item));
    }
}

public struct ItemDropInfo
{
    public int DropRate;
    public Vector3 Position;

    /// <summary>
    /// This constructor creats the ItemDropInfo struct object.
    /// </summary>
    /// <param name="dropRate">The drop rate for droping an item, of type int</param>
    /// <param name="position">The position for the item to be dropped, 
    ///                        of type Vector3</param>
    public ItemDropInfo(int dropRate, Vector3 position)
    {
        DropRate = dropRate;
        Position = position;
    }
}