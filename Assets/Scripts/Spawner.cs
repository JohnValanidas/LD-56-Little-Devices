using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public GameObject prefabToSpawn;
    public GameObject target;
    public Transform spawnPoint;
    public float spawnInterval = 2f;

    public event Action<IList<Vector3Int>> OnTargetChanged;
    public event Action OnSpawnerDestroyed;

    private GameGrid _gameGrid;
    private TargetGridPath _targetGridPath;
    private IList<Vector3Int> _path;

    // Start is called before the first frame update
    void Start()
    {
        _gameGrid = FindObjectOfType<GameGrid>();
        Debug.Assert(_gameGrid != null, "GameGrid NOT FOUND");

        _targetGridPath = GetComponent<TargetGridPath>();

        _gameGrid.OnRecalcPath += UpdateTargetPathLine;

        var targetHealth = target.GetComponent<Health>();
        if (targetHealth)
        {
            targetHealth.OnUnitDestroyed += TargetDestroyed;
        }

        UpdateTargetPathLine();

        StartCoroutine(SpawnPrefab());
    }

    void OnDestroy()
    {
        _gameGrid.OnRecalcPath -= UpdateTargetPathLine;
        StopAllCoroutines();
        OnSpawnerDestroyed?.Invoke();
    }

    IEnumerator SpawnPrefab()
    {
        while (true)
        {
            if (_path == null || _path.Count == 0)
            {
                yield return new WaitForSeconds(spawnInterval);
                continue;
            }

            var prefab = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

            if (prefab.TryGetComponent<Pathfinding>(out var pathfinding))
            {
                pathfinding.gameGrid = _gameGrid;
                pathfinding.path = _path;
                pathfinding.spawner = this;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void UpdateTargetPathLine()
    {
        if (target == null)
        {
            _path = null;
        }
        else
        {
            var targetPosition = target.transform.position;
            var start = _gameGrid.WorldTocell(spawnPoint.position);
            var goal = _gameGrid.WorldTocell(targetPosition);
            var path = _gameGrid.FindPath(start, goal);

            if (_targetGridPath != null)
            {
                _targetGridPath.path = path;
            }
            _path = path;
        }

        if (_targetGridPath != null)
        {
            _targetGridPath.path = _path;
        }

        OnTargetChanged?.Invoke(_path);
    }

    private void TargetDestroyed()
    {
        target = null;
        UpdateTargetPathLine();
    }
}
