using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy prefabEnemy = null;
    public float spawnTimer = 1.0f;

    private List<Node> path = new List<Node>();
    private List<Enemy> enemies = new List<Enemy>();
    private float currentTimer;

    public List<Enemy> Enemies { get => enemies; }

    private void Awake()
    {
        currentTimer = spawnTimer;
    }

    public void SetPath(List<Node> path)
    {
        this.path = path;
        StartCoroutine(SpawnEnemiesRoutine());
    }

    private void SpawnEnemy()
    {
        Enemy enemy = Instantiate(prefabEnemy);
        enemy.OnDeath += RemoveEnemy;
        enemy.Init(path);

        enemies.Add(enemy);
    }

    private void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            if (path == null)
                yield return null;

            currentTimer -= Time.deltaTime;
            if (currentTimer <= 0.0f)
            {
                currentTimer = spawnTimer;
                SpawnEnemy();
            }

            yield return null;
        }
    }

}
