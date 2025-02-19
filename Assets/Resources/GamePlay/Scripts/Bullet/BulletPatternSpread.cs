using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatternSpread : IBulletPattern
{
    public void Fire(Transform firePoint, BulletFactory factory)
    {
        factory.CreateBullet(firePoint.position, new Vector2(-0.2f, 1));
        factory.CreateBullet(firePoint.position, Vector2.up);
        factory.CreateBullet(firePoint.position, new Vector2(0.2f, 1));
    }
}

