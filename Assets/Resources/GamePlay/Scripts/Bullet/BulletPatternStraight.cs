using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPatternStraight : IBulletPattern
{
    public void Fire(Transform firePoint, BulletFactory factory)
    {
        factory.CreateBullet(firePoint.position, Vector2.up);
    }
}