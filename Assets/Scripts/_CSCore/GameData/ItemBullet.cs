using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBullet : ItemModel
{
    public int Damage { get; set; }
    public bool IsOwn { get; set; }
    public bool IsUsing { get; set; }
    public ItemConsumable Price { get; set; }
    
    public override void UseItem(Action act)
    {
        act?.Invoke();
    }

    public override ItemModel Clone()
    {
        return (ItemBullet)this.MemberwiseClone();
    }
}
