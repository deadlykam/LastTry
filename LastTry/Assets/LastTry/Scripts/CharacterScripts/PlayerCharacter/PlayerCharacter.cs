using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : BasicAnimation
{
    [Header("Player Character Properties")]
    public Joystick JoystickAndroid;
    public Transform PlayerModel;

    [Range(0.0f, 1.0f)]
    public float SpeedSlerp;

    public float MovementAcceleration;

    private Vector3 _movement = new Vector3(0, 0, 0.1f);
    private Vector3 _dir = new Vector3(0, 0, 0.1f);

    private float _horizontalValue = 0; // Storing the joystick horizontal value
    private float _horzontalVelocity;

    private float _verticalValue = 0; // Storing the joystick vertical value
    private float _verticalVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovementRotationHandler();
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
        if (_movement != Vector3.zero) _dir = _movement;

        // Condition for rotating the player model
        if (Quaternion.LookRotation(_dir) != PlayerModel.rotation)
        {
            // Rotating the player towards the movement direction
            PlayerModel.rotation = Quaternion.Slerp(PlayerModel.rotation,
                                   Quaternion.LookRotation(_dir),
                                   SpeedSlerp);
        }

        // Moving the player
        transform.Translate(_movement * SpeedMovement * Time.deltaTime);
        // Setting the move animation speed
        SetMoveSpeed(_horizontalValue, _verticalValue);
    }

    /// <summary>
    /// This method initializes the player character at the start up.
    /// </summary>
    protected override void InitializeStartUp()
    {
        base.InitializeStartUp();
    }
}
