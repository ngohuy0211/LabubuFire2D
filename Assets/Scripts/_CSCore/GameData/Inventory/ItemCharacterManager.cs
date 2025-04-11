using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class ItemCharacterManager
{
    private InventoryRepository<CharacterModel> _itemRepository;

    public ItemCharacterManager(InventoryRepository<CharacterModel> itemRepository)
    {
        _itemRepository = itemRepository;
    }
    
    public void SetItems(List<CharacterModel> item)
    {
        Clear();
        AddItems(item);
    }

    public void AddItems(List<CharacterModel> items)
    {
        _itemRepository.AddItems(items);
    }

    public void AddItem(CharacterModel item)
    {
        _itemRepository.AddItem(item);
    }
    
    [CanBeNull]
    public CharacterModel GetItemByKey(int itemKey)
    {
        return _itemRepository.Get(item => item.ItemKey == itemKey);
    }

    public List<CharacterModel> GetAllItems()
    {
        return _itemRepository.GetAllItems();
    }

    public List<int> GetListItemKey()
    {
        return _itemRepository.GetAllItems().Select(character => character.ItemKey).ToList();
    }
    
    public void Clear()
    {
        _itemRepository.Clear();
    }
}