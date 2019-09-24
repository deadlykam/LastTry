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

    private Vector3 _movement = new Vector3(0, 0, 0.1f);
    private Vector3 _dir = new Vector3(0, 0, 0.1f);

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
        // Getting the movement direction
        _movement = new Vector3(
            (JoystickAndroid.Horizontal + Input.GetAxis("Horizontal")),

            0,

            (JoystickAndroid.Vertical + Input.GetAxis("Vertical"))
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
    }

    /// <summary>
    /// This method initializes the player character at the start up.
    /// </summary>
    protected override void InitializeStartUp()
    {
        base.InitializeStartUp();
    }
}
