using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetector : MonoBehaviour
{
    public PlayerCharacter Player;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Weapons"))
            Player.AddHoverItem(other.GetComponent<Items>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Weapons"))
            Player.RemoveHoverItem(other.GetComponent<Items>());
    }
}
