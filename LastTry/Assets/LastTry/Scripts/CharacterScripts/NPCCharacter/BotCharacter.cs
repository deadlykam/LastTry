using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCharacter : BasicCharacter
{
    [Header("Bot Character Properties")]
    public Transform Player;
    public float FollowRadius;
    public float StopRadius;
    public float Acceleration;

    [SerializeField]
    private CharacterState BotState = CharacterState.Stop;
    private float _speedPercentage = 0;
    private Vector3 _lookAtTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBasicCharacter();
        UpdateBotCharacter();
    }

    /// <summary>
    /// This method moves the enemy forward.
    /// </summary>
    /// <param name="dir">For indication acceleration or deceleration,
    ///                   1 means acceleration, -1 means deceleration,
    ///                   of type int</param>
    private void MovementHandler(int dir)
    {
        if (dir == 1) // Condition for speeding up
        {
            if (_speedPercentage != 1) // Condition for acceleration
            {
                _speedPercentage = (_speedPercentage + Time.deltaTime * Acceleration)
                                    >= 1 ?
                                    1 :
                                    _speedPercentage + Time.deltaTime * Acceleration;
            }
        }
        else if(dir == -1) // Condition for speeding down
        {
            // Condition for deceleration
            if (_speedPercentage != 0)
            {

                _speedPercentage = (_speedPercentage - Time.deltaTime * Acceleration)
                                   <= 0 ?
                                   0 :
                                   _speedPercentage - Time.deltaTime * Acceleration;
            }
        }

        // Moving forward
        transform.Translate(Vector3.forward * SpeedMovement
                            * _speedPercentage * Time.deltaTime);
    }

    /// <summary>
    /// This method makes the bot look at a target.
    /// </summary>
    /// <param name="target">The target to look at, of type Transform</param>
    private void LookAtTarget(Transform target)
    {
        _lookAtTarget = target.position - transform.position;
        _lookAtTarget.y = 0;

        // Slerping to look at the target
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                              Quaternion.LookRotation(_lookAtTarget),
                                              SpeedSlerp);
    }

    /// <summary>
    /// This method checks if the target is out of range.
    /// </summary>
    /// <param name="target">The target to check against if out of range, of type
    ///                      Transform</param>
    /// <returns>True means out of range, false otherwise, of type bool</returns>
    private bool IsOutOfRange(Transform target)
    {
        return Vector3.Distance(target.position, transform.position) > FollowRadius;
    }

    /// <summary>
    /// This method checks if the target is with in range.
    /// </summary>
    /// <param name="target">The target to check against if in range, of type
    ///                      Transform</param>
    /// <returns>True means with in range, false otherwise, of type bool</returns>
    private bool IsInRange(Transform target)
    {
        return Vector3.Distance(target.position, transform.position) <= StopRadius;
    }

    /// <summary>
    /// The update method of BotCharacter.
    /// </summary>
    protected void UpdateBotCharacter()
    {
        // Bot stopping state
        if(BotState == CharacterState.Stop)
        {
            // Condition for moving the bot
            if (IsOutOfRange(Player))
            {
                BotState = CharacterState.Move;
                return; // No further logic required
            }

            MovementHandler(-1); // Slowing down and stopping the bot
            LookAtTarget(Player); // Looking at the player
        }
        // Bot moving state
        else if(BotState == CharacterState.Move)
        {
            // Condition for stoping the bot
            if (IsInRange(Player))
            {
                BotState = CharacterState.Stop;
                return; // No further logic required
            }

            MovementHandler(1); // Speeding up the bot
            LookAtTarget(Player); // Looking at the player
        }
    }
}
