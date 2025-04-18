using System.Collections.Generic;
using ChiuChiuCSCore;
using UnityEngine;
using UnityEngine.Serialization;

public class GameObjectPool : MonoBehaviour
{
    private List<GameObject> _pooledObjects = new List<GameObject>();
    [Header("Not need assign")]
    public GameObject objectPrefab;
    
    [Header("Object instantiate first")]
    public int poolSize = 5;
    
    public void SetPrefabPool(GameObject obj)
    {
        objectPrefab = obj;
        if (_pooledObjects.Count < poolSize)
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject go = Instantiate(objectPrefab, this.transform);
                go.SetActive(false);
                _pooledObjects.Add(go);
            }
        }
    }

    public GameObject GetPooledObject(object data)
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                IObjectPoolable poolable = _pooledObjects[i].GetComponent<IObjectPoolable>();
                if (poolable != null)
                    poolable.OnObjectSpawn(data, this);
                _pooledObjects[i].SetActive(true);
                return _pooledObjects[i];
            }
        }

        GameObject obj = Instantiate(objectPrefab, this.transform);
        obj.SetActive(false);
        _pooledObjects.Add(obj);

        IObjectPoolable newPoolable = obj.GetComponent<IObjectPoolable>();
        if (newPoolable != null)
        {
            newPoolable.OnObjectSpawn(data, this);
        }

        obj.SetActive(true);
        return obj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {
        obj.transform.position = Vector3.zero;
        obj.SetActive(false);
    }
    
    public void ReturnObjectUIToPool(GameObject obj)
    {
        obj.transform.localPosition = Vector3.zero;
        obj.SetActive(false);
    }
}