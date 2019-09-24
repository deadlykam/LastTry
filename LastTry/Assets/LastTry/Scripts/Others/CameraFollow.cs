using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Follow Properties")]
    public Transform Target;
    [Range(0.0f, 1.0f)]
    public float SpeedSlerp;

    private Vector3 _cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        // Setting up the camera offset position
        _cameraOffset = transform.position - Target.position;
    }

    // LateUpdate is called after Update methods
    void LateUpdate()
    {
        // Moving the camera
        transform.position = Vector3.Slerp(transform.position,
                                           Target.position + _cameraOffset,
                                           SpeedSlerp);

        // This is for looking at the player
        // transform.LookAt(Target); // Looking at the player
    }
}
