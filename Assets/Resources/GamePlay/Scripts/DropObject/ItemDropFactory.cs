using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDropFactory
{
    private GameObjectPool _poolMachine;

    public ItemDropFactory(GameObjectPool poolMachine)
    {
        _poolMachine = poolMachine;
    }

    public void CreateItemDrop(Vector3 spawnPos, ItemDrop item, System.Action<ItemDrop> cbItemDestroy)
    {
        GameObject goItemDrop = _poolMachine.GetPooledObject(item);
        goItemDrop.transform.position = spawnPos;
        goItemDrop.GetComponent<ItemDropController>().SetItemDestroyCb(cbItemDestroy);
    }
}
