using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    public Transform target;
    public float minDistance = 0.1f;
    public float speed;

    private GameGrid _gameGrid;
    private Vector3Int? _targetcell;

    // Start is called before the first frame update
    void Start()
    {
        _gameGrid = FindObjectOfType<GameGrid>();
        Debug.Assert(_gameGrid != null, "GameGrid NOT FOUND!");

        _gameGrid.AddPathEventListener(UpdateNextTargetCell);
    }

    private void OnDestroy()
    {
        _gameGrid.RemovePathEventListener(UpdateNextTargetCell);
    }

    // Update is called once per frame
    void Update() {
        if (target == null || !_targetcell.HasValue)
        {
            Destroy(gameObject);
            return;
        }

        var cellPos = _targetcell.Value;
        var currentTarget = _gameGrid.CellToWorld(cellPos);
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

        var currentCell = _gameGrid.WorldTocell(transform.position);
        var goalCell = _gameGrid.WorldTocell(targetPosition.Value);
        var path = _gameGrid.FindPath(currentCell, goalCell);
        if (path != null && path.Count > 1)
        {
            _targetcell = path[1];
        }
        else
        {
            _targetcell = null;
        }
    }
}
