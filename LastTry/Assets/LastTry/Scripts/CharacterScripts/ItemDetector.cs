using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetector : MonoBehaviour
{
    public PlayerCharacter Player;

    private void OnTriggerStay(Collider other)
    {
        // Checking if the detected object is item
        if(other.CompareTag("Item"))
            Player.AddHoverObject(other.GetComponent<Items>());
    }

    private void OnTriggerExit(Collider other)
    {
        // Checking if the detected object is item
        if (other.CompareTag("Item"))
            Player.RemoveHoverObject(other.GetComponent<Items>());
    }
}
