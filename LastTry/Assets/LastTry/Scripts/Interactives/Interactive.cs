using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Interactive : MonoBehaviour
{
    [Header("Interactive Properties")]
    public string ObjectName;
    public BoxCollider ObjectCollider;


    /// <summary>
    /// This method decides if the collider will be enabled or disabled.
    /// </summary>
    /// <param name="isEnable">The flag to enable or disable the iteractive collider,
    ///                        of type BoxCollider</param>
    protected virtual void SetCollider(bool isEnable)
    { ObjectCollider.enabled = isEnable; }

    /// <summary>
    /// This method gets the description of the interactive.
    /// </summary>
    /// <returns>The description of the interactive, of type string</returns>
    public virtual string GetDescription() { return ObjectName; }
}
