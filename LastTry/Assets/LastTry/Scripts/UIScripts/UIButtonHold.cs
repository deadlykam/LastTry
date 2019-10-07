using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("UI Button Hold Properties")]
    public UnityEvent OnButtonPressed;
    public float HoldTimer;
    private float _holdTimer = 0;

    private bool _isButtonPressed = false;
    public bool IsButtonPressed { get { return _isButtonPressed; } }

    // Update is called once per frame
    void Update()
    {
        // Condition for checking if player is hovering over an item
        if (UIJoypadController.Instance.IsHoverItem())
        {
            if (_isButtonPressed) // Condition to check if button pressed
            {
                // Counting the timer for holding
                _holdTimer = (_holdTimer + Time.deltaTime) >= HoldTimer ?
                              HoldTimer : _holdTimer + Time.deltaTime;

                // Condition for invoking the method
                if (_holdTimer == HoldTimer)
                {
                    OnButtonPressed.Invoke();
                    ResetHold();
                }
            }
        }
    }

    /// <summary>
    /// This method resets the hold action.
    /// </summary>
    private void ResetHold()
    {
        _isButtonPressed = false;
        _holdTimer = 0;
    }

    /// <summary>
    /// Condition for pressing down.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        _isButtonPressed = true;
    }

    /// <summary>
    /// Condition for pressing up.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        _isButtonPressed = false;
        ResetHold();
    }

    /// <summary>
    /// This method returns the percentage value of the hold timer.
    /// </summary>
    /// <returns>The percentage value of the hold timer, of type float</returns>
    public float GetHoldTimerPercentage() { return _holdTimer / HoldTimer; }
}
