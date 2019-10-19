using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Items : MonoBehaviour
{
    [Header("Items Properties")]
    public string ItemName;
    public int Cost;
    public BoxCollider ItemCollider;

    [Header("Placement In World Properties")]
    public Vector3 PositionOffset;

    /// <summary>
    /// This method decides if the collider will be enabled or disabled.
    /// </summary>
    /// <param name="isEnable">The flag to enable or disable the item collider,
    ///                        of type BoxCollider</param>
    protected virtual void SetCollider(bool isEnable) { ItemCollider.enabled = isEnable; }

    /// <summary>
    /// This method returns the description of the item.
    /// </summary>
    /// <returns>The description of the item, of type string</returns>
    public virtual string GetDescription()
    {
        return ItemName;
    }

    /// <summary>
    /// This method sets the parent to the target and sets the position offset.
    /// </summary>
    /// <param name="target">The new parent for the item, of type Transform</param>
    public virtual void SetParentToWorld(Transform target)
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
    /// This method sets the parent to the target and sets the position offset.
    /// </summary>
    /// <param name="target">The new parent for the item, of type Transform</param>
    /// <param name="position">The new position for the item, of type Vector3</param>
    public virtual void SetParentToWorld(Transform target, Vector3 position)
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
        gameObject.SetActive(false);
    }
}
