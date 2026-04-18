using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public float respawnTime = 3f;

    private GameObject currentEnemy;
    private bool isRespawning = false;

    void Start()
    {
        SpawnEnemy();
    }

    void Update()
    {
        if (currentEnemy == null && !isRespawning)
        {
            isRespawning = true;
            Invoke(nameof(SpawnEnemy), respawnTime);
        }
    }

    void SpawnEnemy()
    {
        currentEnemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        isRespawning = false;
    }
}