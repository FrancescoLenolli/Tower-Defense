using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy prefabEnemy = null;
    public float spawnTimer = 1.0f;

    private List<Node> path = new List<Node>();
    private float currentTimer;

    private void Awake()
    {
        currentTimer = spawnTimer;
    }

    private void Update()
    {
        if (path == null)
            return;

        currentTimer -= Time.deltaTime;
        if(currentTimer <= 0.0f)
        {
            currentTimer = spawnTimer;
            SpawnEnemy();
        }
    }

    public void SetPath(List<Node> path)
    {
        this.path = path;
    }

    private void SpawnEnemy()
    {
        Instantiate(prefabEnemy).StartMoving(path);
    }
}
