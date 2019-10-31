using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
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
        // Condition for coin item description
        else if (Consumable == ConsumableType.Coin)
            return base.GetDescription() + ". +" + ConsumableAmount.ToString() + " Coins";

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

    /// <summary>
    /// This method sets the consumable amount.
    /// </summary>
    /// <param name="amount">The new consumable amount of the item,
    ///                      of type int</param>
    public void SetConsumableAmount(int amount) { ConsumableAmount = amount; }
}

public enum ConsumableType { None, Heal, Coin };