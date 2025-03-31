using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : ItemModel
{
    public int Hp { get; set; }
    public int SpeedFire { get; set; }
    public ItemConsumable Price { get; set; }
    public bool IsOwn { get; set; }
    public bool IsUsing { get; set; }
    
    public override void UseItem(Action act)
    {
        act?.Invoke();
    }

    public override ItemModel Clone()
    {
        return (CharacterModel)this.MemberwiseClone();
    }
}
