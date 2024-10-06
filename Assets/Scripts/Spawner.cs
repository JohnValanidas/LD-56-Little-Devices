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
        var gameGrid = FindObjectOfType<GameGrid>();
        Debug.Assert(gameGrid != null, "GameGrid NOT FOUND");

        while (true)
        {
            if (target == null)
            {
                yield return new WaitForSeconds(spawnInterval);
                continue;
            }

            var targetPosition = target.transform.position;
            var start = gameGrid.WorldTocell(spawnPoint.position);
            var goal = gameGrid.WorldTocell(targetPosition);

            var path = gameGrid.FindPath(start, goal);
            if (path != null)
            {
                GameObject prefab = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
                Pathfinding pathfinding = prefab.GetComponent<Pathfinding>();
                if (pathfinding != null)
                {
                    pathfinding.target = target.transform;
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
