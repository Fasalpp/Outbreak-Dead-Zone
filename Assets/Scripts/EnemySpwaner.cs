using UnityEngine;

public class EnemySpwaner : MonoBehaviour
{
    public GameObject enemyPrefab;         // The enemy to spawn
    public Transform[] spawnPoints;        // Where enemies can spawn
    public float spawnInterval = 5f;       // Time between spawns
    public int maxEnemiesAlive = 50;        // Optional: cap enemies on screen

    private int currentEnemyCount = 0;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 2f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (currentEnemyCount >= maxEnemiesAlive)
            return;

        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];
        Vector3 spawnPosition = spawnPoint.position;
        RaycastHit hit;

        if (Physics.Raycast(spawnPosition + Vector3.up * 10f, Vector3.down, out hit, 20f))
        {
            spawnPosition.y = hit.point.y;
        }

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, spawnPoint.rotation);
        //GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        currentEnemyCount++;

        //// Subscribe to death if enemy script has it
        //Enemy enemyScript = enemy.GetComponent<Enemy>();
        //if (enemyScript != null)
        //{
        //    enemyScript.OnEnemyDeath += HandleEnemyDeath;
        //}
    }

    void HandleEnemyDeath()
    {
        currentEnemyCount--;
    }
}
