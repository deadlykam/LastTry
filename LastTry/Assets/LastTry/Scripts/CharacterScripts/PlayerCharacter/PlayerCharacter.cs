using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : BasicAnimation
{
    [Header("Player Character Properties")]
    public Joystick JoystickAndroid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovementHandler();
    }

    /// <summary>
    /// This method moves the player.
    /// </summary>
    private void MovementHandler()
    {
        transform.position += new Vector3(JoystickAndroid.Horizontal * SpeedMovement * Time.deltaTime,
                                          0,
                                          JoystickAndroid.Vertical * SpeedMovement * Time.deltaTime);
    }

    /// <summary>
    /// This method initializes the player character at the start up.
    /// </summary>
    protected override void InitializeStartUp()
    {
        base.InitializeStartUp();
    }
}
