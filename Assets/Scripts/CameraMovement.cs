
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float speed = 5;

    private Transform Transform;
    private Camera  mainCamera;
    // Start is called before the first frame update
    private void Awake() {
        mainCamera = Camera.main;
    }
    void LateUpdate()
    {
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 vec = new Vector2(moveHorizontal, moveVertical).normalized * speed;
        mainCamera.transform.Translate(vec * Time.deltaTime);
    }
}
