using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ItemAvatarManager
{
    private InventoryRepository<ItemAvatar> _itemRepository;

    public ItemAvatarManager(InventoryRepository<ItemAvatar> itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public void SetItems(List<ItemAvatar> item)
    {
        Clear();
        AddItems(item);
    }

    public void AddItems(List<ItemAvatar> items)
    {
        _itemRepository.AddItems(items);
    }

    public void AddItem(ItemAvatar item)
    {
        _itemRepository.AddItem(item);
    }
    
    [CanBeNull]
    public ItemAvatar GetItemByKey(int itemKey)
    {
        return _itemRepository.Get(item => item.ItemKey == itemKey);
    }

    public List<ItemAvatar> GetAllItems()
    {
        return _itemRepository.GetAllItems();
    }
    
    public void Clear()
    {
        _itemRepository.Clear();
    }
}