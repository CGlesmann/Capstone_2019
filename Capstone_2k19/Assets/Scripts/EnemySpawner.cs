using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Variables")]
    public bool areaComplete = false;
    [SerializeField] private float spawnDelay = 0f;
    [SerializeField] private Vector3 originPoint = Vector3.zero;
    [SerializeField] private Vector3 minOffset = Vector3.zero;
    [SerializeField] private Vector3 maxOffset = Vector3.zero;

    [Header("Enemy Settings")]
    [SerializeField] private GameObject[] enemiesToSpawn = null;

    private GameObject[] spawnedEnemies = null;
    private bool enemiesSpawned = false;

    /// <summary>
    /// Set Initial Variables
    /// </summary>
    private void Start() { enemiesSpawned = false; }

    private void Update()
    {
        // Check for Area Completion
        if (enemiesSpawned)
        {
            // Check each reference to make sure the enemy is dead
            foreach (GameObject enemy in spawnedEnemies)
                if (enemy != null)
                    return;

            // All enemies are dead, area complete
            areaComplete = true;
        }
    }

    /// <summary>
    /// Begin Enemy spawn loop
    /// </summary>
    public void StartSpawnEnemies() {
        if (!areaComplete && !enemiesSpawned)
            StartCoroutine("SpawnEnemies");
    }

    /// <summary>
    /// Spawning Loop
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnEnemies()
    {
        // Initialize the spawnedEnemies array
        int counter = 0;
        spawnedEnemies = new GameObject[enemiesToSpawn.Length];

        foreach (GameObject enemy in enemiesToSpawn)
        {
            // Get the point the spawn the enemy
            Vector3 point = originPoint + new Vector3(Random.Range(minOffset.x, maxOffset.x),
                                                      Random.Range(minOffset.y, maxOffset.y),
                                                      Random.Range(minOffset.z, maxOffset.z));

            // Instantiate the clone of an enemy
            GameObject newEnemy = Instantiate(enemy, point, Quaternion.Euler(Vector3.zero));
            spawnedEnemies[counter++] = newEnemy;

            // Setting the State of the Enemy
            newEnemy.GetComponent<EnemyMovement>().SetChaseState();

            // Add an artifical delay
            yield return new WaitForSeconds(0.1f);
        }

        // Set spawned to True
        enemiesSpawned = true;
    }

    // Draw the spawn area
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(originPoint, new Vector3(maxOffset.x - minOffset.x, 1f, maxOffset.z - minOffset.z));
    }
}
