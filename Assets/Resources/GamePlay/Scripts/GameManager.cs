using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ObstacleFactory obstacleFactory;
    public float spawnInterval = 2f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0;
        }
    }

    void SpawnObstacle()
    {
        float randomX = Random.Range(-2.5f, 2.5f);
        Vector2 spawnPosition = new Vector2(randomX, 6);
        string obstacleType = (Random.value > 0.5f) ? "Basic" : "Fast";
        obstacleFactory.CreateObstacle(spawnPosition, obstacleType);
    }
}
