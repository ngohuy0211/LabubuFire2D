using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ItemModel
{
    public int ItemKey { get; set; } = -1;
    public string ItemName { get; set; } = "";
    public string ItemDesc { get; set; } = "";
    public long ItemNumber { get; set; } = -1;

    public abstract void UseItem(System.Action act);

    public virtual ItemModel Clone()
    {
        object obj = this.MemberwiseClone();
        return (ItemModel) obj;
    }

    public override string ToString()
    {
        return  " --- Item Model Key: " + ItemKey + " -- name : " + ItemName +
               " m_Number : " + ItemNumber;
    }
}