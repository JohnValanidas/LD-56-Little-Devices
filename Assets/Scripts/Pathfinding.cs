using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    public Transform body;
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
            Destroy(this);
            return;
        }

        if (!currentTargetCell.HasValue)
        {
            return;
        }

        var cellPos = currentTargetCell.Value;
        var currentTarget = gameGrid.CellToWorld(cellPos);
        var distance = currentTarget - body.position;
        var direction = distance.normalized * speed;

        body.Translate(direction * Time.deltaTime);
        var newDistance = currentTarget - body.position;

        if (distance.magnitude <= minDistance || newDistance.magnitude > distance.magnitude)
        {
            UpdateNextTargetCell();
        }
    }

    private void UpdateNextTargetCell()
    {
        if (target == null)
        {
            return;
        }

        var currentCell = gameGrid.WorldTocell(body.position);
        var goalCell = gameGrid.WorldTocell(target.position);
        var path = gameGrid.FindPath(currentCell, gameGrid.WorldTocell(target.position));
        if (path != null)
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
