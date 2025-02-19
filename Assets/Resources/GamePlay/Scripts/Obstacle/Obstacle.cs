using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    protected float speed;
    
    public void Initialize(float speed)
    {
        this.speed = speed;
    }
    
    void Update()
    {
        Move();
        CheckBounds();
    }
    
    protected virtual void Move()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
    
    protected virtual void CheckBounds()
    {
        if (transform.position.y < -6)
        {
            Destroy(gameObject);
        }
    }
}