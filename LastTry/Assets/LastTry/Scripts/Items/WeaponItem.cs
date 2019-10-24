using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles all the weapon related events. Like picking up weapon, 
/// current weapon, enemy weapon and boss weapon. This class has replaced the previous
/// class called WeaponInfo.
/// </summary>
public class WeaponItem : Items
{
    [Header("Weapon Item Properties")]
    public WeaponType Weapon = WeaponType.None;
    public int DamageMax;
    public int DamageMin;
    public CombatAnimation[] AttackAnimations;

    [Header("Placement In Player Properties")]
    public Vector3 PlacementPosition;
    public Vector3 PlacementRotation;

    /// <summary>
    /// This method returns a random damage value within the range.
    /// </summary>
    /// <returns>The random damage value within the range, of type int</returns>
    public int GetDamage() { return Random.Range(DamageMin, DamageMax + 1); }

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

    /// <summary>
    /// This method sets the parent to the target and sets the placement position
    /// and rotation.
    /// </summary>
    /// <param name="target">The new parent for the weapon item, of type Transform</param>
    public void SetParentToPlayer(Transform target)
    {
        transform.parent = target;
        transform.localPosition = PlacementPosition;
        transform.localRotation = Quaternion.Euler(PlacementRotation);
        SetCollider(false); // Removing collision trigger
    }
}

public enum WeaponType { None, Sword, Staff, Hammer };
