using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    public Transform body;
    public Transform target;

    public float minDistance = 0.1f;

    public float speed;

    private GameGrid gameGrid;
    private GridPathfinder gridPathfinder;

    private IList<Vector3Int> pathCells;

    // Start is called before the first frame update
    void Start()
    {
        gameGrid = FindObjectOfType<GameGrid>();
        gridPathfinder = FindObjectOfType<GridPathfinder>();

        var startCellPos = gameGrid.WorldTocell(transform.position);
        var targetCellPos = gameGrid.WorldTocell(target.position);
        pathCells = gridPathfinder.FindPath(startCellPos, targetCellPos);

        // Remove current cell pos
        pathCells.RemoveAt(0);

        Debug.Log($"Path cells: {pathCells.Count}");
    }

    // Update is called once per frame
    void Update() {
        if (pathCells == null || pathCells.Count == 0)
        {
            return;
        }

        var cellPos = pathCells[0];
        var currentTarget = gameGrid.CellToWorld(cellPos);
        var distance = currentTarget - body.position;
        var direction = distance.normalized * speed;

        body.Translate(direction * Time.deltaTime);
        var newDistance = currentTarget - body.position;

        if (distance.magnitude <= minDistance || newDistance.magnitude > distance.magnitude)
        {
            pathCells.RemoveAt(0);
        }
    }
}
