using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : BasicAnimation
{
    [Header("Player Character Properties")]
    public Joystick JoystickAndroid;
    public Transform PlayerModel;

    private Vector3 _movement;

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

        // Rotating the player towards the movement direction
        PlayerModel.rotation = Quaternion.LookRotation(_movement);
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
