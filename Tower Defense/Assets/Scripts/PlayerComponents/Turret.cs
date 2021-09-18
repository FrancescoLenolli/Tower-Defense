using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Turret : PlaceableObject
{
    public Transform turret;
    public Transform muzzle;
    public Bullet prefabBullet;
    public float range = 1.0f;
    public float fireRate = 1.0f;

    private Transform target = null;
    private EnemySpawner enemySpawner = null;
    private bool canShoot = false;
    private float currentTimer = 0.0f;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void Update()
    {
        if (!canShoot)
            return;

        if (!target)
        {
            GetTargets();
            return;
        }

        if (!(Vector3.Distance(transform.position, target.transform.position) <= range))
        {
            target = null;
        }
        else
        {
            turret.LookAt(target.transform);
            turret.rotation = Quaternion.Euler(0, turret.rotation.eulerAngles.y, 0);

            currentTimer -= Time.deltaTime;
            if(currentTimer <= 0.0f)
            {
                currentTimer = fireRate;
                FireBullet();
            }
        }
    }

    public override void Place()
    {
        canShoot = true;
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    private void GetTargets()
    {
        List<Enemy> targets = enemySpawner.Enemies.Where(enemy => Vector3.Distance(transform.position, enemy.transform.position) <= range).ToList();
        targets.OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position));

        if (target == null && targets.Count > 0)
        {
            target = targets[0].transform;
        }
    }

    private void FireBullet()
    {
        if (!target)
            return;

        Vector3 direction = target.transform.position - muzzle.position;
        Instantiate(prefabBullet, muzzle.position, prefabBullet.transform.rotation, transform).Fire(direction);
    }
}
