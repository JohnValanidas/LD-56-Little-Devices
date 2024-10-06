using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public Transform spawnPoint;

    public float spawnInterval = 2f;

    public GameObject prefabToSpawn;

    public Transform target;


    private GameGrid _gameGrid;
    private TargetGridPath _targetGridPath;
    private IList<Vector3Int> _path;

    // Start is called before the first frame update
    void Start()
    {
        _gameGrid = FindObjectOfType<GameGrid>();
        Debug.Assert(_gameGrid != null, "GameGrid NOT FOUND");

        _targetGridPath = GetComponent<TargetGridPath>();

        _gameGrid.AddPathEventListener(UpdateTargetPathLine);

        UpdateTargetPathLine();

        StartCoroutine(SpawnPrefab());
    }

    void OnDestroy()
    {
        _gameGrid.RemovePathEventListener(UpdateTargetPathLine);
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

            Debug.Log("Spawner loop!");

            var prefab = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

            Pathfinding pathfinding = prefab.GetComponent<Pathfinding>();
            if (pathfinding != null)
            {
                pathfinding.target = target.transform;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void UpdateTargetPathLine()
    {
        if (transform == null)
        {
            _path = null;
            if (_targetGridPath != null)
            {
                _targetGridPath.path = null;
            }
            return;
        }

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
}
