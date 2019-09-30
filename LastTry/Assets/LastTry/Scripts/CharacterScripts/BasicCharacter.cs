using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles all the basic attribute of a character.
/// </summary>
public class BasicCharacter : MonoBehaviour
{
    [Header("Basic Character Properties")]
    public int HealthMax;
    public int HealthMin;
    private int _healthCur;
    public WeaponInfo[] Weapons; // For storing multiple weapons for player and boss

    public float SpeedMovement;

    [Range(0.0f, 1.0f)]
    public float SpeedSlerp;

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
        _healthCur = HealthMax; // Setting up the current health
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
}
