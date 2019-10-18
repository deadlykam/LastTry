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
}
