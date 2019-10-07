using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [Depricated] This class may not be required anymore. It has been replaced by
/// WeaponItem class. See WeaponItem for more details. This class will be deleted
/// in future.
/// </summary>
public class WeaponInfo : MonoBehaviour
{
    public WeaponType Weapon;
    public int DamageMax;
    public int DamageMin;
    public int Cost;

    /// <summary>
    /// This method returns a random damage value within the range.
    /// </summary>
    /// <returns>The random damage value within the range, of type int</returns>
    public int GetDamage() { return Random.Range(DamageMin, DamageMax); }
}
