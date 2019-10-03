using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : BasicAnimation
{
    [Header("Enemy Character Properties")]
    public EnemyManager Manager;
    public float FollowRadius;
    public float StopRadius;
    [Tooltip("The rate at which the enemy will come to a halt or run")]
    public float Acceleration;

    private float _speedPercentage = 0;
    private Vector3 _lookAtTarget;

    protected bool IsPlayerInWeaponRange = false;
    protected CombatAnimation CurrentCombatInfo;
    protected float AttackTimer = -1; // Starting must be -1 because it means animation done
                                      // or another attack can take place.
    protected bool IsAttacking { get { return AttackTimer != -1; } }

    /// <summary>
    /// Checking if the player is within the target.
    /// </summary>
    protected bool IsInRange
    { get { return Vector3.Distance(Manager.Player.transform.position,
                                    transform.position) <= FollowRadius
                                    &&
                                    Vector3.Distance(Manager.Player.transform.position,
                                    transform.position) > StopRadius; ; } }

    /// <summary>
    /// For checking if the enemy have reached the player.
    /// </summary>
    protected bool IsReachedTarget
    { get {
            return Vector3.Distance(Manager.Player.transform.position,
                                    transform.position) <= StopRadius; } }

    // Start is called before the first frame update
    void Start()
    {
        InitializeStartUp();
    }

    // Update is called once per frame
    void Update()
    {
        // Calling the update of BasicCharacter
        UpdateBasicCharacter();

        // Calling the update of BasicAnimation
        UpdateBasicAnimation();

        UpdateEnemyCharacter();
    }

    /// <summary>
    /// This method looks at the target
    /// </summary>
    private void LookAtTarget()
    {
        _lookAtTarget = Manager.Player.transform.position - transform.position;
        _lookAtTarget.y = 0;

        // Instantly looking at the player
        /*transform.rotation = Quaternion.LookRotation(_lookAtTarget);*/

        // Slerping to look at the player
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                              Quaternion.LookRotation(_lookAtTarget),
                                              SpeedSlerp);
    }

    /// <summary>
    /// This method moves the enemy forward.
    /// </summary>
    private void MovementHandler()
    {
        if (_speedPercentage != 1) // Condition for acceleration
        {
            _speedPercentage = (_speedPercentage + Time.deltaTime * Acceleration)
                                >= 1 ?
                                1 :
                                _speedPercentage + Time.deltaTime * Acceleration;
        }

        // Moving forward
        transform.Translate(Vector3.forward * SpeedMovement 
                            * _speedPercentage * Time.deltaTime);

        SetMoveSpeed(_speedPercentage); // Setting animation speed
    }

    /// <summary>
    /// This method slows down the enemy.
    /// </summary>
    private void SlowingDownEnemy()
    {
        // Condition for deceleration
        if (_speedPercentage != 0)
        {

            _speedPercentage = (_speedPercentage - Time.deltaTime * Acceleration)
                               <= 0 ?
                               0 :
                               _speedPercentage - Time.deltaTime * Acceleration;

            SetMoveSpeed(_speedPercentage); // Setting animation speed
        }
    }

    /// <summary>
    /// This method initializes the enemy character at the start up in EnemyCharacter.
    /// </summary>
    protected override void InitializeStartUp()
    {
        base.InitializeStartUp();
    }

    /// <summary>
    /// This method is the update method of EnemyCharacter.
    /// </summary>
    protected virtual void UpdateEnemyCharacter()
    {
        if (!IsDead && !IsHurt) // Condition for when the enemy is not hurt or dead
        {
            // Condition for looking at the player
            // and moving towards the player
            if (IsInRange && !IsAttacking)
            {
                LookAtTarget(); // Looking at the player
                MovementHandler(); // Moving towards the player
            }
            else // Condition for not moving
            {
                // Condition for deceleration
                SlowingDownEnemy();

                // Condition for looking at the player
                // if not in weapon range
                if(!IsAttacking && IsReachedTarget && !IsPlayerInWeaponRange)
                {
                    LookAtTarget();
                }
            }
        }
        else SlowingDownEnemy(); // Slowing down enemy for getting hurt
    }

    /// <summary>
    /// This method gets a new attack information from the BasicAnimation class.
    /// </summary>
    protected virtual void GetNewAttackAnimation()
    {
        CurrentCombatInfo = GetAttackAnimation();
        AttackTimer = 0; // Resetting attack timer for further use
    }

    /// <summary>
    /// This method makes the enemy take damage and resets any attack timer.
    /// </summary>
    /// <param name="amount">The amount of damage to be taken, of type int</param>
    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);

        AttackTimer = -1;
    }

    /// <summary>
    /// This method sets if the player is in weapon range.
    /// </summary>
    /// <param name="isInRange">The flag which sets that the player is
    ///                         in weapon range, of type bool</param>
    public void SetPlayerInWeaponRange(bool isInRange)
    {
        IsPlayerInWeaponRange = isInRange;
    }
}
