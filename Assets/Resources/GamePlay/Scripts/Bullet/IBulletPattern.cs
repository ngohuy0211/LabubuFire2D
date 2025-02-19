using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBulletPattern
{
    void Fire(Transform firePoint, BulletFactory factory);
}
