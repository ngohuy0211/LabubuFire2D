using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFactory : MonoBehaviour
{
    public GameObject basicObstaclePrefab;
    public GameObject fastObstaclePrefab;

    public Obstacle CreateObstacle(Vector2 position, string type)
    {
        GameObject obstacleObj = null;
        
        switch (type)
        {
            case "Basic":
                obstacleObj = Instantiate(basicObstaclePrefab, position, Quaternion.identity);
                break;
            case "Fast":
                obstacleObj = Instantiate(fastObstaclePrefab, position, Quaternion.identity);
                break;
        }
        
        Obstacle obstacle = obstacleObj.GetComponent<Obstacle>();
        obstacle.Initialize(2f);
        return obstacle;
    }
}