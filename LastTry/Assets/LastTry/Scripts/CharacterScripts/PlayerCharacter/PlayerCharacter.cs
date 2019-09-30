using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : PlayerCombatControl
{
    [Header("Player Character Properties")]
    public Joystick JoystickAndroid;
    public Transform PlayerModel;
    [Tooltip("True for using the joypad in editor. Make false while build " +
        "otherwise touching screen will act as triggering attack.")]
    public bool IsJoyPad;
    
    public float MovementAcceleration;

    private Vector3 _movement = new Vector3(0, 0, 0.1f);
    //private Vector3 _dir = new Vector3(0, 0, 0.1f);
    private Quaternion _dir = Quaternion.identity;

    private float _horizontalValue = 0; // Storing the joystick horizontal value
    private float _horzontalVelocity;

    private float _verticalValue = 0; // Storing the joystick vertical value
    private float _verticalVelocity;

    private bool _isAttackButtonA = false;

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

        // Calling the Update of PlayerCombatControl
        UpdatePlayerCombatControl();
        
        // Condition for being able to move
        if (IsMovable) MovementRotationHandler();

        AttackHandler();
    }

    /// <summary>
    /// This method moves the player through thumbstick, joypad and keyboard.
    /// </summary>
    private void MovementRotationHandler()
    {
        // Accelerating horizontal movement from joystick/keyboard/joypad
        _horizontalValue = Mathf.SmoothDamp(_horizontalValue,
                                            JoystickAndroid.Horizontal
                                            + Input.GetAxis("Horizontal"),
                                            ref _horzontalVelocity,
                                            MovementAcceleration);

        // Accelerating vertical movement from joystick/keyboard/joypad
        _verticalValue = Mathf.SmoothDamp(_verticalValue,
                                          JoystickAndroid.Vertical
                                          + Input.GetAxis("Vertical"),
                                          ref _verticalVelocity,
                                          MovementAcceleration);

        // Getting the movement direction
        _movement = new Vector3(
            _horizontalValue,
            0,
            _verticalValue
            );

        // Condition for updating the direction when movement is not zero,
        // this condition is needed so that the direction is not updated
        // to zero when player not moving
        if (_movement != Vector3.zero) _dir = Quaternion.LookRotation(_movement);

        // Condition for rotating the player model
        if (_dir != PlayerModel.rotation)
        {
            // Rotating the player towards the movement direction
            PlayerModel.rotation = Quaternion.Slerp(
                                       PlayerModel.rotation,
                                       _dir,
                                       SpeedSlerp);
        }

        // Moving the player
        transform.Translate(_movement * SpeedMovement * Time.deltaTime);
        // Setting the move animation speed
        SetMoveSpeed(_horizontalValue, _verticalValue);
    }

    /// <summary>
    /// This method handles the attack system of the player.
    /// </summary>
    private void AttackHandler()
    {
        // Condition for attacking
        if (IsAcceptInput)
        {
            // Condition for getting input from the user and then
            // the player attacking
            if ((Input.GetButtonDown("Fire1") && IsJoyPad) 
                || _isAttackButtonA)
            {
                // Attacking
                AddCombatInput();

                // Resetting the speed values because the player
                // is attacking
                ResetMovementRotation();
            }
        }

        // Making button A false if being used
        _isAttackButtonA = _isAttackButtonA ? false : false;
    }

    /// <summary>
    /// This method resets the movement and rotation feature.
    /// </summary>
    private void ResetMovementRotation()
    {
        _horizontalValue = 0;
        _verticalValue = 0;
        _dir = PlayerModel.rotation;
        //_movement = Vector3.zero;
        SetMoveSpeed(0);
    }

    /// <summary>
    /// This method initializes the player character at the start up in PlayerCharacter.
    /// </summary>
    protected override void InitializeStartUp()
    {
        base.InitializeStartUp();
    }

    /// <summary>
    /// This method takes the button A attack command from the on screen button A.
    /// </summary>
    public void ButtonA() { if (IsAcceptInput) _isAttackButtonA = true; }
}
