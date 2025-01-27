using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ItemConsumableManager
{
    private InventoryRepository<ItemConsumable> _itemRepository;

    public ItemConsumableManager(InventoryRepository<ItemConsumable> itemRepository)
    {
        _itemRepository = itemRepository;
    }
    
    public void Clear()
    {
        _itemRepository.Clear();
    }

    #region SET
    
    public void SetItems(List<ItemConsumable> heroes)
    {
        Clear();
        AddItems(heroes);
    }
    
    public void AddItems(List<ItemConsumable> items)
    {
        _itemRepository.AddItems(items);
    }

    public void AddItem(ItemConsumable item)
    {
        _itemRepository.AddItem(item);
    }

    public void AddItem(int itemKey, long numberCurrent)
    {
        bool found = false;
        foreach (ItemConsumable ic in _itemRepository.GetAllItems())
        {
            if (ic.ItemKey == itemKey)
            {
                found = true;
                ic.ItemNumber = numberCurrent;
                break;
            }
        }

        if (!found)
        {
            ItemConsumable item = DbManager.GetInstance().GetItemConsumableCopy(itemKey);
            if (item != null)
            {
                item.ItemNumber = numberCurrent;
                AddItem(item);
            }
        }
    }
    
    public void AddItemNumber(int itemKey, long numberAdd)
    {
        bool found = false;
        foreach (ItemConsumable ic in _itemRepository.GetAllItems())
        {
            if (ic.ItemKey == itemKey)
            {
                found = true;
                ic.ItemNumber += numberAdd;
                break;
            }
        }

        if (!found)
        {
            ItemConsumable item = DbManager.GetInstance().GetItemConsumableCopy(itemKey);
            if (item != null)
            {
                item.ItemNumber = numberAdd;
                AddItem(item);
            }
        }
    }

    #endregion

    #region GET

    [CanBeNull]
    public ItemConsumable GetItemByKey(int itemKey)
    {
        return _itemRepository.Get(item => item.ItemKey == itemKey);
    }

    public List<ItemConsumable> GetAllItems()
    {
        return _itemRepository.GetAllItems();
    }
    
    public long GetNumberItemConsumable(int itemKey)
    {
        var item = _itemRepository.Get(i => i.ItemKey == itemKey);
        return item?.ItemNumber ?? 0;
    }

    #endregion

    #region REMOVE

    public void RemoveItemByKey(long key)
    {
        int count = GetAllItems().Count;
        for (int i = count - 1; i >= 0; i--)
        {
            ItemConsumable item = GetAllItems()[i];
            if (key == item.ItemKey)
            {
                RemoveItemAt(i);
                break;
            }
        }
    }
    
    public void RemoveItem(ItemConsumable item)
    {
        _itemRepository.RemoveItem(item);
    }
    
    public void RemoveItemAt(int index)
    {
        _itemRepository.RemoveAt(index);
    }

    #endregion

    #region OTHER ACTION

    public void SortItems(Comparison<ItemConsumable> comparison)
    {
        _itemRepository.Sort(comparison);
    }

    #endregion
}
