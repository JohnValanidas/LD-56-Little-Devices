using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {
    public IList<Vector3Int> path;
    public float minDistance = 0.1f;
    public float speed;
    public Spawner spawner;
    public GameGrid gameGrid;

    public event Action OnDestroyed;

    private float _cellPosition = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawner.OnTargetChanged += UpdateTargetPath;
        transform.position = gameGrid.CellToWorld(path[0]);
        _cellPosition = 0;
    }

    private void OnDestroy()
    {
        spawner.OnTargetChanged -= UpdateTargetPath;
        OnDestroyed?.Invoke();
    }

    // Update is called once per frame
    void Update() {
        if (path == null || path.Count <= 1)
        {
            Destroy(gameObject);
            return;
        }

        _cellPosition = Math.Min(_cellPosition + speed * Time.deltaTime, path.Count - 1);

        var currentCellIndex = (int)Math.Floor(_cellPosition);
        var targetCellIndex = currentCellIndex + 1;
        var currentOffset = _cellPosition - currentCellIndex;

        var currentCell = gameGrid.CellToWorld(path[currentCellIndex]);
        var targetCell = gameGrid.CellToWorld(path[targetCellIndex]);
        var direction = targetCell - currentCell;

        transform.position = currentCell + (direction * currentOffset);
    }

    private void UpdateTargetPath(IList<Vector3Int> path)
    {
        this.path = path;
        _cellPosition = Math.Min(_cellPosition, path.Count - 1);
    }
}
