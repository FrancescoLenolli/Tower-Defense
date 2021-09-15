﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    private float speed;
    private List<Node> path;

    public void StartMoving(List<Node> path, float speed)
    {
        this.path = path;
        this.speed = speed;
        transform.position = path[0].worldPosition;
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        if (path != null)
        {
            int currentPathIndex = 1;
            Vector3 targetPosition = path[currentPathIndex].worldPosition;

            while (currentPathIndex < path.Count)
            {
                Vector3 startingPosition = transform.position;
                targetPosition = path[currentPathIndex].worldPosition;
                transform.LookAt(targetPosition);

                float t = 0;
                float step = speed / Vector3.Distance(startingPosition, targetPosition);
                while (t < 1)
                {
                    t += step * Time.deltaTime;
                    transform.position = Vector3.Lerp(startingPosition, targetPosition, t);

                    yield return null;
                }
                currentPathIndex++;
                transform.position = targetPosition;
                if(currentPathIndex == path.Count)
                {
                    Destroy(gameObject);
                }
                yield return null;
            }

            transform.position = targetPosition;
            yield return null;
        }
        yield return null;
    }
}
