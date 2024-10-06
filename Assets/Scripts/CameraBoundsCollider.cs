using UnityEngine;

public class CameraBoundsCollider : MonoBehaviour
{
    public BoxCollider2D boundary; 
    public Camera cam;

    void Start()
    {
        // cam = Camera.main;
    }

    void Update()
    {
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float minX = boundary.bounds.min.x + camWidth;
        float maxX = boundary.bounds.max.x - camWidth;
        float minY = boundary.bounds.min.y + camHeight;
        float maxY = boundary.bounds.max.y - camHeight;

        // Debug.Log("x,y,X,Y: " + minX + "," + minY +  "," + maxX +"," + maxY);
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}