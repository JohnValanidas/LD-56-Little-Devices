using System;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public Vector2Int minTilePos;
    public Vector2Int maxTilePos;

    public GameObject spawner;
    public GameObject target;
    public GameObject block;

    public event Action OnRecalcPath;

    private GridLayout _grid;
    private readonly GridPathfinder _pathfinder = new();
    private readonly Dictionary<Tuple<Vector3Int, Vector3Int>, IList<Vector3Int>> _pathCache = new();
    private readonly Dictionary<Vector3Int, GameObject> _staticObjects = new();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("test");

        _grid = GetComponentInParent<GridLayout>();
        Debug.Assert(_grid, "Grid NOT FOUND!");

        _pathfinder.IsValidNeighbor = delegate (Vector3Int cellPos)
        {
            return InGridBounds(cellPos) && !ExistsAtCell(cellPos);
        };

        _pathfinder.Weight = delegate (Vector3Int current, Vector3Int neighbor)
        {
            return 1f;
        };

        _pathfinder.Heuristic = delegate (Vector3Int cellPos, Vector3Int target)
        {
            return Mathf.Abs(target.x - cellPos.x) + Mathf.Abs(target.y - cellPos.y);
        };
    }

    // Update is called once per frame
    void Update()
    {
        GameObject objectToInstiate = null;

        if (Input.GetMouseButtonDown(0)) // left
        {
            objectToInstiate = spawner;
        } else if (Input.GetMouseButtonDown(1)) // right
        {
            objectToInstiate = block;
        }

        if (objectToInstiate != null)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var cellPos = _grid.WorldToCell(pos);
            Debug.Log($"Click! {cellPos.x}, {cellPos.y}");

            if (!ExistsAtCell(cellPos))
            {
                var instance = InstantiateAtCell(objectToInstiate, cellPos);

                var component = instance.GetComponent<Spawner>();
                if (component != null) {
                    component.target = target;
                }
            } else
            {
                DestroyAtCell(cellPos);
            }
        }

        DrawDebugGrid();
    }

    public bool ExistsAtCell(Vector3Int cellPos)
    {
        return _staticObjects.ContainsKey(cellPos);
    }

    public GameObject InstantiateAtCell(GameObject prefab, Vector3Int cellPos, bool isStatic = true)
    {
        var tileCenter = _grid.CellToWorld(cellPos) + (_grid.cellSize / 2);
        var instance = Instantiate(prefab, tileCenter, Quaternion.identity);

        if (isStatic)
        {
            AddToCell(instance, cellPos);
        }

        return instance;
    }

    public void AddToCell(GameObject gameObject, Vector3Int cellPos)
    {
        Debug.Assert(!_staticObjects.ContainsKey(cellPos), $"Object already exists at cell position: {cellPos}");

        _staticObjects.Add(cellPos, gameObject);
        _pathCache.Clear();
        OnRecalcPath?.Invoke();
    }

    public GameObject RemoveAtCell(Vector3Int cellPos)
    {
        Debug.Assert(_staticObjects.ContainsKey(cellPos), $"No objects at cell position: {cellPos}");

        var gameObject = _staticObjects[cellPos];
        _staticObjects.Remove(cellPos);
        _pathCache.Clear();
        OnRecalcPath?.Invoke();

        return gameObject;
    }

    public void DestroyAtCell(Vector3Int cellPos)
    {
        Destroy(RemoveAtCell(cellPos));
    }

    public bool InGridBounds(Vector3Int cellPos)
    {
        if (cellPos.x < minTilePos.x || cellPos.x > maxTilePos.x || cellPos.y < minTilePos.y || cellPos.y > maxTilePos.y)
        {
            return false;
        }

        return true;
    }

    public Vector3 CellToWorld(Vector3Int cellPos)
    {
        return _grid.CellToWorld(cellPos) + (_grid.cellSize / 2);
    }

    public Vector3Int WorldTocell(Vector3 worldPos)
    {
        return _grid.WorldToCell(worldPos);
    }

    public IList<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        var tuple = Tuple.Create(start, goal);
        if (_pathCache.ContainsKey(tuple))
        {
            return _pathCache[tuple];
        }

        var path = _pathfinder.FindPath(start, goal);
        _pathCache[tuple] = path;
        return path;
    }

    private void DrawDebugTile(Vector3 bottomLeft, Vector3 topRight, Color color, float duration)
    {
        var topLeft = new Vector3(bottomLeft.x, topRight.y);
        var bottomRight = new Vector3(topRight.x, bottomLeft.y);

        Debug.DrawLine(bottomLeft, topLeft, color, duration);
        Debug.DrawLine(bottomLeft, bottomRight, color, duration);
        Debug.DrawLine(topLeft, topRight, color, duration);
    }

    private void DrawDebugGrid()
    {
        var rect = Camera.main.pixelRect;
        var minTile = _grid.WorldToCell(Camera.main.ScreenToWorldPoint(rect.min));
        var maxTile = _grid.WorldToCell(Camera.main.ScreenToWorldPoint(rect.max));

        var cellSize = _grid.cellSize;

        for (var y = minTile.y; y <= maxTile.y; y += 1)
        {
            for (var x = minTile.x; x <= maxTile.x; x += 1)
            {
                var start = _grid.CellToWorld(new Vector3Int(x, y, 0));
                var end = new Vector3(start.x + cellSize.x, start.y + cellSize.y);
                DrawDebugTile(start, end, Color.black, 1 / 60f);
            }
        }
    }
}
