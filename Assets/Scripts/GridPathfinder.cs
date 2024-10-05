using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridPathfinder : MonoBehaviour
{
    private GameGrid gameGrid;

    private List<Vector3Int> relativeNeighbors = new List<Vector3Int>();

    GridPathfinder()
    {
        relativeNeighbors.Add(new Vector3Int(1, 0));
        relativeNeighbors.Add(new Vector3Int(0, 1));
        relativeNeighbors.Add(new Vector3Int(-1, 0));
        relativeNeighbors.Add(new Vector3Int(0, -1));
    }

    void Start()
    {
        gameGrid = FindObjectOfType<GameGrid>();
    }


    private int Heuristic(Vector3Int cellPos)
    {
        return 1;
    }

    private float Weight(Vector3Int current, Vector3Int neighbor)
    {
        return 1;
    }

    private IList<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        Debug.Log($"Reconstructing path: {current}, cameFrom: {cameFrom.Count}");

        var path = new List<Vector3Int>();
        path.Add(current);

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }

        path.Reverse();

        string s = "Path: ";
        foreach(var cell in path)
        {
            s += $"({cell.x},{cell.y}),";
        }
        Debug.Log(s);

        return path;
    }

    private IEnumerable<Vector3Int> Neighbors(Vector3Int cellPos)
    {
        foreach (var pos in relativeNeighbors)
        {
            var newCellPos = cellPos + pos;
            if (gameGrid.InGridBounds(newCellPos) && !gameGrid.ExistsAtCell(newCellPos))
            {
                yield return newCellPos;
            }
        }

        yield break;
    }

    public IList<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        Debug.Log($"GridPathfinder.FindPath {start} -> {goal}");

        var openSet = new PriorityQueue<Vector3Int>();

        var cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        var gScore = new Dictionary<Vector3Int, float>();
        gScore[start] = 0f;

        var fScore = new Dictionary<Vector3Int, float>();
        fScore[start] = Heuristic(start);

        openSet.Enqueue(start, fScore[start]);

        while (!openSet.IsEmpty())
        {
            //Debug.Log($"openSet count: {openSet.Count}");

            var current = openSet.Dequeue();
            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }
            
            foreach (var neighbor in Neighbors(current))
            {
                var tentative_gScore = gScore.GetValueOrDefault(current, float.PositiveInfinity) + Weight(current, neighbor);
                if (tentative_gScore < gScore.GetValueOrDefault(neighbor, float.PositiveInfinity))
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentative_gScore;
                    fScore[neighbor] = tentative_gScore + Heuristic(neighbor);
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore.GetValueOrDefault(neighbor, float.PositiveInfinity));
                    } else
                    {
                        openSet.Replace(neighbor, fScore.GetValueOrDefault(neighbor, float.PositiveInfinity));
                    }
                }
            }
        }

        Debug.LogError($"Path not found for {start} -> {goal}");
        return null;
    }
}
