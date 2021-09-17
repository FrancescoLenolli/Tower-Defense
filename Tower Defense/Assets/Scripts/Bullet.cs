﻿using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 1.0f;
    public float speed = 2.0f;

    private new Rigidbody rigidbody;

    public void Fire(Vector3 direction)
    {
        rigidbody = GetComponent<Rigidbody>();
        float rotationY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90.0f, rotationY, 0.0f);
        rigidbody.velocity = speed * direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        HealthComponent healthComponent = collision.collider.GetComponent<HealthComponent>();

        if (healthComponent)
        {
            healthComponent.Damage(damage);
        }
    }
}
