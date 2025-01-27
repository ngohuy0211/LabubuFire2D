using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : Singleton<PlayerInventory>
{
    public ItemConsumableManager ItemConsumableManager { get; private set; }
    
    public PlayerInventory()
    {
        var itemConsumRepository = new InventoryRepository<ItemConsumable>();
        ItemConsumableManager = new ItemConsumableManager(itemConsumRepository);
    }

    public void ClearInventory()
    {
        ItemConsumableManager.Clear();
    }
    
}