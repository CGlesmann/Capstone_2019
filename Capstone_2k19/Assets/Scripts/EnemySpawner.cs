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

    public void StartSpawnEnemies() { StartCoroutine("SpawnEnemies"); }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            StartSpawnEnemies();
    }

    private IEnumerator SpawnEnemies()
    {
        foreach (GameObject enemy in enemiesToSpawn)
        {
            // Get the point the spawn the enemy
            Vector3 point = originPoint + new Vector3(Random.Range(minOffset.x, maxOffset.x),
                                                      Random.Range(minOffset.y, maxOffset.y),
                                                      Random.Range(minOffset.z, maxOffset.z));

            // Instantiate the clone of an enemy
            GameObject newEnemy = Instantiate(enemy, point, Quaternion.Euler(Vector3.zero));
            //newEnemy.transform.position = point;

            // Setting the State of the Enemy
            newEnemy.GetComponent<EnemyMovement>().SetChaseState();

            // Add an artifical delay
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Draw the spawn area
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(originPoint, new Vector3(maxOffset.x - minOffset.x, 1f, maxOffset.z - minOffset.z));
    }
}
