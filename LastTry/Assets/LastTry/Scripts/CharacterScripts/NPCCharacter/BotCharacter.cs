using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCharacter : BasicCharacter
{
    [Header("Bot Character Properties")]
    public Transform Player;
    public Transform BotModel;
    public float FollowRadius;
    public float StopRadius;
    public float Acceleration;

    [Header("Bot Hover Properties")]
    public float HoverHeight;
    public float HoverSpeed;
    private int _hoverDir = 1;

    [Header("Bot Enemy Targeting Properties")]
    public float EnemyRadius;
    public Transform LaserShooter;
    public LineRenderer Laser;
    public GameObject LaserVFX1;
    public GameObject LaserVFX2;
    public float AttackTimer;
    private float _attackTimer = 0;
    private List<EnemyCharacter> _enemies = new List<EnemyCharacter>();
    private EnemyCharacter _enemy { get { return _enemies[0]; } }

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
    /// This method makes the BotModel to hover.
    /// </summary>
    public void HoverHandler()
    {
        // Conditions for determinging the hover direction
        if (BotModel.localPosition.y > HoverHeight) _hoverDir = -1;
        else if (BotModel.localPosition.y <= 0) _hoverDir = 1;

        // Condition for moving the bot vertically
        BotModel.Translate(Vector3.up * HoverSpeed * Time.deltaTime * _hoverDir);
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
    /// This method attacks the enemy.
    /// </summary>
    private void AttackHandler()
    {
        // Counting up attack timer
        _attackTimer = (_attackTimer + Time.deltaTime) >= AttackTimer ? 
                       AttackTimer : _attackTimer + Time.deltaTime;

        // Condition for damaging the enemy
        if(_attackTimer == AttackTimer)
        {
            // Damaging the enemy
            _enemy.TakeDamage(GetDefaultWeapon().GetDamage());
            _attackTimer = 0; // Resetting the attack timer
        }
    }

    /// <summary>
    /// This method shoots laser from the boot
    /// </summary>
    /// <param name="isShot">The flag to enable/disable the laser,
    ///                      of type bool</param>
    private void ShootLaser(bool isShot)
    {
        if (isShot) // Shoots the laser
        {
            Laser.SetPosition(0, LaserShooter.position); // Starting the laser

            // Ending the laser
            Laser.SetPosition(1, _enemy.LaserContactOffset.transform.position);

            if (!LaserVFX1.activeSelf) // Condition to show laser vfxs
            {
                LaserVFX1.SetActive(true);
                LaserVFX2.SetActive(true);
            }

            // Setting the LaserVFX2 position to the contact point
            LaserVFX2.transform.position = _enemy.LaserContactOffset.transform.position;
        }
        else // Disables the laser
        {
            Laser.SetPosition(0, Vector3.zero); // Starting the laser
            Laser.SetPosition(1, Vector3.zero); // Ending the laser

            if (LaserVFX1.activeSelf) // Condition to show laser vfxs
            {
                LaserVFX1.SetActive(false);
                LaserVFX2.SetActive(false);
            }
        }
    }

    /// <summary>
    /// This method checks if the enemy is dead and makes the bot go back 
    /// to the player.
    /// </summary>
    private void CheckEnemy()
    {
        // Condition to move back to player if enemy is dead
        if (_enemy.IsDead)
        {
            _enemies.RemoveAt(0); // Removing the dead enemy
            ShootLaser(false); // Stopping the laser
            _attackTimer = 0; // Resetting the attack timer

            // Condition for no more enemies
            if (_enemies.Count == 0) BotState = CharacterState.Move;
            else BotState = CharacterState.GetNextEnemy; // Condition for starting
                                                         // to get the next enemy
                                                         // process
        }
    }

    /// <summary>
    /// This method finds another enemy to target from the list
    /// </summary>
    private void GetNextEnemyProcess()
    {
        if (_enemies.Count != 0) // Condition to check there are enemies to process
        {
            // Condition for moving to the next with in range enemy and not dead
            if (Vector3.Distance(transform.position, _enemy.transform.position)
                <= EnemyRadius && !_enemy.IsDead)
            {
                BotState = CharacterState.MoveToEnemy;
            }
            else _enemies.RemoveAt(0); // Removing the enemy from the list
        }
        else BotState = CharacterState.Move; // Condition to move back to the player
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

            LookAtTarget(Player); // Looking at the player
            MovementHandler(-1); // Slowing down and stopping the bot
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

            LookAtTarget(Player); // Looking at the player
            MovementHandler(1); // Speeding up the bot
        }
        // Bot moving to enemy state
        else if(BotState == CharacterState.MoveToEnemy)
        {
            // Condition for stopping the bot
            if (IsInRange(_enemy.transform))
            {
                BotState = CharacterState.AttackEnemy;
                return; // No further logic required
            }

            LookAtTarget(_enemy.transform); // Looking at the enemy
            MovementHandler(1); // Speeding up the bot
            CheckEnemy(); // Checking enemy status
        }
        // Bot stopping state and attacking the enemy
        else if(BotState == CharacterState.AttackEnemy)
        {
            // Condition for moving the bot to the enemy
            if (IsOutOfRange(_enemy.transform))
            {
                BotState = CharacterState.MoveToEnemy;
                ShootLaser(false); // Stopping the laser
                return; // No further logic required
            }

            LookAtTarget(_enemy.transform); // Looking at the enemy
            MovementHandler(-1); // Slowing down and stopping the bot
            ShootLaser(true); // Shooting the laser
            AttackHandler(); // Attacking the enemy
            CheckEnemy(); // Checking enemy status
        }
        // Bot getting next enemy from the list state
        else if(BotState == CharacterState.GetNextEnemy)
        {
            GetNextEnemyProcess();
        }

        HoverHandler(); // Hovering the bot model
    }

    /// <summary>
    /// This adds an enemy to the list.
    /// </summary>
    /// <param name="enemy">The enemy to add, of type EnemyCharacter</param>
    public void AddEnemy(EnemyCharacter enemy)
    {
        if (!enemy.IsDead) // Condition to add if enemy is not dead
        {
            _enemies.Add(enemy); // adding enemy

            // Checking if only 1 enemy is added then making the bot to
            // follow the first and only enemy
            if (_enemies.Count == 1) BotState = CharacterState.MoveToEnemy;
        }
    }

    /// <summary>
    /// This method removes an enemy from the list
    /// </summary>
    /// <param name="enemy">The enemy to remove, of type EnemyCharacter</param>
    public void RemoveEnemy(EnemyCharacter enemy)
    {
        _enemies.Remove(enemy);
    }
}
