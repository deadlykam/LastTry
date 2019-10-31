using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles all the weapon related events. Like picking up weapon, 
/// current weapon, enemy weapon and boss weapon. This class has replaced the previous
/// class called WeaponInfo.
/// </summary>
public class WeaponItem : UpgradableItem
{
    [Header("Weapon Item Properties")]
    public WeaponType Weapon = WeaponType.None;
    public int DamageMax;
    public int DamageMin;
    public CombatAnimation[] AttackAnimations;

    [Header("Placement In Player Properties")]
    public Vector3 PlacementPosition;
    public Vector3 PlacementRotation;

    private void Start()
    {
        InitializeStartUp(); // Initializing the upgrade item values
    }

    /// <summary>
    /// This method gets the total damage value by adding item damage to upgrade damage.
    /// </summary>
    /// <param name="amount">The damage value to add, of type int</param>
    /// <returns>The damage value with added upgrade damage value, of type int</returns>
    private int GetTotalDamage(int amount) { return GetAttack() + amount; }

    /// <summary>
    /// This method returns a random damage value within the range.
    /// </summary>
    /// <returns>The random damage value within the range, of type int</returns>
    public int GetDamage()
    {
        return Random.Range(GetTotalDamage(DamageMin), GetTotalDamage(DamageMax) + 1);
    }

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
    public override void PickUpItem(Transform target)
    {
        base.PickUpItem(target);
        transform.localPosition = PlacementPosition;
        transform.localRotation = Quaternion.Euler(PlacementRotation);
        SetCollider(false); // Removing collision trigger
    }

    /// <summary>
    /// This method gets the attack attribute value from the WeaponItem
    /// </summary>
    /// <returns>The attack attribute value, of type string</returns>
    public override string GetAttributeDescription()
    {
        return "Damage: +" + (DamageMax + GetAttack()).ToString() 
                + " (+" + AttackOffset.ToString() + ")";
    }
}

public enum WeaponType { None, Sword, Staff, Hammer };
