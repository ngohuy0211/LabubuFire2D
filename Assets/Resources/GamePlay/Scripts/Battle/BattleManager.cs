using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private Transform tfSpawnPlayer;
    [SerializeField] private GameObject prefabPlayer;
    
    private PlayerController _playerController;
    private MapModel _currMap;
    private float spawnInterval = 2f;
    private float timer;

    private void Awake()
    {
        SetMapData(GameContext.Instance.CurrMapSelect);
    }

    private void SetMapData(MapModel currMap)
    {
        _currMap = currMap;
        //
        SpawnChar();
    }

    private void SpawnChar()
    {
        GameObject goChar = Instantiate(prefabPlayer, tfSpawnPlayer, false);
        _playerController = goChar.GetComponent<PlayerController>();
        
    }

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
    }
}
