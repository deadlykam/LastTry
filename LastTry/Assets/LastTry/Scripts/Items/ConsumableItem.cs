using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Items
{
    [Header("Consumable Item Property")]
    public ConsumableType Consumable;
    public int ConsumableAmount;

    /// <summary>
    /// This method retursn the description of the consumable item.
    /// </summary>
    /// <returns>The description of the consumable item, of type string</returns>
    public override string GetDescription()
    {
        // Condition for health item description
        if(Consumable == ConsumableType.Heal)
            return base.GetDescription() + ". +" + ConsumableAmount.ToString() + "HP";

        return base.GetDescription();
    }

    /// <summary>
    /// This method gets the value of the consumable item.
    /// </summary>
    /// <returns>The value of the consumable item, of type int</returns>
    public int GetValue() { return ConsumableAmount; }

    /// <summary>
    /// This method picks up the item and returns the item back to the
    /// world item list.
    /// </summary>
    public override void PickUpItem()
    {
        RemoveItem();
        base.PickUpItem();
    }
}

public enum ConsumableType { None, Heal, Coin };