using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : PlayerCoinControl
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
    private bool _isDashButton; // This get flag info from the virtual button

    [Header("Weapon Slot Locations")]
    public Transform RightHand;

    [Header("Wearable Slot Locations")]
    public SkinnedMeshRenderer SkinnedMesh;

    /// <summary>
    /// index 0 = body
    /// index 1 = hand
    /// index 2 = head
    /// index 3 = legs
    /// index 4 = shoes
    /// </summary>
    private WearableItem[] _wearableItems;
    public int WearableItemsLength { get { return _wearableItems.Length; } }

    private int _statDefense = 0;

    private Vector3 _movement = new Vector3(0, 0, 0.1f);
    private Quaternion _dir = Quaternion.identity;

    private float _horizontalValue = 0; // Storing the joystick horizontal value
    private float _horzontalVelocity;

    private float _verticalValue = 0; // Storing the joystick vertical value
    private float _verticalVelocity;

    /// <summary>
    /// The item being currently hovered over.
    /// </summary>
    private Interactive _hoverObject;
    public Interactive HoverObject { get { return _hoverObject; } }

    public bool IsHoverObject { get { return _hoverObject != null; } }

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

        // Calling the update of PlayerCoinControl
        UpdatePlayerCoinControl();

        if (!IsDead) // Condition for not being dead
        {
            // Calling the Update of PlayerCombatControl
            UpdatePlayerCombatControl();

            // Condition for being able to move
            if (IsMovable && !_IsDash)
                MovementRotationHandler();

            if(!_IsDash) AttackHandler();
            DashHandler();
            PickUpObjectFromJoypad();
        }

        // Setting the player health
        UIInGameUIController.Instance
            .SetPlayerHealthBar(GetHealthPercentage(), Health, GetTotalHealth());
    }

    /// <summary>
    /// This method moves the player through thumbstick, joypad and keyboard.
    /// </summary>
    private void MovementRotationHandler()
    {
        // Accelerating horizontal movement from joystick/keyboard/joypad
        _horizontalValue = Mathf.SmoothDamp(_horizontalValue,

                                            IsMenusClosed() ?
                                            JoystickAndroid.Horizontal
                                            + Input.GetAxis("Horizontal")
                                            : 0,

                                            ref _horzontalVelocity,
                                            MovementAcceleration);

        // Accelerating vertical movement from joystick/keyboard/joypad
        _verticalValue = Mathf.SmoothDamp(_verticalValue,

                                          IsMenusClosed() ?
                                          JoystickAndroid.Vertical
                                          + Input.GetAxis("Vertical")
                                          : 0,

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
        if (!_IsDash && 
            ((Input.GetButtonDown("Fire2") && IsJoyPad) || _isDashButton)
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

        // Condition for resetting the dash button
        if (_isDashButton) _isDashButton = false;
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
    /// Picking up object from joypad.
    /// </summary>
    private void PickUpObjectFromJoypad()
    {
        if (Input.GetButton("Fire1") && IsJoyPad)
        {
            if (_hoverObject != null) PickUpObjectTimer();
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
    /// This method shows/hides the object description.
    /// </summary>
    /// <param name="isShow">The flag to show or hide object description,
    ///                      of type bool</param>
    private void SetObjectDescription(bool isShow)
    {
        if (isShow) // Condition to show object description
        {
            if (_hoverObject as Item) // Checking if interactive is any item
            {
                // Checking if the object is Equipment Item
                if (_hoverObject as WeaponItem)
                {
                    // Showing the weapon description hereS
                    UIInGameUIController.Instance.ShowPopup(
                        GetDefaultWeapon(), _hoverObject);
                }
                // Checking if object is Consumable Item
                else if (_hoverObject as ConsumableItem)
                {
                    // Showing the consumable description
                    UIInGameUIController.Instance.ShowPopup(_hoverObject);
                }
                // Checking if object is Wearable Item
                else if (_hoverObject as WearableItem)
                {
                    if (_wearableItems[0] != null &&
                       ((WearableItem)_hoverObject).Wearable == WearableType.Body)
                    {
                        // Showing the wearable description here
                        UIInGameUIController.Instance.ShowPopup(
                            _wearableItems[0], _hoverObject);
                    }
                    // Condition for hand wearable item
                    else if (_wearableItems[1] != null &&
                       ((WearableItem)_hoverObject).Wearable == WearableType.Hands)
                    {
                        // Showing the wearable description here
                        UIInGameUIController.Instance.ShowPopup(
                            _wearableItems[1], _hoverObject);
                    }
                    // Condition for head wearable item
                    if (_wearableItems[2] != null &&
                       ((WearableItem)_hoverObject).Wearable == WearableType.Head)
                    {
                        // Showing the wearable description here
                        UIInGameUIController.Instance.ShowPopup(
                            _wearableItems[2], _hoverObject);
                    }
                    // Condition for leg wearable item
                    else if (_wearableItems[3] != null &&
                       ((WearableItem)_hoverObject).Wearable == WearableType.Legs)
                    {
                        // Showing the wearable description here
                        UIInGameUIController.Instance.ShowPopup(
                            _wearableItems[3], _hoverObject);
                    }
                    // Condition for shoes wearable item
                    else if (_wearableItems[4] != null &&
                       ((WearableItem)_hoverObject).Wearable == WearableType.Legs)
                    {
                        // Showing the wearable description here
                        UIInGameUIController.Instance.ShowPopup(
                            _wearableItems[4], _hoverObject);
                    }
                    else // Condition for showing a single wearable item
                    {
                        // Showing the wearable description
                        UIInGameUIController.Instance.ShowPopup(_hoverObject);
                    }
                }
            }
            else // Interactive is not an item
            {
                // Condition for shop
                if (_hoverObject as ShopInteractive)
                    UIInGameUIController.Instance.ShowPopup(_hoverObject);
            }
        }
        else UIInGameUIController.Instance.HideAllPopUp(); // Condition for hiding all
                                                           // object descriptions
    }

    /// <summary>
    /// This method picks up the object.
    /// </summary>
    private void PickUpInteractive()
    {
        // Condition for picking up the weapon
        if (_hoverObject as WeaponItem)
        {
            PickUpWeapon1(((WeaponItem)_hoverObject));
        }
        else if (_hoverObject as ConsumableItem) // Condition for picking up
        {                                        // consumable
            // Condition for healing the player
            if (((ConsumableItem)_hoverObject).Consumable == ConsumableType.Heal)
                Heal(((ConsumableItem)_hoverObject).GetValue());
            // Condition for adding coins
            else if (((ConsumableItem)_hoverObject).Consumable == ConsumableType.Coin)
                AddCoin(((ConsumableItem)_hoverObject).GetValue());

            ((ConsumableItem)_hoverObject).PickUpItem(); // Picking up the item
        }
        // Condition for picking up the wearable item
        else if (_hoverObject as WearableItem)
        {
            // Setting the wearable item to its correct equipment place
            // and removing the old item if exist
            UpdateWearableItems(((WearableItem)_hoverObject));

            // Picking up the wearable item
            ((WearableItem)_hoverObject).PickUpItem();
        }
        // Condition to perform shop action
        else if(_hoverObject as ShopInteractive)
        {
            ((ShopInteractive)_hoverObject).Action();
        }
    }

    /// <summary>
    /// This method sets the wearable item and removes any similar wearable item, 
    /// also removes and applies blendshape.
    /// </summary>
    /// <param name="item">The item to store or remove if present,
    ///                    of type WearableItem</param>
    private void UpdateWearableItems(WearableItem item)
    {
        // Checking if the item is body
        if (item.Wearable == WearableType.Body)
        {
            // Condition for removing the body item
            if (_wearableItems[0] != null)
            {
                SetWearableItemBlendShape(_wearableItems[0], 0); // Removing blendshape
                _wearableItems[0].DropItem(GameWorldManager.Instance.Equipments,
                                      transform.position);
                _wearableItems[0] = null; // Removing the item's reference
            }

            _wearableItems[0] = item; // Setting the new item
        }
        else if (item.Wearable == WearableType.Hands)
        {
            // Todo: remove the hand item and its stats
        }
        // Checking if the item is head
        else if (item.Wearable == WearableType.Head)
        {
            // Condition for removing the head item
            if (_wearableItems[2] != null)
            {
                SetWearableItemBlendShape(_wearableItems[2], 0); // Removing blendshape
                _wearableItems[2].DropItem(GameWorldManager.Instance.Equipments,
                                      transform.position);
                _wearableItems[2] = null; // Removing the item's reference
            }

            _wearableItems[2] = item; // Setting the new item
        }
        // Checking if the item is legs
        else if (item.Wearable == WearableType.Legs)
        {
            // Condition for removing the legs item
            if (_wearableItems[3] != null)
            {
                SetWearableItemBlendShape(_wearableItems[3], 0); // Removing blendshape
                _wearableItems[3].DropItem(GameWorldManager.Instance.Equipments,
                                      transform.position);
                _wearableItems[3] = null; // Removing the item's reference
            }

            _wearableItems[3] = item; // Setting the new item
        }
        // Checking if the item is shoes
        else if (item.Wearable == WearableType.Shoes)
        {
            // Condition for removing the legs item
            if (_wearableItems[4] != null)
            {
                SetWearableItemBlendShape(_wearableItems[4], 0); // Removing blendshape
                _wearableItems[4].DropItem(GameWorldManager.Instance.Equipments,
                                      transform.position);
                _wearableItems[4] = null; // Removing the item's reference
            }

            _wearableItems[4] = item; // Setting the new item
        }

        SetWearableItemBlendShape(item, 100); // Giving blendshape
    }

    /// <summary>
    /// This method applies blendshape to the player model.
    /// </summary>
    /// <param name="item">The item from which the blendshape will be applied,
    ///                    of type WearableItem</param>
    /// <param name="weight">The amount of blendshape to apply, of type float</param>
    private void SetWearableItemBlendShape(WearableItem item, float weight)
    {
        // Condition to apply a blendshape
        if (item.BlendShapeType != MeshShapeType.None)
        {
            SkinnedMesh.SetBlendShapeWeight((int)item.BlendShapeType, weight);
        }
    }

    /// <summary>
    /// This method calculates the damage value using the defense stat, damage value
    /// is never 0 and the lowest value is 1.
    /// </summary>
    /// <param name="amount">The damage value to reduce from, or type int</param>
    /// <returns>The new damage value, if damage value 0 then converted to 1,
    ///          of type int</returns>
    private int CalculateDamage(int amount)
    {
        return (amount - _statDefense) <= 0 ? 1 : amount - _statDefense;
    }

    /// <summary>
    /// This method checks if all interactive menus are closed.
    /// </summary>
    /// <returns>True means interactive menus are closed, false otherwise,
    ///          of type bool</returns>
    private bool IsMenusClosed()
    {
        // Add other menu checks over here using 'and'
        // expression
        return !UIShopController.Instance.IsMenuShown;
    }

    /// <summary>
    /// This method initializes the player character at the start up in PlayerCharacter.
    /// </summary>
    protected override void InitializeStartUp()
    {
        base.InitializeStartUp();

        // Initializing the size of the wearable items
        _wearableItems = new WearableItem[5];
    }

    /// <summary>
    /// This method picks up another weapon for index 0th and plays visual effects.
    /// </summary>
    /// <param name="weaponItem">The weapon to pick up, of type WeaponItem</param>
    protected override void PickUpWeapon1(WeaponItem weaponItem)
    {
        base.PickUpWeapon1(weaponItem);
        weaponItem.PickUpItem(RightHand);
    }

    /// <summary>
    /// This method hurts the player.
    /// </summary>
    /// <param name="amount">The amount of damage to take, of type int</param>
    public override void TakeDamage(int amount)
    {
        // Conditions to take damage by calculating the damage
        if(!_IsDash) base.TakeDamage(CalculateDamage(amount));
    }
    
    /// <summary>
    /// This method adds a hovered over interactive.
    /// </summary>
    /// <param name="hoverObject">The object that is being hovered over by the
    ///                           player, of type Interactive</param>
    public void AddHoverObject(Interactive hoverObject)
    {
        // Condition to check if there are no current hovered object
        if (_hoverObject == null)
        {
            _hoverObject = hoverObject;

            SetObjectDescription(true); // Showing interactive description
        }
        else if(_hoverObject != hoverObject) // Checking if not same object
        {
            if(Vector3.Distance(transform.position, hoverObject.transform.position) <
               Vector3.Distance(transform.position, _hoverObject.transform.position))
            {
                _hoverObject = hoverObject;

                SetObjectDescription(true); // Showing interactive description
            }
        }

        // Note: Don't show weapon description here because the detector is using
        //       OnTriggerStay so then the description menu will be called every
        //       detection frame which is not good for UI.
    }

    /// <summary>
    /// This method gets/equips an item from the shop.
    /// </summary>
    /// <param name="shopObject">The item to get or equip, of type
    ///                          Interactive</param>
    public void AddObject(Interactive shopObject)
    {
        _hoverObject = shopObject; // Setting the item to be equiped
        PickUpObjectInstant(); // Equipping the item
    }

    /// <summary>
    /// This method removes the interactive from being picked up.
    /// </summary>
    /// <param name="hoverItem">The object needed to check if to remove
    ///                         the hover object, of type 
    ///                         Interactive</param>
    public void RemoveHoverObject(Interactive hoverItem)
    {
        // Condition for removing the selected hover object
        if(_hoverObject == hoverItem)
        {
            _hoverObject = null;
            SetObjectDescription(false);
        }
    }

    /// <summary>
    /// This method clears the hover object and is usually called
    /// by interactive objects like Shop.
    /// </summary>
    public void ClearHoverObject()
    {
        // Condition for clearing the hover object
        if (_hoverObject != null) _hoverObject = null;
    }

    /// <summary>
    /// This method is for picking up objects with timer.
    /// </summary>
    public void PickUpObjectTimer()
    {
        if (IsHoverObject) // Condition to check if the player is hovering over an object
        {
            // Counting how long the button has been pressed
            _pickUpTimer = (_pickUpTimer + Time.deltaTime) >= PickUpTimer ?
                                             PickUpTimer :
                                            _pickUpTimer + Time.deltaTime;

            // Condition for picking up the object
            if(_pickUpTimer == PickUpTimer)
            {
                PickUpInteractive(); // Picking up the object
                _hoverObject = null; // Clearing the hover selected object
                ResetPickupTimer();

                UIInGameUIController.Instance.ResetAllBar();
                SetObjectDescription(false);
                return; // No further logic required
            }

            UIInGameUIController.Instance
                .SetAllBar(_hoverObject, _pickUpTimer / PickUpTimer);
        }
    }

    /// <summary>
    /// This method picks up the object instantly.
    /// </summary>
    public void PickUpObjectInstant()
    {
        if (IsHoverObject) // Condition to check if the player is hovering over a weapon
        {
            PickUpInteractive(); // Picking up the object
            _hoverObject = null; // Clearing the hover selected object

            ResetPickupTimer();
            SetObjectDescription(false);
        }
    }

    /// <summary>
    /// This method resets the pickup timer.
    /// </summary>
    public void ResetPickupTimer() { _pickUpTimer = 0; }

    /// <summary>
    /// This method adds defense stat from items or others.
    /// </summary>
    /// <param name="amount">The amount of defense stat to add, of type int</param>
    public void AddStatDefense(int amount) { _statDefense += amount; }

    /// <summary>
    /// This method removes defense stat.
    /// </summary>
    /// <param name="amount">The amount of defense stat to remove, of type int</param>
    public void RemoveStatDefense(int amount)
    { _statDefense = (_statDefense - amount) <= 0 ? 0 : _statDefense - amount; }

    /// <summary>
    /// This method gets the WearableItem of the player.
    /// </summary>
    /// <param name="index">The index of the WearableItem, of type int</param>
    /// <returns>The wearable item of the player, of type WearableItem</returns>
    public WearableItem GetWearableItem(int index) { return _wearableItems[index]; }

    /// <summary>
    /// This method takes the button A attack command from the on screen button A.
    /// </summary>
    public void ButtonA() { if (IsAcceptInput) _isAttackButtonA = true; }

    /// <summary>
    /// This method takes the button B dash command from the on screen button B.
    /// </summary>
    public void ButtonB() { _isDashButton = true; }
}