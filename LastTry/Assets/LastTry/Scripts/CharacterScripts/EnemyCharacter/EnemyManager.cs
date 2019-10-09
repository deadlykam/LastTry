using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Manager Properties")]
    public PlayerCharacter Player;
    public HealthBarPoolingManager HealthBarManager;

    
    /// <summary>
    /// This method requests an available health bar to be set.
    /// </summary>
    /// <param name="enemy">The enemy to set the healthbar to, of type
    ///                     transform</param>
    /// <returns>-1 means request failed, any other number means succeeded,
    ///          of type int</returns>
    public void RequestHealthBar(Transform enemy)
    {
        HealthBarManager.RequestHealthBar(enemy);
    }

    /// <summary>
    /// This method requests to free up a health bar.
    /// </summary>
    /// <param name="index">The index of the health bar to be freed,
    ///                     of type int</param>
    public void RequestToReleasehealthBar(int index)
    { HealthBarManager.RequestReleaseHealthBar(index); }
}
