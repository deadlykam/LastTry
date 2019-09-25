using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles all the animations of a character.
/// </summary>
public class BasicAnimation : BasicCharacter
{
    private readonly string AnimationMovePercentage = "MovePercentage";

    [Header("Basic Animation Properties")]
    public Animator CharacterAnimator;

    // Start is called before the first frame update
    void Start()
    {
        InitializeStartUp();
    }

    /// <summary>
    /// This method initializes all basic attribute and basic animations at the start up.
    /// </summary>
    protected override void InitializeStartUp()
    {
        base.InitializeStartUp();
    }

    /// <summary>
    /// This method sets the move percentage of the move animation.
    /// </summary>
    /// <param name="percentage">The move percentage value between 0.0 - 1.0f,
    ///                          of type float</param>
    protected void SetMoveSpeed(float percentage)
    {
        // Converting percentage value to absolute value
        percentage = percentage < 0 ? percentage * -1 : percentage;
        CharacterAnimator.SetFloat(AnimationMovePercentage, percentage);
    }

    /// <summary>
    /// This method sets the highest value from two values as the
    /// the move percentage of the move animation.
    /// </summary>
    /// <param name="value1">First value to compare, of type float</param>
    /// <param name="value2">Second value to compare, of type float</param>
    protected void SetMoveSpeed(float value1, float value2)
    {
        // Converting value1 value to absolute value
        value1 = value1 < 0 ? value1 * -1 : value1;
        // Converting percentage value to absolute value
        value2 = value2 < 0 ? value2 * -1 : value2;

        // When player not moving that is both value1 and value2
        // are zero which is the idle animation
        if (value1 == 0 && value2 == 0) SetMoveSpeed(0);

        // Taking the value1 value to apply as percentage
        else if (value1 > value2) SetMoveSpeed(value1);

        // Taking the value2 value to apply as percentage
        else SetMoveSpeed(value2); // Vertical value
    }
}
