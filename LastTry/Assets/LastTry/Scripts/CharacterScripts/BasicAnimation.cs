using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles all the animations of a character.
/// </summary>
public class BasicAnimation : BasicCharacter
{
    #region Animator Attributes
    private readonly string AnimationMovePercentage = "MovePercentage";

    private readonly string AnimationIsAttackIdle = "IsAttackIdle";
    private readonly string AnimationIsDash = "IsDash";

    private readonly string AnimationTriggerAttackSword = "Attack";
    private readonly string AnimationTriggerHurt = "Hurt";
    private readonly string AnimationTriggerDeath = "Death";
    private readonly string AnimationTriggerDash = "Dash";
    #endregion Animator Attributes

    [Header("Basic Animation Properties")]
    public Animator CharacterAnimator;
    public AnimationClip AttackClip; // This clip is used to play other animations
                                     // from script
    public CombatAnimation[] DefaultAnimationAttacks;
    //public CombatAnimation[] SwordAnimationAttacks;
    private CombatAnimation[] _currentAnimationAttacks;
    private AnimatorOverrideController _overrideController;

    public float CombatTimer;
    private float _combatTimerCurrent;

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

        // Making the combat stance false at the start
        CharacterAnimator.SetBool(AnimationIsAttackIdle, false);
    }

    /// <summary>
    /// This method picks up another weapon for index 0th and updates the
    /// attack animations.
    /// </summary>
    /// <param name="weaponItem">The weapon to pick up and to replace the
    ///                          attack animation/s from, of type WeaponItem</param>
    protected override void PickUpWeapon1(WeaponItem weaponItem)
    {
        base.PickUpWeapon1(weaponItem);

        // Replacing attack animations with picked up weapon's
        // attack animations
        _currentAnimationAttacks = weaponItem.AttackAnimations;
    }

    /// <summary>
    /// This method is the update method of BasicAnimation.
    /// </summary>
    protected void UpdateBasicAnimation()
    {
        // Condition for handling attack idle mode
        if (CharacterAnimator.GetBool(AnimationIsAttackIdle))
        {
            // Counting down the attack idle mode
            _combatTimerCurrent -= Time.deltaTime;

            // Condition for stopping the idle mode
            if(_combatTimerCurrent <= 0)
            {
                // Stopping the attack idle animation
                CharacterAnimator.SetBool(AnimationIsAttackIdle, false);
                _combatTimerCurrent = 0; // Making timer to 0 to clearify
                                         // that attack idle mode is done
            }
        }
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
    /// This method plays the random attack animation of the weapon and resets the
    /// combat mode timer and enables the combat stance.
    /// </summary>
    /// <param name="animation">The animatino to play, of type AnimationClip</param>
    protected void PlayAttackAnimation(AnimationClip animation)
    {
        CharacterAnimator.SetTrigger(AnimationTriggerAttackSword);
        _overrideController[AttackClip.name] = animation;

        _combatTimerCurrent = CombatTimer; // Resetting the combat timer

        // Condition for starting the combat mode
        if (!CharacterAnimator.GetBool(AnimationIsAttackIdle))
        {
            CharacterAnimator.SetBool(AnimationIsAttackIdle, true);
        }
    }

    /// <summary>
    /// This method plays the dash animation.
    /// </summary>
    protected void PlayDashAnimation()
    {
        CharacterAnimator.SetBool(AnimationIsDash, true);
        CharacterAnimator.SetTrigger(AnimationTriggerDash);
        _combatTimerCurrent = 0; // Stopping any combat stance
    }

    /// <summary>
    /// This method stops the dashing animation.
    /// </summary>
    protected void StopDashing() { CharacterAnimator.SetBool(AnimationIsDash, false); }

    /// <summary>
    /// This method hurts the character and plays the hurt animation.
    /// </summary>
    /// <param name="amount">The amount of damage to take, of type int</param>
    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);

        // Condition for triggering hurt animation
        if (!IsDead) CharacterAnimator.SetTrigger(AnimationTriggerHurt);
        // Condition for trigerring death animation
        else CharacterAnimator.SetTrigger(AnimationTriggerDeath);
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