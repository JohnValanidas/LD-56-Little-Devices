using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridPathfinder : MonoBehaviour
{
    public GameGrid gameGrid;

    private List<Vector3Int> relativeNeighbors = new List<Vector3Int>();

    GridPathfinder()
    {
        relativeNeighbors.Add(new Vector3Int(1, 0));
        relativeNeighbors.Add(new Vector3Int(0, 1));
        relativeNeighbors.Add(new Vector3Int(-1, 0));
        relativeNeighbors.Add(new Vector3Int(0, -1));
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
        return new List<Vector3Int>();
    }

    private IEnumerable<Vector3Int> Neighbors(Vector3Int cellPos)
    {
        foreach (var pos in relativeNeighbors)
        {
            var newCellPos = cellPos + pos;
            if (gameGrid.InGridBounds(newCellPos))
            {
                yield return newCellPos;
            }
        }

        yield break;
    }

    public IList<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        var cells = new List<Vector3Int>();

        var openSet = new PriorityQueue<Vector3Int>();

        var cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        var gScore = new Dictionary<Vector3Int, float>();
        gScore[start] = 0f;

        var fScore = new Dictionary<Vector3Int, float>();
        fScore[start] = Heuristic(start);

        while (!openSet.IsEmpty())
        {
            var current = openSet.Dequeue();
            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }
            
            foreach (var neighbor in Neighbors(current))
            {
                var tentative_gScore = gScore[current] + Weight(current, neighbor);
                if (tentative_gScore < gScore.GetValueOrDefault(neighbor, float.PositiveInfinity))
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentative_gScore;
                    fScore[neighbor] = tentative_gScore + Heuristic(neighbor);
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore.GetValueOrDefault(neighbor, float.PositiveInfinity));
                    }
                }
            }
        }

        return null;
    }
}
