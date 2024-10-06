using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    public Transform target;

    public float minDistance = 0.1f;

    public float speed;

    private GameGrid gameGrid;

    Vector3Int? currentTargetCell;

    // Start is called before the first frame update
    void Start()
    {
        gameGrid = FindObjectOfType<GameGrid>();
        Debug.Assert(gameGrid != null, "GameGrid NOT FOUND!");

        UpdateNextTargetCell();
    }

    // Update is called once per frame
    void Update() {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (!currentTargetCell.HasValue)
        {
            return;
        }

        var cellPos = currentTargetCell.Value;
        var currentTarget = gameGrid.CellToWorld(cellPos);
        var distance = currentTarget - transform.position;
        var direction = distance.normalized * speed;

        transform.Translate(direction * Time.deltaTime);
        var newDistance = currentTarget - transform.position;

        if (distance.magnitude <= minDistance || newDistance.magnitude > distance.magnitude)
        {
            UpdateNextTargetCell();
        }
    }

    private void UpdateNextTargetCell()
    {
        var targetPosition = target?.transform.position;
        if (!targetPosition.HasValue)
        {
            return;
        }

        var currentCell = gameGrid.WorldTocell(transform.position);
        var goalCell = gameGrid.WorldTocell(targetPosition.Value);
        var path = gameGrid.FindPath(currentCell, goalCell);
        if (path != null && path.Count > 0)
        {
            path.RemoveAt(0);
            currentTargetCell = path[0];
        }
        else
        {
            currentTargetCell = null;
        }
    }
}
