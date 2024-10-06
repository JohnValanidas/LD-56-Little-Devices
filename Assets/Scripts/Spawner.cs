using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public Transform spawnPoint;

    public float spawnInterval = 2f;

    public GameObject prefabToSpawn;

    public Transform target;
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(SpawnPrefab());
    }

    IEnumerator SpawnPrefab()
    {
        while (true)
        {
            var targetPosition = target?.position;
            if (!targetPosition.HasValue)
            {
                yield return new WaitForSeconds(spawnInterval);
            }

            var gameGrid = FindObjectOfType<GameGrid>();
            Debug.Assert(gameGrid != null, "GameGrid NOT FOUND");

            var start = gameGrid.WorldTocell(spawnPoint.position);
            var goal = gameGrid.WorldTocell(targetPosition.Value);

            var path = gameGrid.FindPath(start, goal);
            if (path != null)
            {
                GameObject prefab = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
                Pathfinding pathfinding = prefab.GetComponent<Pathfinding>();
                if (pathfinding != null)
                {
                    pathfinding.target = target;
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
