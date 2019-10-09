using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;

    private void Update()
    {
        UpdateUIFollow();
    }

    /// <summary>
    /// This method is the update method of UIFollow class.
    /// </summary>
    protected void UpdateUIFollow() { transform.position = Target.position + Offset; }
}
