using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles all the basic attribute of a character.
/// </summary>
public class BasicCharacter : MonoBehaviour
{
    [Header("Basic Character Properties")]
    public Collider CharacterCollider;
    public Rigidbody CharacterRigid;

    public int HealthMax;
    public int HealthMin;
    [SerializeField]
    private int _health;
    protected int StatHealth = 0;
    public bool IsDead { get { return _health == 0; } }

    public WeaponItem[] Weapons; // For storing multiple weapons for player and boss

    public float SpeedMovement;

    [Range(0.0f, 1.0f)]
    public float SpeedSlerp;

    public float HurtTimer;
    private float _hurtTimer;
    protected bool IsHurt { get { return _hurtTimer != 0; } }

    public Color DamageFontColour;
    public Color HealFontColour;

    // Start is called before the first frame update
    void Start()
    {
        InitializeStartUp();
    }

    /// <summary>
    /// This method initializes all basic attribute at the start up in BasicCharacter.
    /// </summary>
    protected virtual void InitializeStartUp()
    {
        _health = HealthMax; // Setting up the current health
        _hurtTimer = 0;
    }
    
    /// <summary>
    /// This method handles the BasicCharacter update and must be called by
    /// the child class.
    /// </summary>
    protected void UpdateBasicCharacter()
    {
        if (IsHurt) // Condition for counting down the hurt timer
        {
            // Counting down the hurt timer
            _hurtTimer = (_hurtTimer - Time.deltaTime) <= 0 ? 
                         0 : _hurtTimer - Time.deltaTime;
        }
    }

    /// <summary>
    /// This method gets the default weapon of the character which is the
    /// weapon at the 0th index.
    /// </summary>
    /// <returns>The weapon at the 0th index, of type WeaponItem</returns>
    protected WeaponItem GetDefaultWeapon()
    {
        return Weapons[0];
    }

    /// <summary>
    /// This method gets the default weapon type of the character which is
    /// the weapontype at the 0th index.
    /// </summary>
    /// <returns>The weapon type at the 0th index, of type WeaponType</returns>
    protected WeaponType GetDefaultWeaponType()
    {
        return GetDefaultWeapon().Weapon;
    }

    /// <summary>
    /// This method picks up another weapon for index 0th.
    /// </summary>
    /// <param name="weaponItem">The weapon to pick up, of type WeaponItem</param>
    protected virtual void PickUpWeapon1(WeaponItem weaponItem)
    {
        // Dropping the item in the game world
        Weapons[0].DropItem(GameWorldManager.Instance.Equipments);

        Weapons[0] = weaponItem; // Replacing the weapon at the 0th index
    }

    /// <summary>
    /// This method takes damage for the BasicCharacter
    /// </summary>
    /// <param name="amount">The amount of damage to take, of type int</param>
    public virtual void TakeDamage(int amount)
    {
        _hurtTimer = HurtTimer; // Starting the hurt timer

        // Taking damage and keeping health with in range
        _health = (_health - amount) <= 0 ? 0 : _health - amount;

        // Condition for the BasicCharacter to die
        if (_health == 0)
        {
            CharacterRigid.useGravity = false;
            CharacterRigid.isKinematic = true;

            CharacterCollider.isTrigger = true;
        }

        // Starting a damage font effect
        UIDamageFontManager.Instance.RequestDamageFont(transform.position, amount,
                                                       DamageFontColour);
    }

    /// <summary>
    /// This method heals the basic character.
    /// </summary>
    /// <param name="amount">The amount of health to add to the current health, if
    ///                      health amount is over max then max amount will be used,
    ///                      of type int</param>
    public virtual void Heal(int amount)
    {
        // Calculating health from healing
        _health = (_health + amount) >= GetTotalHealth() ? 
                                        GetTotalHealth() : _health + amount;

        // Starting a heal font effect
        UIDamageFontManager.Instance.RequestDamageFont(transform.position, amount,
                                                       HealFontColour);
    }

    /// <summary>
    /// This method returns the health percentage value.
    /// </summary>
    /// <returns>The health percentage value of health, of type float</returns>
    public virtual float GetHealthPercentage()
    { return (float)_health / (float)GetTotalHealth(); }

    /// <summary>
    /// This method adds health stat from items or others.
    /// </summary>
    /// <param name="amount">The amount of health stat to add, of type int</param>
    public virtual void AddStatHealth(int amount) { StatHealth += amount; }

    /// <summary>
    /// This method removes health stat.
    /// </summary>
    /// <param name="amount">The amount of health stat to remove, of type int</param>
    public virtual void RemoveStatHealth(int amount)
    {
        StatHealth = (StatHealth - amount) <= 0 ? 0 : StatHealth - amount;

        // Checking if current health is above total health and fixing it
        _health = _health >= GetTotalHealth() ? GetTotalHealth() : _health;
    }

    /// <summary>
    /// This method gets the total max health amount.
    /// </summary>
    /// <returns>The total max health, of type int</returns>
    public virtual int GetTotalHealth() { return HealthMax + StatHealth; }
}

public enum CharacterState { None, Move, Stop, MoveToEnemy, AttackEnemy, GetNextEnemy};