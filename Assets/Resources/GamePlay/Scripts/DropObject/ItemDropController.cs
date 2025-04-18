using System;
using System.Collections;
using System.Collections.Generic;
using ChiuChiuCSCore;
using UnityEngine;

public class ItemDropController : BattleItemSpawnController
{
    #region Properties

    private ItemDrop _itemDrop;
    private ItemDropObject _itemDropObject;
    private System.Action<ItemDrop> _itemDestroyCb;

    #endregion

    #region Override

    protected override void Awake()
    {
        base.Awake();
        _itemDropObject = this.gameObject.GetComponent<ItemDropObject>();
    }

    protected override void SetData(object item)
    {
        if (item is not ItemDrop itemDrop) return;
        _itemDrop = itemDrop;
        _itemDropObject.SetData(itemDrop);
    }
    
    protected override void Die()
    {
        base.Die();
        _itemDrop.Hp = 0;
    }
    
    public override void TakeDame(int dame)
    {
        _itemDrop.Hp -= dame;
        if (_itemDrop.Hp > 0) return;
        Die();
        _itemDestroyCb?.Invoke(_itemDrop);
    }

    #endregion
    

    public void SetItemDestroyCb(System.Action<ItemDrop> cb)
    {
        _itemDestroyCb = cb;
    }
    
    private void Update()
    {
        transform.Translate(Vector2.down * _itemDrop.Speed * Time.deltaTime);
        CheckScreenLimit();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            player.TakeDame(_itemDrop.Damage);
            Die();
        }
    }
}
