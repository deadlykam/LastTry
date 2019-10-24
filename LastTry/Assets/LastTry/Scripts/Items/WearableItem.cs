using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for all the armours and wearable items like pendant, ring, etc.
/// </summary>
public class WearableItem : Items
{
    [Header("Wearable Item Properties")]
    public GameObject WorldObject;
    public SkinnedMeshRenderer WearableObject;
    //public SkinnedMeshRenderer SkinMesh;
    public WearableType Wearable;
    public StatInfo[] Stats;

    private string _description;

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

            _description = _description + "\n"; // Adding a new line
        }

        return _description;
    }

    /// <summary>
    /// This method picks up the item and wears it.
    /// </summary>
    public void PickUpItem()
    {
        WorldObject.SetActive(false); // Hiding the world item gameobject
        WearableObject.gameObject.SetActive(true); // Showing the wearable item gameobject

        // Making the parent of the wearable object to the Player
        WearableObject.transform.parent = GameWorldManager
                                            .Instance.Player.SkinnedMesh.transform;

        // Updating the bones of the wearable item
        WearableObject.bones = GameWorldManager.Instance.Player.SkinnedMesh.bones;
        WearableObject.rootBone = GameWorldManager.Instance.Player.SkinnedMesh.rootBone;
    }
}

[System.Serializable]
public struct StatInfo
{
    public int StatAmount;
    public StatType Stat;
}

public enum WearableType { None, ArmourHead, ArmourBody, ArmourGloves, ArmourLegs,
                           ArmourShoes };

public enum StatType { None, Attack, Defense, Special };