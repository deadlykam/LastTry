using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles all the animations of a character.
/// </summary>
public class BasicAnimation : BasicCharacter
{
    // Start is called before the first frame update
    void Start()
    {
        InitializeStartUp();
    }

    /// <summary>
    /// This method initializes all basic attribute and basic animations at the start up.
    /// </summary>
    protected override void InitializeStartUp()
    {
        base.InitializeStartUp();
    }
}
