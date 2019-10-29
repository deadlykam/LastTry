using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class InteractiveController : MonoBehaviour
{
    [Header("Interactive Properties")]
    public string InteractiveName;
    public BoxCollider InteractiveCollider;


    /// <summary>
    /// This method decides if the collider will be enabled or disabled.
    /// </summary>
    /// <param name="isEnable">The flag to enable or disable the iteractive collider,
    ///                        of type BoxCollider</param>
    protected virtual void SetCollider(bool isEnable)
    { InteractiveCollider.enabled = isEnable; }

    /// <summary>
    /// This method performs the action of the interactive.
    /// </summary>
    public virtual void Action() { /*Override method to perform actions*/ }

    /// <summary>
    /// This method gets the description of the interactive.
    /// </summary>
    /// <returns>The description of the interactive, of type string</returns>
    public virtual string GetDescription() { return InteractiveName; }
}
