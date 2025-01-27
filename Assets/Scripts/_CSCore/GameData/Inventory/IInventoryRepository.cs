using System;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryRepository<T>
{
    void AddItems(List<T> items);
    void AddItem(T item);
    void RemoveItem(T item);
    bool RemoveAt(int index);
    T Get(Predicate<T> predicate);
    List<T> GetAllItems();
    void Sort(Comparison<T> comparison);
    void Clear();
}
