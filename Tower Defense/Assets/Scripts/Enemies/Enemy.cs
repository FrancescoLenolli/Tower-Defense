using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2.0f;
    public float health = 2.0f;

    private PathFollower pathFollower;
    private HealthComponent healthComponent;
    private Action<Enemy> onDeath;

    public Action<Enemy> OnDeath { get => onDeath; set => onDeath = value; }

    public void Init(List<Node> path)
    {
        pathFollower = gameObject.AddComponent<PathFollower>();
        pathFollower.OnEndTargetReached += Die;

        healthComponent = gameObject.AddComponent<HealthComponent>();
        healthComponent.SetValue(health);
        healthComponent.OnHealthDepleted += Die;

        StartMoving(path);
    }

    private void StartMoving(List<Node> path)
    {
        transform.position = path[0].worldPosition;
        pathFollower.Move(path, speed);
    }

    private void Die()
    {
        onDeath?.Invoke(this);
        Destroy(gameObject);
    }
}
