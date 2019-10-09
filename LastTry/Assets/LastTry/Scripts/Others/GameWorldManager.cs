using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorldManager : MonoBehaviour
{
    public static GameWorldManager Instance;

    public Transform Root;
    public Transform Items;

    private void Start()
    {
        // The instance will be replaced each time a new world is
        // loaded so that basic game world attributes can be used
        // again
        Instance = this;
    }
}
