using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public PlayerCharacter Player;

    private void OnTriggerEnter(Collider other)
    {
        // Condition for adding the enemy to the weapon range
        if (other.CompareTag("Enemy"))
            Player.AddEnemyToRange(other.GetComponent<EnemyCharacter>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
            Player.RemoveEnemyFromRange(other.GetComponent<EnemyCharacter>());
    }
}
