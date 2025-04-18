using System;
using System.Collections;
using System.Collections.Generic;
using ChiuChiuCSCore;
using UnityEngine;

public abstract class BattleItemSpawnController : MonoBehaviour, IObjectPoolable
{
    private Camera _mainCamera;
    private GameObjectPool _poolMachine;

    protected virtual void Awake()
    {
        _mainCamera = Camera.main;
    }

    protected abstract void SetData(object item);

    public virtual void TakeDame(int dame)
    {
        
    }

    protected virtual void Die()
    {
        _poolMachine.ReturnObjectToPool(this.gameObject);
    }
    
    protected void CheckScreenLimit()
    {
        Vector3 viewportPos = _mainCamera.WorldToViewportPoint(this.transform.position);

        if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1)
            _poolMachine.ReturnObjectToPool(this.gameObject);
    }

    public void OnObjectSpawn(object data, GameObjectPool poolMachine)
    {
        _poolMachine = poolMachine;
        SetData(data);
    }
}
