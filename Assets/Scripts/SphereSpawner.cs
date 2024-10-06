using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    public GameGrid gameGrid;  // Reference to your GridLayout component
    public GameObject spherePrefab;  // Prefab for the sphere you want to spawn
    public Vector3 offset; // Offset to position at the corner of the grid

    private GridLayout _gridLayout;
    void Start() {
        _gridLayout = GetComponentInParent<GridLayout>();
        Debug.Assert(_gridLayout, "Grid NOT FOUND!");
        GenerateSpheres();
    }

    void GenerateSpheres() {
        int minX = gameGrid.minTilePos[0] - 1;
        int maxX = gameGrid.maxTilePos[0] - 1;
        int minY = gameGrid.minTilePos[1] - 1;
        int maxY = gameGrid.maxTilePos[1] - 1;
        Vector3Int gridSize = new Vector3Int(maxX, maxY, 1);

        for (int x = minX; x < gridSize.x; x++)
        {
            for (int y = minY; y < gridSize.y; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                Vector3 worldPosition = _gridLayout.CellToWorld(cellPosition);

                Vector3 spherePosition = worldPosition + offset;

                Instantiate(spherePrefab, spherePosition, Quaternion.identity);
            }
        }
    }
}