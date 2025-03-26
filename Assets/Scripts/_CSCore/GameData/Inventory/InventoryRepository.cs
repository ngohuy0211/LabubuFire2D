using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryRepository<T> : IInventoryRepository<T>
{
    private readonly List<T> _items = new List<T>();

    public void AddItems(List<T> items)
    {
        _items.AddRange(items);
    }

    public void AddItem(T item)
    {
        _items.Add(item);
    }

    public void RemoveItem(T item)
    {
        _items.Remove(item);
    }
    
    public bool RemoveAt(int index)
    {
        if (index < 0 || index >= _items.Count)
        {
            Debug.LogError("Index is out of range: " + index);
        }
        _items.RemoveAt(index);
        return true;
    }

    public T Get(Predicate<T> predicate)
    {
        return _items.Find(predicate);
    }

    public List<T> GetAllItems()
    {
        return _items;
    }

    public void Sort(Comparison<T> comparison)
    {
        _items.Sort(comparison);
    }

    public void Clear()
    {
        _items.Clear();
    }
}
