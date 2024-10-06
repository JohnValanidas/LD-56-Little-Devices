using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public GameObject prefabToSpawn;
    public Transform spawnPoint;
    public float spawnInterval = 2f;
    public GameObject target;

    public event Action<IList<Vector3Int>> OnTargetChanged;
    public event Action OnSpawnerDestroyed;

    private GameGrid _gameGrid;
    private TargetGridPath _targetGridPath;
    private IList<Vector3Int> _path;
    private GameObject _target;

    // Start is called before the first frame update
    void Start()
    {
        _gameGrid = FindObjectOfType<GameGrid>();
        Debug.Assert(_gameGrid != null, "GameGrid NOT FOUND");

        _targetGridPath = GetComponent<TargetGridPath>();

        _gameGrid.OnRecalcPath += UpdateTargetPathLine;


        UpdateTarget(_target);

        StartCoroutine(SpawnPrefab());
    }

    private void Update()
    {
        if (_target != target)
        {
            UpdateTarget(target);
        }
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
        if (_target == null)
        {
            _path = null;
        }
        else
        {
            var targetPosition = _target.transform.position;
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
        _target = null;
        UpdateTargetPathLine();
    }

    private void UpdateTarget(GameObject newTarget)
    {
        if (_target)
        {
            var targetHealth = _target.GetComponent<Health>();
            if (targetHealth)
            {
                targetHealth.OnUnitDestroyed -= TargetDestroyed;
            }
        }

        _target = newTarget;
        if (_target)
        {
            var targetHealth = _target.GetComponent<Health>();
            if (targetHealth)
            {
                targetHealth.OnUnitDestroyed += TargetDestroyed;
            }
        }

        UpdateTargetPathLine();
    }
}
