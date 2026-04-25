using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    [Header("Spawn Settings")]
    public float spawnDelay = 3f;
    public Transform spawnPoint;

    private float timer;

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnEnemy();
            timer = spawnDelay;
        }
    }

    void SpawnEnemy()
    {
        int rand = Random.Range(0, 3);

        GameObject enemyToSpawn = null;

        if (rand == 0)
            enemyToSpawn = enemy1;
        else if (rand == 1)
            enemyToSpawn = enemy2;
        else
            enemyToSpawn = enemy3;

        if (enemyToSpawn != null)
        {
            Instantiate(
                enemyToSpawn,
                spawnPoint.position,
                Quaternion.identity
            );
            Vector3 pos = spawnPoint.position;

            if (enemyToSpawn == enemy3)
                pos.y += 1.0f;
        }
    }
}