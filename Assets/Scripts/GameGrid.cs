using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public Vector2Int minTilePos;
    public Vector2Int maxTilePos;

    public GameObject spawner;
    public GameObject target;
    public GameObject block;

    private GridLayout grid;
    private GridPathfinder pathfinder = new();

    private Dictionary<Vector3Int, GameObject> staticObjects = new Dictionary<Vector3Int, GameObject>();


    private void DrawDebugTile(Vector3 bottomLeft, Vector3 topRight, Color color, float duration)
    {
        var topLeft = new Vector3(bottomLeft.x, topRight.y);
        var bottomRight = new Vector3(topRight.x, bottomLeft.y);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);
        Debug.DrawLine(bottomLeft, bottomRight, color, duration);
        Debug.DrawLine(topLeft, topRight, color, duration);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("test");

        grid = GetComponentInParent<GridLayout>();
        Debug.Assert(grid, "Grid NOT FOUND!");

        pathfinder.IsValidNeighbor = delegate (Vector3Int cellPos)
        {
            return InGridBounds(cellPos) && !ExistsAtCell(cellPos);
        };

        pathfinder.Weight = delegate (Vector3Int current, Vector3Int neighbor)
        {
            return 1f;
        };

        pathfinder.Heuristic = delegate (Vector3Int cellPos)
        {
            return 1f;
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

            var cellPos = grid.WorldToCell(pos);
            Debug.Log($"Click! {cellPos.x}, {cellPos.y}");

            if (!ExistsAtCell(cellPos))
            {
                var instance = InstantiateAtCell(objectToInstiate, cellPos);

                var component = instance.GetComponent<Spawner>();
                if (component != null) {
                    component.target = target.transform;
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
        return staticObjects.ContainsKey(cellPos);
    }

    public GameObject InstantiateAtCell(GameObject gameObject, Vector3Int cellPos, bool isStatic = true)
    {
        var tileCenter = grid.CellToWorld(cellPos) + (grid.cellSize / 2);
        var instance = Instantiate(gameObject, tileCenter, Quaternion.identity);

        if (isStatic)
        {
            staticObjects.Add(cellPos, instance);
        }

        return instance;
    }

    public bool DestroyAtCell(Vector3Int cellPos)
    {
        Debug.Assert(staticObjects.ContainsKey(cellPos), $"No objects at cell position: {cellPos}");

        var gameObject = staticObjects[cellPos];
        Destroy(gameObject);
        staticObjects.Remove(cellPos);

        return true;
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
        return grid.CellToWorld(cellPos) + (grid.cellSize / 2);
    }

    public Vector3Int WorldTocell(Vector3 worldPos)
    {
        return grid.WorldToCell(worldPos);
    }

    public IList<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        return pathfinder.FindPath(start, goal);
    }

    private void DrawDebugGrid()
    {
        var rect = Camera.main.pixelRect;
        var minTile = grid.WorldToCell(Camera.main.ScreenToWorldPoint(rect.min));
        var maxTile = grid.WorldToCell(Camera.main.ScreenToWorldPoint(rect.max));

        var cellSize = grid.cellSize;

        for (var y = minTile.y; y <= maxTile.y; y += 1)
        {
            for (var x = minTile.x; x <= maxTile.x; x += 1)
            {
                var start = grid.CellToWorld(new Vector3Int(x, y, 0));
                var end = new Vector3(start.x + cellSize.x, start.y + cellSize.y);
                DrawDebugTile(start, end, Color.black, 1 / 60f);
            }
        }
    }
}
