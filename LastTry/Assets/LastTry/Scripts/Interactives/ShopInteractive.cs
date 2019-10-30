using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteractive : InteractiveController
{

    /// <summary>
    /// This method opens the shop.
    /// </summary>
    public override void Action()
    {
        // Todo: Open shop UI here
        // Todo: Load player equipments here
        UIShopController.Instance.ShowMenu();
    }

    /// <summary>
    /// This method gets the description of the shop.
    /// </summary>
    /// <returns>The description of the shop, of type string</returns>
    public override string GetDescription()
    {
        return "To open " + ObjectName;
    }
}
