using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : ItemModel
{
    public int Hp { get; set; }
    public float Speed { get; set; }
    public int Value { get; set; }
    public ItemQuality Quality { get; set; }
    public int Damage { get; set; }
    public ItemDropType Type { get; set; }
    public bool Selected { get; set; }= false;

    public ItemDrop(bool selected = false)
    {
        this.Selected = selected;
    }


    public override void UseItem(Action act)
    {
        act?.Invoke();
    }

    public override ItemModel Clone()
    {
        return (ItemDrop)this.MemberwiseClone();
    }
}
