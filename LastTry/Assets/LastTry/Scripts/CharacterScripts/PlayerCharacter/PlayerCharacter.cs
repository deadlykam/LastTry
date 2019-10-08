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

    [Tooltip("This timer is for joypad use not the virtual button press.")]
    public float PickUpTimer;
    private float _pickUpTimer = 0;

    [Header("Weapon Slot Locations")]
    public Transform RightHand;

    private Vector3 _movement = new Vector3(0, 0, 0.1f);
    //private Vector3 _dir = new Vector3(0, 0, 0.1f);
    private Quaternion _dir = Quaternion.identity;

    private float _horizontalValue = 0; // Storing the joystick horizontal value
    private float _horzontalVelocity;

    private float _verticalValue = 0; // Storing the joystick vertical value
    private float _verticalVelocity;

    /// <summary>
    /// The weapon being currently hovered over.
    /// </summary>
    private WeaponItem _hoverWeapon;

    public bool IsHoverWeapon { get { return _hoverWeapon != null; } }

    private bool _isAttackButtonA = false;

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

        if (!IsDead) // Condition for not being dead
        {
            // Calling the Update of PlayerCombatControl
            UpdatePlayerCombatControl();

            // Condition for being able to move
            if (IsMovable) MovementRotationHandler();

            AttackHandler();
            PickUpItemFromJoypad();
        }
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
    /// Picking up item from joypad.
    /// </summary>
    private void PickUpItemFromJoypad()
    {
        if (Input.GetButton("Fire1") && IsJoyPad)
        {
            if (_hoverWeapon != null) PickUpItemTimer();
        }
        else _pickUpTimer = 0; // This condition may be required to be called
                               // differently since joypad will not be used
                               // in android
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
    /// This method picks up another weapon for index 0th and plays visual effects.
    /// </summary>
    /// <param name="weaponItem">The weapon to pick up, of type WeaponItem</param>
    protected override void PickUpWeapon1(WeaponItem weaponItem)
    {
        base.PickUpWeapon1(weaponItem);
        weaponItem.SetParentToPlayer(RightHand);
    }

    /// <summary>
    /// This method takes the button A attack command from the on screen button A.
    /// </summary>
    public void ButtonA() { if (IsAcceptInput) _isAttackButtonA = true; }

    /// <summary>
    /// This method adds an hovered over weapon item.
    /// </summary>
    /// <param name="weaponItem">The weapon that is being hovered over by the
    ///                          player, of type WeaponItem</param>
    public void AddHoverWeapon(WeaponItem weaponItem)
    {
        // Condition to check if there are no current hovered weapon item
        if (_hoverWeapon == null)
        {
            _hoverWeapon = weaponItem;
            
            // Showing the weapon description here
            UIInGameUIController.Instance.ShowWeaponPopup(
                GetDefaultWeapon().ItemName, _hoverWeapon.ItemName,
                GetDefaultWeapon().GetDescription(),
                _hoverWeapon.GetDescription());
        }
        else if (_hoverWeapon != weaponItem) // Checking if not same weapon
        {
            // Condition to check if the new hover weapon item is closer
            // thus making it the current hovered over weapon item
            if (Vector3.Distance(transform.position, weaponItem.transform.position) <
                Vector3.Distance(transform.position, _hoverWeapon.transform.position))
            {
                _hoverWeapon = weaponItem;

                // Showing the weapon description here
                UIInGameUIController.Instance.ShowWeaponPopup(
                    GetDefaultWeapon().ItemName, _hoverWeapon.ItemName,
                    GetDefaultWeapon().GetDescription(),
                    _hoverWeapon.GetDescription());
            }
        }

        //Note: Don't show weapon description here because the detector is using
        //      OnTriggerStay so then the description menu will be called every
        //      detection frame which is not good for UI.
    }

    /// <summary>
    /// This method removes the weapon from being picked up.
    /// </summary>
    /// <param name="weaponItem">The weapon needed to check if to remove
    ///                          the hover weapon, of type WeaponItem</param>
    public void RemoveHoverWeapon(WeaponItem weaponItem)
    {
        // Removing the hover weapon from the pick up slot
        //if (_hoverWeapon != null) _hoverWeapon = null;

        // Condition for removing the selected hover weapon
        if (_hoverWeapon == weaponItem)
        {
            _hoverWeapon = null;
            UIInGameUIController.Instance.HideWeaponPopup();
        }

        //Todo: Call the hide UI from here for hiding the weapon description
    }

    /// <summary>
    /// This method is for picking up items with timer.
    /// </summary>
    public void PickUpItemTimer()
    {
        if (IsHoverWeapon) // Condition to check if the player is hovering over a weapon
        {
            // Counting how long the button has been pressed
            _pickUpTimer = (_pickUpTimer + Time.deltaTime) >= PickUpTimer ?
                                             PickUpTimer :
                                            _pickUpTimer + Time.deltaTime;

            // Condition for picking up the weapon
            if (_pickUpTimer == PickUpTimer)
            {
                // Picking up the weapon
                PickUpWeapon1(_hoverWeapon);
                _hoverWeapon = null; // Clearing the hover select weapon

                ResetPickupTimer();
                UIInGameUIController.Instance.SetWeaponBar(0);
                UIInGameUIController.Instance.HideWeaponPopup();
            }

            UIInGameUIController.Instance.SetWeaponBar(_pickUpTimer / PickUpTimer);
        }
    }

    /// <summary>
    /// This method picks up the item instantly.
    /// </summary>
    public void PickUpItemInstant()
    {
        if (IsHoverWeapon) // Condition to check if the player is hovering over a weapon
        {
            // Picking up the weapon
            PickUpWeapon1(_hoverWeapon);
            _hoverWeapon = null; // Clearing the hover select weapon

            ResetPickupTimer();
            UIInGameUIController.Instance.HideWeaponPopup();
        }
    }

    /// <summary>
    /// This method resets the pickup timer.
    /// </summary>
    public void ResetPickupTimer() { _pickUpTimer = 0; }
}