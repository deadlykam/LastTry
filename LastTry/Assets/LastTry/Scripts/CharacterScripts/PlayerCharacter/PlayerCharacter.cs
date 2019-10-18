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

    [Header("Dash Properties")]
    public float DashSpeed;
    public float DashTimer;
    private float _dashTimer = 0;
    private bool _IsDash { get { return _dashTimer != 0; } }
    public float DashReloadTimer;
    private float _dashReloadTimer;
    private bool _isDashReloaded { get { return _dashReloadTimer == 0; } }

    [Header("Weapon Slot Locations")]
    public Transform RightHand;

    private Vector3 _movement = new Vector3(0, 0, 0.1f);
    private Quaternion _dir = Quaternion.identity;

    private float _horizontalValue = 0; // Storing the joystick horizontal value
    private float _horzontalVelocity;

    private float _verticalValue = 0; // Storing the joystick vertical value
    private float _verticalVelocity;

    /// <summary>
    /// The item being currently hovered over.
    /// </summary>
    private Items _hoverItem;
    public Items HoverItem { get { return _hoverItem; } }

    public bool IsHoverItem { get { return _hoverItem != null; } }

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
            if (IsMovable && !_IsDash) MovementRotationHandler();

            if(!_IsDash) AttackHandler();
            DashHandler();
            PickUpItemFromJoypad();
        }

        // Setting the player health
        UIInGameUIController.Instance.SetPlayerHealthBar(GetHealthPercentage());
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
    /// This method handles all the dash related events.
    /// </summary>
    private void DashHandler()
    {
        // Condition for dashing
        if (!_IsDash && Input.GetButtonDown("Fire2")
            && _isDashReloaded)
        {
            _dashTimer = DashTimer;
            PlayDashAnimation();
            SetCollisions(false);

            // Resetting the speed values because the player
            // is attacking
            ResetMovementRotation();

            RemoveAllEnemies(); // Removing enemies in the list
            IsStopDamage = true; // Stopping any damage given to the enemies
            _dashReloadTimer = DashReloadTimer; // Reloading timer for the next dash
        }

        if (_IsDash) // Condition for dashing
        {
            _dashTimer = (_dashTimer - Time.deltaTime) <= 0 ? 
                         0 : _dashTimer - Time.deltaTime;

            // Player dashing forward
            transform.Translate(PlayerModel.transform.forward * DashSpeed * Time.deltaTime);

            if (!_IsDash) // Condition for stopping dash
            {
                StopDashing(); // Stopping dashing animation
                SetCollisions(true);

                IsStopDamage = false; // Starting any damage given to the enemies
            }
        }
        else if (!_isDashReloaded) // Condition for resetting the dash
        {
            _dashReloadTimer = (_dashReloadTimer - Time.deltaTime) <= 0 ?
                               0 :
                               (_dashReloadTimer - Time.deltaTime);
        }
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
    /// This method enables/disables collision between the player and the enemy.
    /// </summary>
    /// <param name="isCollision">The flat to set the collision, true means
    ///                           there should be collision, false otherwise,
    ///                           of type bool</param>
    private void SetCollisions(bool isCollision)
    {
        if (isCollision) // Condition for enabling collision
        {
            Physics.IgnoreLayerCollision(8, 10, false); // Enabling enemy collisions
            Physics.IgnoreLayerCollision(8, 11, false); // Enabling enemy collisions
            Physics.IgnoreLayerCollision(9, 10, false); // Enabling enemy detection
        }
        else // Condition for disabling collision
        {
            Physics.IgnoreLayerCollision(8, 10); // Disabling enemy collisions
            Physics.IgnoreLayerCollision(8, 11); // Disabling enemy collisions
            Physics.IgnoreLayerCollision(9, 10); // Disabling enemy detection
        }
    }

    /// <summary>
    /// Picking up item from joypad.
    /// </summary>
    private void PickUpItemFromJoypad()
    {
        if (Input.GetButton("Fire1") && IsJoyPad)
        {
            if (_hoverItem != null) PickUpItemTimer();
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
        SetMoveSpeed(0);
    }

    /// <summary>
    /// This method shows/hides the item description.
    /// </summary>
    /// <param name="isShow">The flag to show or hide item description,
    ///                      of type bool</param>
    private void SetItemDescription(bool isShow)
    {
        if (isShow) // Condition to show item description
        {
            // Checking if the item is a WeaponItem
            if (_hoverItem as WeaponItem != null)
            {
                // Showing the weapon description here
                UIInGameUIController.Instance.ShowWeaponPopup(
                    GetDefaultWeapon().ItemName, ((WeaponItem)_hoverItem).ItemName,
                    GetDefaultWeapon().GetDescription(),
                    ((WeaponItem)_hoverItem).GetDescription());
            }
        }
        else UIInGameUIController.Instance.HideAllPopUp(); // Condition for hiding all
                                                           // item descriptions
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
    /// This method hurts the player.
    /// </summary>
    /// <param name="amount">The amount of damage to take, of type int</param>
    public override void TakeDamage(int amount)
    {
        // Conditions to take damage
        if(!_IsDash) base.TakeDamage(amount);
    }

    /// <summary>
    /// This method takes the button A attack command from the on screen button A.
    /// </summary>
    public void ButtonA() { if (IsAcceptInput) _isAttackButtonA = true; }

    /// <summary>
    /// This method adds a hovered over item.
    /// </summary>
    /// <param name="hoverItem">The item that is being hovered over by the
    ///                         player, of type Items</param>
    public void AddHoverItem(Items hoverItem)
    {
        // Condition to check if there are no current hovered item
        if(_hoverItem == null)
        {
            _hoverItem = hoverItem;

            SetItemDescription(true); // Showing item description
        }
        else if(_hoverItem != hoverItem) // Checking if not same item
        {
            if(Vector3.Distance(transform.position, hoverItem.transform.position) <
               Vector3.Distance(transform.position, _hoverItem.transform.position))
            {
                _hoverItem = hoverItem;

                SetItemDescription(true); // Showing item description
            }
        }

        //Note: Don't show weapon description here because the detector is using
        //      OnTriggerStay so then the description menu will be called every
        //      detection frame which is not good for UI.
    }

    /// <summary>
    /// This method removes the item from being picked up.
    /// </summary>
    /// <param name="hoverItem">The item needed to check if to remove
    ///                         the hover item, of type Items</param>
    public void RemoveHoverItem(Items hoverItem)
    {
        // Condition for removing the selected hover item
        if(_hoverItem == hoverItem)
        {
            _hoverItem = null;
            SetItemDescription(false);
        }
    }

    /// <summary>
    /// This method is for picking up items with timer.
    /// </summary>
    public void PickUpItemTimer()
    {
        if (IsHoverItem) // Condition to check if the player is hovering over a weapon
        {
            // Counting how long the button has been pressed
            _pickUpTimer = (_pickUpTimer + Time.deltaTime) >= PickUpTimer ?
                                             PickUpTimer :
                                            _pickUpTimer + Time.deltaTime;

            // Condition for picking up the item
            if(_pickUpTimer == PickUpTimer)
            {
                // Condition for picking up the weapon
                if (_hoverItem as WeaponItem) PickUpWeapon1(((WeaponItem)_hoverItem));

                _hoverItem = null; // Clearing the hover selected item
                ResetPickupTimer();

                UIInGameUIController.Instance.ResetAllBar();
                SetItemDescription(false);
                return; // No further logic required
            }

            UIInGameUIController.Instance.SetAllBar(_hoverItem, _pickUpTimer / PickUpTimer);
        }
    }

    /// <summary>
    /// This method picks up the item instantly.
    /// </summary>
    public void PickUpItemInstant()
    {
        if (IsHoverItem) // Condition to check if the player is hovering over a weapon
        {
            // Condition for picking up the weapon
            if (_hoverItem as WeaponItem) PickUpWeapon1(((WeaponItem)_hoverItem));

            _hoverItem = null; // Clearing the hover selected item

            ResetPickupTimer();
            SetItemDescription(false);
        }
    }

    /// <summary>
    /// This method resets the pickup timer.
    /// </summary>
    public void ResetPickupTimer() { _pickUpTimer = 0; }
}