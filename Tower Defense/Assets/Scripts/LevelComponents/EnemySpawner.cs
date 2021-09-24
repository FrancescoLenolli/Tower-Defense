using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy prefabEnemy = null;
    public float spawnTimer = 1.0f;
    public int enemyCount = 5;

    private List<Node> path = new List<Node>();
    private List<Enemy> enemies = new List<Enemy>();
    private float currentTimer;
    private int currentCount;

    public List<Enemy> Enemies { get => enemies; }

    private void Awake()
    {
        currentTimer = 0;
        currentCount = 0;
    }

    public void SetPath(List<Node> path)
    {
        this.path = path;
    }

    public void StartWave()
    {
        if (currentCount == 0)
        {
            currentTimer = spawnTimer;
            currentCount = enemyCount;
            StartCoroutine(SpawnEnemiesRoutine());
        }
    }

    private void SpawnEnemy()
    {
        Enemy enemy = Instantiate(prefabEnemy);
        enemy.OnDeath += RemoveEnemy;
        enemy.Init(path);

        enemies.Add(enemy);

        Debug.Log("Spawn Enemy");
    }

    private void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        while (currentCount > 0)
        {
            if (path == null)
                yield return null;

            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0.0f)
            {
                currentTimer = spawnTimer;
                currentCount--;
                SpawnEnemy();
            }

            yield return null;
        }

        yield return null;
    }

}
