﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public delegate float GridPathfinderHeuristic(Vector3Int cellPos, Vector3Int target);
public delegate float GridPathfinderWeight(Vector3Int current, Vector3Int neighbor);
public delegate bool GridPathfinderIsValidNeighbor(Vector3Int cellPos);


public class GridPathfinder
{
    public GridPathfinderHeuristic Heuristic { get; set; }
    public GridPathfinderWeight Weight { get; set; }
    public GridPathfinderIsValidNeighbor IsValidNeighbor { get; set; }

    private List<Vector3Int> _relativeNeighbors = new();

    public GridPathfinder()
    {
        _relativeNeighbors.Add(new Vector3Int(1, 0));
        _relativeNeighbors.Add(new Vector3Int(0, 1));
        _relativeNeighbors.Add(new Vector3Int(-1, 0));
        _relativeNeighbors.Add(new Vector3Int(0, -1));
    }

    private IList<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        Debug.Log($"Reconstructing path: {current}, cameFrom: {cameFrom.Count}");

        var path = new List<Vector3Int>
        {
            current
        };

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
        foreach (var pos in _relativeNeighbors)
        {
            yield return cellPos + pos;
        }

        yield break;
    }

    public IList<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {
        Debug.Log($"GridPathfinder.FindPath {start} -> {goal}");

        var openSet = new PriorityQueue<Vector3Int>();

        var cameFrom = new Dictionary<Vector3Int, Vector3Int>();

        var gScore = new Dictionary<Vector3Int, float>
        {
            [start] = 0f
        };

        var fScore = new Dictionary<Vector3Int, float>
        {
            [start] = Heuristic(start, goal)
        };

        openSet.Enqueue(start, fScore[start]);

        while (!openSet.IsEmpty())
        {
            var current = openSet.Dequeue();
            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }
            
            foreach (var neighbor in Neighbors(current))
            {
                if (neighbor != goal && !IsValidNeighbor(neighbor))
                {
                    continue;
                }

                var tentative_gScore = gScore.GetValueOrDefault(current, float.PositiveInfinity) + Weight(current, neighbor);
                if (tentative_gScore < gScore.GetValueOrDefault(neighbor, float.PositiveInfinity))
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentative_gScore;
                    fScore[neighbor] = tentative_gScore + Heuristic(neighbor, goal);
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
