using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for all the armours and wearable items like pendant, ring, etc.
/// </summary>
public class WearableItem : UpgradableItem
{
    [Header("Wearable Item Properties")]
    public GameObject WorldObject;
    public SkinnedMeshRenderer WearableObject;
    public WearableType Wearable;
    public MeshShapeType BlendShapeType;
    public StatInfo[] Stats;

    private string _description;


    private void Start()
    {
        InitializeStartUp(); // Initializing the upgrade item values
    }

    /// <summary>
    /// This method shows the description of the wearable item.
    /// </summary>
    /// <returns>The description of the wearable item, of type string</returns>
    public override string GetDescription()
    {
        _description = base.GetDescription() + ".\n";

        // Loop for getting all of the stats description
        for (int i = 0; i < Stats.Length; i++)
        {
            if (Stats[i].Stat == StatType.Attack) // For showing attack description
                _description = _description + "Damage: +" + Stats[i].StatAmount.ToString();
            else if (Stats[i].Stat == StatType.Defense) // For showing defense description
                _description = _description + "Defense: +" + Stats[i].StatAmount.ToString();
            else if (Stats[i].Stat == StatType.Health) // For showing defense description
                _description = _description + "Health: +" + Stats[i].StatAmount.ToString();

            _description = _description + "\n"; // Adding a new line
        }

        return _description;
    }

    /// <summary>
    /// This method upgrades the WearableItem.
    /// </summary>
    public override void UpgradeItem()
    {
        base.UpgradeItem();

        // Increasing stat by offset, the current values are not used
        // because it will yield wrong calculation when removing the item
        GameWorldManager.Instance.Player.AddStatDamage(AttackOffset);
        GameWorldManager.Instance.Player.AddStatDefense(DefenseOffset);
        GameWorldManager.Instance.Player.AddStatHealth(HealthOffset);
    }

    /// <summary>
    /// This method picks up the item and wears it.
    /// </summary>
    public override void PickUpItem()
    {
        WorldObject.SetActive(false); // Hiding the world item gameobject
        WearableObject.gameObject.SetActive(true); // Showing the wearable item gameobject

        // Making the parent of the wearable object to the Player
        WearableObject.transform.parent = GameWorldManager
                                            .Instance.Player.SkinnedMesh.transform;

        // Updating the bones of the wearable item
        WearableObject.bones = GameWorldManager.Instance.Player.SkinnedMesh.bones;
        WearableObject.rootBone = GameWorldManager.Instance.Player.SkinnedMesh.rootBone;
        
        // Loop for adding all the stats
        for(int i = 0; i < Stats.Length; i++)
        {
            if (Stats[i].Stat == StatType.Attack) // Adding damage stat
                GameWorldManager.Instance.Player.AddStatDamage(Stats[i].StatAmount);
            else if (Stats[i].Stat == StatType.Defense) // Adding defense stat
                GameWorldManager.Instance.Player.AddStatDefense(Stats[i].StatAmount);
            else if (Stats[i].Stat == StatType.Health) // Adding health stat
                GameWorldManager.Instance.Player.AddStatHealth(Stats[i].StatAmount);
        }

        SetCollider(false); // Removing the collision
    }

    /// <summary>
    /// This method sets the parent to the target, sets the position offset and shows
    /// the world item model and hides the player item model.
    /// </summary>
    /// <param name="target">The new parent for the item, of type Transform</param>
    /// <param name="position">The new position for the item, of type Vector3</param>
    public override void DropItem(Transform target, Vector3 position)
    {
        base.DropItem(target, position);

        // Loop for removing all the stats
        for (int i = 0; i < Stats.Length; i++)
        {
            if (Stats[i].Stat == StatType.Attack) // Removing damage stat
                GameWorldManager.Instance.Player
                    .RemoveStatDamage(Stats[i].StatAmount + GetAttack());
            else if (Stats[i].Stat == StatType.Defense) // Removing defense stat
                GameWorldManager.Instance.Player
                    .RemoveStatDefense(Stats[i].StatAmount + GetDefense());
            else if (Stats[i].Stat == StatType.Health) // Removing health stat
                GameWorldManager.Instance.Player
                    .RemoveStatHealth(Stats[i].StatAmount + GetHealth());
        }

        WorldObject.SetActive(true); // Showing the item again
        WearableObject.gameObject.SetActive(false); // Hiding the wearable item

        // Removing the bones from the player
        WearableObject.bones = null;
        WearableObject.rootBone = null;
    }

    /// <summary>
    /// This method gets all the attribute value from the WearableItem.
    /// </summary>
    /// <returns>All the attribute value, of type string</returns>
    public override string GetAttributeDescription()
    {
        _description = ""; // Removing previous values

        // Loop for adding all the stat description
        for(int i = 0; i < Stats.Length; i++)
        {
            // Condition to add attack stat
            if (Stats[i].Stat == StatType.Attack) {
                _description += Stats[i].GetStatName() + ": "
                    + (Stats[i].StatAmount + GetAttack()).ToString() 
                    + " (+" + AttackOffset.ToString() + ")";
            }
            // Condition to add defense stat
            else if (Stats[i].Stat == StatType.Defense)
            {
                _description += Stats[i].GetStatName() + ": "
                    + (Stats[i].StatAmount + GetDefense()).ToString()
                    + " (+" + DefenseOffset.ToString() + ")";
            }
            // Condition to add health stat
            else if (Stats[i].Stat == StatType.Health)
            {
                _description += Stats[i].GetStatName() + ": "
                    + (Stats[i].StatAmount + GetHealth()).ToString()
                    + " (+" + HealthOffset.ToString() + ")";
            }

            // Checking if the stat is not the last stat
            // and adding a next line to it
            if (i != Stats.Length - 1) _description += "\n";
        }

        return _description;
    }
}

[System.Serializable]
public struct StatInfo
{
    public int StatAmount;
    public StatType Stat;

    /// <summary>
    /// This method gets the name of the stat.
    /// </summary>
    /// <returns>The name of the stat, of type string</returns>
    public string GetStatName()
    {
        if (Stat == StatType.Attack) return "Damage";
        else if (Stat == StatType.Defense) return "Defense";
        else if (Stat == StatType.Health) return "Health";

        return "null";
    }
}

public enum WearableType { None, Head, Body, Hands, Legs, Shoes };
public enum StatType { None, Attack, Defense, Special, Health };

/// <summary>
/// The enum values' name must be same as the blendshape index so that
/// changing the blendshape can be done dynamically. Give special case
/// for 'None' to specify that no blendshape needed and must always be
/// at the end.
/// </summary>
public enum MeshShapeType { LowerLeg, Torso, Arms, UpperLegs, None }