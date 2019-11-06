using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactive
{
    [Header("Items Properties")]
    public int Cost;

    [Header("Placement In World Properties")]
    public Vector3 PositionOffset;

    /// <summary>
    /// This method drops the item in the target transform.
    /// </summary>
    /// <param name="target">The new parent for the item, of type Transform</param>
    public virtual void DropItem(Transform target)
    {
        transform.parent = target;

        // Making y axis to 0
        transform.position = new Vector3(transform.position.x,
                                         0,
                                         transform.position.z);
        // Adding the new offset position
        transform.position += PositionOffset;

        // Making rotation to 0
        transform.rotation = Quaternion.Euler(Vector3.zero);

        SetCollider(true); // Enabling collision trigger
    }

    /// <summary>
    /// This method drops the item in the target transform with a vector offset.
    /// </summary>
    /// <param name="target">The new parent for the item, of type Transform</param>
    /// <param name="position">The new position for the item, of type Vector3</param>
    public virtual void DropItem(Transform target, Vector3 position)
    {
        transform.parent = target;

        // Making y axis to 0
        transform.position = new Vector3(position.x,
                                         0,
                                         position.z);
        // Adding the new offset position
        transform.position += PositionOffset;

        // Making rotation to 0
        transform.rotation = Quaternion.Euler(Vector3.zero);

        SetCollider(true); // Enabling collision trigger
    }

    /// <summary>
    /// This method adds the item back to the world item list.
    /// </summary>
    public virtual void RemoveItem()
    {
        GameWorldManager.Instance.ReAddItem(this);
    }

    /// <summary>
    /// This method picks up the item and hides the item.
    /// </summary>
    public virtual void PickUpItem() { gameObject.SetActive(false); }

    /// <summary>
    /// This method picks up the item and puts it in the new transform
    /// </summary>
    /// <param name="target">The new parent of the item, of type Transform</param>
    public virtual void PickUpItem(Transform target) { transform.parent = target; }

    /// <summary>
    /// This method checks if the item is buyable by the player.
    /// </summary>
    /// <returns>True means the item is buyable, false otherwise, 
    ///          of type int</returns>
    public virtual bool IsBuyable()
    {
        return GameWorldManager.Instance.Player.Coins >= Cost;
    }

    /// <summary>
    /// This method buys the item and equips it.
    /// </summary>
    public virtual void BuyItem()
    {
        gameObject.SetActive(true); // Showing the gameobject
        GameWorldManager.Instance.Player.Buy(Cost); // Using up the coins to buy
        GameWorldManager.Instance.Player.AddObject(this); // Equipping the item
    }
}
