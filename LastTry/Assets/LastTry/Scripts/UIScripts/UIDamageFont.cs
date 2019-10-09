using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDamageFont : MonoBehaviour
{
    public TextMeshProUGUI DamageValue;
    public float TransitionTime;
    private float _transitionTime = 1f;
    public float HideTimer;
    private float _hideTimer;
    public float MovementSpeed;

    [Header("Random Properties")]
    [Tooltip("The value given will range from negative to positive")]
    public float AxisRandom;
    public float TransitionTimeOffset;
    private float _transitionTimeOffset;

    private float ActualTransitionTime
    { get { return TransitionTime + _transitionTimeOffset; } }

    /// <summary>
    /// The index position of this UIDamageFont
    /// </summary>
    public int Index { get; set; }

    public void Update()
    {
        if (_transitionTime != 1f) // Condition for moving the damage font effect
        {
            _transitionTime = (_transitionTime + (ActualTransitionTime * Time.deltaTime)) 
                              >= 1f ? 
                              1f :
                              _transitionTime + (ActualTransitionTime * Time.deltaTime);

            transform.Translate(0, MovementSpeed * _transitionTime, 0); // Moving the damage
                                                                        // font effect
        }
        else // Condition for staying still
        {
            if(_hideTimer != HideTimer) // Condition for counting down to hide
            {
                _hideTimer = (_hideTimer + Time.deltaTime) >= HideTimer ? 
                             HideTimer : 
                             (_hideTimer + Time.deltaTime);

                // Condition for hiding the damage font
                if(_hideTimer == HideTimer)
                    UIDamageFontManager.Instance
                    .RequestReleaseDamageFont(Index);
            }
        }
    }

    /// <summary>
    /// This method starts and sets the damage font effect.
    /// </summary>
    /// <param name="position">The position of the damage effect, of type Vector3</param>
    /// <param name="damageValue">The damage value to set for the effect,
    ///                           of type float</param>
    public void StartEffect(Vector3 position, int damageValue)
    {
        position.x = position.x + Random.Range(-AxisRandom, AxisRandom);
        position.z = position.z + Random.Range(-AxisRandom, AxisRandom);

        transform.position = position; // Setting the position

        // Setting the damage value
        DamageValue.text = damageValue < 10 ? "00" + damageValue.ToString() :
                           damageValue < 100 ? "0" + damageValue.ToString() :
                           damageValue.ToString();

        _transitionTime = 0; // Resetting the transition
        _hideTimer = 0; // Resetting the hide timer

        // Getting the random value for the transition timer
        _transitionTimeOffset = Random.Range(0, TransitionTimeOffset);
    }
}
