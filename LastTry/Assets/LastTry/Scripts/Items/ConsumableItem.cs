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
    /// This method handles all the pick up related events of the consumable item.
    /// </summary>
    /// <returns>The amount of value of the consumable item, of type int</returns>
    public int PickConsumable()
    {
        // Todo: Return availability to a manager here
        gameObject.SetActive(false);
        return ConsumableAmount;
    }
}

public enum ConsumableType { None, Heal, Coin };