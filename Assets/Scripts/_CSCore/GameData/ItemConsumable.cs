using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemConsumable : ItemModel
{
    public int ShowType { get; set; }
    public int Type { get; set; }
    public List<int> Previews { get; set; } = new List<int>();
    public List<int> HowToGet { get; set; } = new List<int>();
    public bool Selected { get; set; }= false;

    public ItemConsumable(bool selected = false)
    {
        this.Selected = selected;
    }


    public override void UseItem(Action act)
    {
        act?.Invoke();
    }

    public override ItemModel Clone()
    {
        return (ItemConsumable)this.MemberwiseClone();
    }
}
