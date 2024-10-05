using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundsCollider : MonoBehaviour
{
    public BoxCollider2D boundary;  // Collider that defines the boundary
    public Camera camera;

    void Start()
    {
        // camera = Camera
    }

    void LateUpdate()
    {
        // Get boundary limits
        // float minX = boundary.bounds.min.x + camWidth;
        // float maxX = boundary.bounds.max.x - camWidth;
        // float minY = boundary.bounds.min.y + camHeight;
        // float maxY = boundary.bounds.max.y - camHeight;
        //
        // // Clamp the camera position
        // float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        // float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        //
        // // Apply the clamped position
        // transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}