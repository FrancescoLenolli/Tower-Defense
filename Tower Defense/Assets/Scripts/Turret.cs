using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Turret : PlaceableObject
{
    public float range = 1.0f;

    private Enemy target = null;
    private bool canShoot = false;
    private List<Enemy> targets = new List<Enemy>();

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

        if (Vector3.Distance(transform.position, target.transform.position) <= range)
            transform.LookAt(target.transform);
        else
            target = null;

        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    public override void Place()
    {
        canShoot = true;
    }

    private void GetTargets()
    {
        targets.Clear();
        targets = FindObjectsOfType<Enemy>().Where(enemy => Vector3.Distance(transform.position, enemy.transform.position) <= range).ToList();
        targets.OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position));

        if (target == null && targets.Count > 0)
        {
            target = targets[0];
        }
    }
}
