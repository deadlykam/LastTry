using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradableItem : Item
{
    [Header("Upgradable Properties")]
    public int UpgradeLimit;
    private int _upgradeCurrent;
    public int AttackOffset;
    private int _attackCurrent;
    public int DefenseOffset;
    private int _defenseCurrent;
    public int HealthOffset;
    private int _healthCurrent;

    public bool IsUpgradable { get { return _upgradeCurrent < UpgradeLimit; } }

    /// <summary>
    /// This method initializes all the attribute at the start up in UpgradableItem.
    /// </summary>
    protected virtual void InitializeStartUp() { ResetUpgrade(); }

    /// <summary>
    /// This method gets the upgrade attack value.
    /// </summary>
    /// <returns>The attack value of the upgraded item, of type int</returns>
    protected int GetAttack() { return _attackCurrent; }

    /// <summary>
    /// This method gets the upgrade defense value.
    /// </summary>
    /// <returns>The defense value of the upgraded item, of type int</returns>
    protected int GetDefense() { return _defenseCurrent; }

    /// <summary>
    /// This method gets the upgrade health value.
    /// </summary>
    /// <returns>The health value of the upgraded item, of type int</returns>
    protected int GetHealth() { return _healthCurrent; }

    /// <summary>
    /// This method upgrades the upgradable item.
    /// </summary>
    public virtual void UpgradeItem()
    {
        if (IsUpgradable) // Checking if item is upgradable
        {
            _attackCurrent += AttackOffset;   // Upgrading attack
            _defenseCurrent += DefenseOffset; // Upgrading defense
            _healthCurrent += HealthOffset;   // Upgrading health

            // Incrementing upgrade counter
            _upgradeCurrent = (_upgradeCurrent + 1) >= UpgradeLimit 
                              ? UpgradeLimit : _upgradeCurrent + 1;
        }
    }

    /// <summary>
    /// This method gets the upgrade percentage value.
    /// </summary>
    /// <returns>The upgrade percentage value, of type float</returns>
    public virtual float GetUpgradePercentage() { return _upgradeCurrent / UpgradeLimit; }

    /// <summary>
    /// This method returns all the attribute value in UpgradableItem.
    /// </summary>
    /// <returns>All the attribute value, of type string</returns>
    public virtual string GetAttributeDescription()
    {
        return AttackOffset.ToString() + " " + DefenseOffset.ToString() + " " + 
               HealthOffset.ToString();
    }

    /// <summary>
    /// This method resets the upgraded values.
    /// </summary>
    public virtual void ResetUpgrade()
    {
        _upgradeCurrent = 0;
        _attackCurrent = 0;
        _defenseCurrent = 0;
        _healthCurrent = 0;
    }
}
