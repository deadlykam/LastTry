using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles all the weapon related events. Like picking up weapon, current weapon,
/// enemy weapon and boss weapon. This class has replaced the previous class called 
/// WeaponInfo.
/// </summary>
public class WeaponItem : Items
{
    public WeaponType Weapon = WeaponType.None;
    public int DamageMax;
    public int DamageMin;
    public CombatAnimation[] AttackAnimations;

    /// <summary>
    /// This method returns a random damage value within the range.
    /// </summary>
    /// <returns>The random damage value within the range, of type int</returns>
    public int GetDamage() { return Random.Range(DamageMin, DamageMax); }

    /// <summary>
    /// This method shows the description of the weapon.
    /// </summary>
    /// <returns>The description of the weapon, of type string</returns>
    public override string GetDescription()
    {
        return base.GetDescription() + ". Damage: " 
               + DamageMin.ToString() + " - " 
               + DamageMax.ToString();
    }
}

public enum WeaponType { None, Sword, Staff, Hammer };
