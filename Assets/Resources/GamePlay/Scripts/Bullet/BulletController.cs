using System;
using System.Collections;
using System.Collections.Generic;
using ChiuChiuCSCore;
using UnityEngine;

public class BulletController : BattleItemSpawnController
{
    #region Properties

    private Vector2 _direction;
    private const float SpeedFly = 10f;
    private ItemBullet _itemBullet;
    private Bullet _bulletObject;

    #endregion

    #region Override

    protected override void Awake()
    {
        base.Awake();
        _bulletObject = this.gameObject.GetComponent<Bullet>();
    }
    
    protected override void SetData(object data)
    {
        // _itemBullet = PlayerInventory.Instance.ItemBulletManager.GetItemByKey(GameContext.Instance.UserModel.bulletUsingId);
        //Test
        _itemBullet = DbManager.GetInstance().GetItemBulletCopy(1);
        //Test
        _bulletObject.SetData(_itemBullet);
        //
        if (data is Vector2 direction)
            _direction = direction.normalized;
    }

    #endregion

    private void Update()
    {
        transform.Translate(_direction * SpeedFly * Time.deltaTime);
        CheckScreenLimit();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out ItemDropController item))
        {
            item.TakeDame(_itemBullet.Damage);
        }
    }
}
