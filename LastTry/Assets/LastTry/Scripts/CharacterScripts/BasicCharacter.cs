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
    protected bool IsDead { get { return _health == 0; } }

    public WeaponInfo[] Weapons; // For storing multiple weapons for player and boss

    public float SpeedMovement;

    [Range(0.0f, 1.0f)]
    public float SpeedSlerp;

    public float HurtTimer;
    private float _hurtTimer;
    protected bool IsHurt { get { return _hurtTimer != 0; } }

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
    /// <returns>The weapon at the 0th index, of type WeaponInfo</returns>
    protected WeaponInfo GetDefaultWeapon()
    {
        return Weapons[0];
    }

    /// <summary>
    /// This method gets the default weapon type of the character which is
    /// the weapontype at the 0th index.
    /// </summary>
    /// <returns>The weapon type at the 0th index, of type WeaponType</returns>
    protected WeaponInfo.WeaponType GetDefaultWeaponType()
    {
        return GetDefaultWeapon().Weapon;
    }

    /// <summary>
    /// This method picks up another weapon.
    /// </summary>
    protected virtual void PickUpWeapon()
    {
        //Todo: This method picks up a weapon and either replaces at the 0th index
        //      or adds to 0th or 1st index.
    }

    /// <summary>
    /// This method takes damage for the BasicCharacter
    /// </summary>
    /// <param name="amount">The amount of damage to take, of type int</param>
    public virtual void TakeDamage(int amount)
    {
        _hurtTimer = HurtTimer; // Starting the hurt timer

        // Taking damage and keeping health with in range
        _health = (_health - amount <= 0) ? 0 : _health - amount;

        // Todo: Put all the death conditions here or in child
        //       override method
        // Condition for the BasicCharacter to die
        if (_health == 0)
        {
            CharacterRigid.useGravity = false;
            CharacterRigid.isKinematic = true;

            CharacterCollider.isTrigger = true;
        }
    }
}
