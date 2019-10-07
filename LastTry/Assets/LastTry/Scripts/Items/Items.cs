using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Items : MonoBehaviour
{
    public string Title;
    public int cost;

    /// <summary>
    /// This method returns the description of the item.
    /// </summary>
    /// <returns>The description of the item, of type string</returns>
    public virtual string GetDescription()
    {
        return Title;
    }
}
