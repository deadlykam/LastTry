using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarPoolingManager : MonoBehaviour
{
    public Canvas[] Backgrounds;
    public Canvas[] HealthBars;

    private Queue<Transform> _requestHealthBar = new Queue<Transform>();
    private Queue<int> _releaseHealthBar = new Queue<int>();

    private int _pointer;

    private bool _isProcessingRequest = false;
    private bool _isProcessingRelease = false;

    private void Update()
    {
        // Condition for processing the next healthbar
        if (_requestHealthBar.Count != 0 && !_isProcessingRequest)
        {
            _isProcessingRequest = true;
            GetHealthBar(_requestHealthBar.Dequeue());
        }

        // Condition for releasing a healthbar
        if (_releaseHealthBar.Count != 0 && !_isProcessingRelease)
        {
            _isProcessingRelease = true;
            ReleaseHealthBar(_releaseHealthBar.Dequeue());
        }
    }

    /// <summary>
    /// This method sets the health bar to an enemy.
    /// </summary>
    /// <param name="enemy">The enemy transform to follow by the health bar,
    ///                     of type Transform</param>
    /// <returns>-1 means no health bar found, any other number means health bar found,
    ///          of type int</returns>
    private void GetHealthBar(Transform enemy)
    {
        // Incrementing pointer to get the next healthbar
        _pointer = (_pointer + 1) >= Backgrounds.Length ? 0 : _pointer + 1;

        // Condition to check if health bar is not being used
        if (!Backgrounds[_pointer].enabled)
        {
            Backgrounds[_pointer].enabled = true; // Enabling the background
            HealthBars[_pointer].enabled = true;  // Enabling the bar

            // Making the health bar move with the enemy
            Backgrounds[_pointer].GetComponent<UIFollow>().Target = enemy;
            HealthBars[_pointer].GetComponent<UIFollowCharacter>().Target = enemy;
            HealthBars[_pointer].GetComponent<UIFollowCharacter>().Character =
                enemy.GetComponent<BasicCharacter>();

            // Setting the reference to the healthbar
            enemy.GetComponent<EnemyCharacter>().SetHealthBarReference(_pointer);
        }
        else enemy.GetComponent<EnemyCharacter>().SetHealthBarReference(-1);

        _isProcessingRequest = false; // Processing done
    }

    /// <summary>
    /// This method frees up a health bar.
    /// </summary>
    /// <param name="index">The index of the health bar to be freed,
    ///                     of type int</param>
    private void ReleaseHealthBar(int index)
    {
        Backgrounds[index].enabled = false;
        HealthBars[index].enabled = false;
        _isProcessingRelease = false;
    }

    /// <summary>
    /// This method adds the request to a queue.
    /// </summary>
    /// <param name="enemy">Request made on the enemy transform, of type Transform</param>
    public void RequestHealthBar(Transform enemy) { _requestHealthBar.Enqueue(enemy); }

    /// <summary>
    /// This method requests to free up a healthbar.
    /// </summary>
    /// <param name="index">The index of the health bar to free up, of type int</param>
    public void RequestReleaseHealthBar(int index) { _releaseHealthBar.Enqueue(index); }
}
