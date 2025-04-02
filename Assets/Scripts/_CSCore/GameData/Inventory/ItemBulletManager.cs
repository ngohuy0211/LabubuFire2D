using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ItemBulletManager
{
    private InventoryRepository<ItemBullet> _itemRepository;

    public ItemBulletManager(InventoryRepository<ItemBullet> itemRepository)
    {
        _itemRepository = itemRepository;
    }
    
    public void SetItems(List<ItemBullet> item)
    {
        Clear();
        AddItems(item);
    }

    public void AddItems(List<ItemBullet> items)
    {
        _itemRepository.AddItems(items);
    }

    public void AddItem(ItemBullet item)
    {
        _itemRepository.AddItem(item);
    }
    
    [CanBeNull]
    public ItemBullet GetItemByKey(int itemKey)
    {
        return _itemRepository.Get(item => item.ItemKey == itemKey);
    }

    public List<ItemBullet> GetAllItems()
    {
        return _itemRepository.GetAllItems();
    }

    public void Clear()
    {
        _itemRepository.Clear();
    }
}