using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    protected float speed;
    protected Vector2 direction;
    
    public void Initialize(float speed, Vector2 direction)
    {
        this.speed = speed;
        this.direction = direction.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
