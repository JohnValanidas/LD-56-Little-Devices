using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    public Transform body;
    public Transform target;

    public float minDistance = 0;

    public float speed;

    private GameGrid gameGrid;

    // Start is called before the first frame update
    void Start()
    {
        gameGrid = FindObjectOfType<GameGrid>();
    }

    // Update is called once per frame
    void Update() {
        if (!target)
        {
            return;
        }

        Vector3 distance = target.position - body.position;

        if (distance.magnitude <= minDistance) {
            return;
        }
        
        Vector3 direction = distance.normalized * speed;
        body.Translate(direction * Time.deltaTime);
    }
}
