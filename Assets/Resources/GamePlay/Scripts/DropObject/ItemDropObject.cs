using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer imgItem;
    
    public void SetData(ItemDrop item)
    {
        imgItem.sprite = ResourceHelper.LoadSprite("_Common/ItemDrop/" + item.ItemKey);
    }
}
