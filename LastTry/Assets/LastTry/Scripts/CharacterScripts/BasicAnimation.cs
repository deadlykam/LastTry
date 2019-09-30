using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles all the animations of a character.
/// </summary>
public class BasicAnimation : BasicCharacter
{
    private readonly string AnimationMovePercentage = "MovePercentage";
    protected readonly string AnimationTriggerAttackSword = "Attack";

    [Header("Basic Animation Properties")]
    public Animator CharacterAnimator;
    public AnimationClip AttackClip; // This clip is used to play other animations
                                     // from script
    public CombatAnimation[] DefaultAnimationAttacks;
    public CombatAnimation[] SwordAnimationAttacks;
    private CombatAnimation[] _currentAnimationAttacks;
    private AnimatorOverrideController _overrideController;

    // Start is called before the first frame update
    void Start()
    {
        InitializeStartUp();
    }

    /// <summary>
    /// This method triggers the sword attack animation.
    /// </summary>
    private void SwordAttack()
    { CharacterAnimator.SetTrigger(AnimationTriggerAttackSword); }

    /// <summary>
    /// This method initializes all basic attribute and basic animations at the start up
    /// in BasicAnimation.
    /// </summary>
    protected override void InitializeStartUp()
    {
        base.InitializeStartUp();

        // Setting up the override controller
        _overrideController = 
            new AnimatorOverrideController(CharacterAnimator.runtimeAnimatorController);
        CharacterAnimator.runtimeAnimatorController = _overrideController;

        // Setting the attack animations to default at start
        // NOTE: This may change in future and will depend on the weapon equipped
        _currentAnimationAttacks = DefaultAnimationAttacks;
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

    /// <summary>
    /// This method returns the information of the animation.
    /// </summary>
    /// <returns>The animation information of the current attack, of
    ///          type CombatAnimation</returns>
    protected CombatAnimation GetAttackAnimation()
    {
        return _currentAnimationAttacks[Random.Range(0, _currentAnimationAttacks.Length)];
    }

    /// <summary>
    /// This method plays the random attack animation of the weapon.
    /// </summary>
    /// <param name="animation">The animatino to play, of type AnimationClip</param>
    protected void PlayAttackAnimation(AnimationClip animation)
    {
        CharacterAnimator.SetTrigger(AnimationTriggerAttackSword);
        _overrideController[AttackClip.name] = animation;
    }

    /// <summary>
    /// [Depricated]This method plays attack animation of the character.
    /// </summary>
    /// <param name="weapon">The type of weapon animation to play,
    ///                      of type WeaponType</param>
    protected void PlayAttackAnimation(WeaponInfo.WeaponType weapon)
    {
        // Animation for sword attacks
        if (weapon == WeaponInfo.WeaponType.Sword)
            SwordAttack();
    }
}

[System.Serializable]
/// <summary>
/// This class stores the time information of a combat animation.
/// </summary>
public struct CombatAnimation
{
    /// <summary>
    /// Total animation time.
    /// </summary>
    public float TotalTime;

    /// <summary>
    /// The time at which no input will be accepted.
    /// </summary>
    public float ProcessTime;

    public AnimationClip AttackAnimation;
}