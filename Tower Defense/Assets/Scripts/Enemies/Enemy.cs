using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2.0f;

    private PathFollower pathFollower;

    private void Awake()
    {
        pathFollower = gameObject.AddComponent<PathFollower>();
    }

    public void StartMoving(List<Node> path)
    {
        transform.position = path[0].worldPosition;
        pathFollower.Move(path, speed);
    }


}
