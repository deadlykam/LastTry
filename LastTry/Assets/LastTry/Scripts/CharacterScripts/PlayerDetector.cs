using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public EnemyCharacter Enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Enemy.SetPlayerInWeaponRange(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            Enemy.SetPlayerInWeaponRange(false);
    }
}
