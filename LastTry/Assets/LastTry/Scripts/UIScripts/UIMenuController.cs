using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuController : MonoBehaviour
{
    [Header("UIMenuController Properties")]
    public Canvas[] Canvases;

    /// <summary>
    /// This method enables/disables all the canvases.
    /// </summary>
    /// <param name="isEnabled">The flag to enable/disable canvases, of type bool</param>
    private void SetCanvases(bool isEnabled)
    {
        // Loop for enabling/disabling the canvases
        for (int i = 0; i < Canvases.Length; i++)
            Canvases[i].enabled = isEnabled;
    }

    /// <summary>
    /// This method shows the menu.
    /// </summary>
    public virtual void ShowMenu() { SetCanvases(true); }

    /// <summary>
    /// This method hides the menu.
    /// </summary>
    public virtual void BtnClose() { SetCanvases(false); }
}
