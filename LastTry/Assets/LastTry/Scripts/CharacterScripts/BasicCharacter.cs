using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles all the basic attribute of a character.
/// </summary>
public class BasicCharacter : MonoBehaviour
{
    public int HealthMax;
    public int HealthMin;
    private int _healthCur;

    public float SpeedMovement;

    // Start is called before the first frame update
    void Start()
    {
        InitializeStartUp();
    }

    /// <summary>
    /// This method initializes all basic attribute at the start up.
    /// </summary>
    protected virtual void InitializeStartUp()
    {
        _healthCur = HealthMax; // Setting up the current health
    }
}
