using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : Singleton<PlayerInventory>
{
    public ItemAvatarManager ItemAvatarManager { get; private set; }
    public ItemBulletManager ItemBulletManager { get; private set; }
    public ItemCharacterManager ItemCharacterManager { get; private set; }

    public PlayerInventory()
    {
        var avatarRepository = new InventoryRepository<ItemAvatar>();
        ItemAvatarManager = new ItemAvatarManager(avatarRepository);
        
        var charRepository = new InventoryRepository<CharacterModel>();
        ItemCharacterManager = new ItemCharacterManager(charRepository);
        
        var bulletRepository = new InventoryRepository<ItemBullet>();
        ItemBulletManager = new ItemBulletManager(bulletRepository);
    }

    public void ClearInventory()
    {
        ItemAvatarManager.Clear();
        ItemBulletManager.Clear();
        ItemCharacterManager.Clear();
    }
    
}