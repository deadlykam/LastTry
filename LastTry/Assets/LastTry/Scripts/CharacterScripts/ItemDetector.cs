using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetector : MonoBehaviour
{
    public PlayerCharacter Player;

    private void OnTriggerStay(Collider other)
    {
        // Taking in any interactives
        Player.AddHoverObject(other.GetComponent<Interactive>());
    }

    private void OnTriggerExit(Collider other)
    {
        // Removing any interactives
        Player.RemoveHoverObject(other.GetComponent<Interactive>());
    }
}
