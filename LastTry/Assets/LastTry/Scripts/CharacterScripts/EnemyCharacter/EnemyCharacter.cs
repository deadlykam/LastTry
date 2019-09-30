using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : BasicAnimation
{
    [Header("Enemy Character Properties")]
    public EnemyManager Manager;
    public float AttackRadius;
    [Tooltip("The rate at which the enemy will come to a halt or run")]
    public float Acceleration;

    private float _speedPercentage = 0;
    private Vector3 _lookAtTarget;

    protected bool IsInRange
    { get
        {
            return Vector3.Distance(Manager.Player.transform.position,
                                    transform.position) <= AttackRadius;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeStartUp();
    }

    // Update is called once per frame
    void Update()
    {
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
        if (IsInRange) // Condition for looking at the player
        {
            LookAtTarget(); // Looking at the player
            MovementHandler(); // Moving towards the player
        }
        else // Condition for not moving
        {
            // Condition for deceleration
            if (_speedPercentage != 0) {

                _speedPercentage = (_speedPercentage - Time.deltaTime * Acceleration) 
                                   <= 0 ? 
                                   0 : 
                                   _speedPercentage - Time.deltaTime * Acceleration;

                SetMoveSpeed(_speedPercentage); // Setting animation speed
            }
        }
    }
}
