using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public GameObject Character;

    private void OnTriggerEnter(Collider other)
    {
        // Condition for adding the enemy to the weapon range
        /*if (other.CompareTag("Enemy"))
            Player.AddEnemyToRange(other.GetComponent<EnemyCharacter>());*/

        if (other.CompareTag("Enemy"))
        {
            // Checking if the gameobject has PlayerCharacter
            if(Character.GetComponent<PlayerCharacter>() != null)
            {
                // Adding the enemy to the weapon range
                Character.GetComponent<PlayerCharacter>().
                    AddEnemyToRange(other.GetComponent<EnemyCharacter>());
            }
            // Checking if the gameobject has BotCharacter
            else if(Character.GetComponent<BotCharacter>() != null)
            {
                // Targeting the first enemy
                Character.GetComponent<BotCharacter>().
                    AddEnemy(other.GetComponent<EnemyCharacter>());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        /*if (other.CompareTag("Enemy"))
            Player.RemoveEnemyFromRange(other.GetComponent<EnemyCharacter>());*/

        if (other.CompareTag("Enemy"))
        {
            // Checking if the gameobject has PlayerCharacter
            if (Character.GetComponent<PlayerCharacter>() != null)
            {
                // Removing the enemy from weapon range
                Character.GetComponent<PlayerCharacter>().
                    RemoveEnemyFromRange(other.GetComponent<EnemyCharacter>());
            }
            // Checking if the gameobject has BotCharacter
            else if (Character.GetComponent<BotCharacter>() != null)
            {
                // Targeting the first enemy
                Character.GetComponent<BotCharacter>().
                    RemoveEnemy(other.GetComponent<EnemyCharacter>());
            }
        }
    }
}
