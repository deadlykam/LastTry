using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetector : MonoBehaviour
{
    public PlayerCharacter Player;

    private void OnTriggerStay(Collider other)
    {
        // Checking and adding the weapon item
        if (other.CompareTag("Weapons"))
            Player.AddHoverWeapon(other.GetComponent<WeaponItem>());
    }

    private void OnTriggerExit(Collider other)
    {
        // Condition for removing the hovered weapon
        if (other.CompareTag("Weapons"))
            Player.RemoveHoverWeapon(other.GetComponent<WeaponItem>());
    }
}
