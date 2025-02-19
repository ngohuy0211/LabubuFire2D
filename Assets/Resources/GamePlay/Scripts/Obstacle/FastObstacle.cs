using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastObstacle : Obstacle
{
    protected override void Move()
    {
        transform.Translate(Vector2.down * (speed * 1.5f) * Time.deltaTime);
    }
}
