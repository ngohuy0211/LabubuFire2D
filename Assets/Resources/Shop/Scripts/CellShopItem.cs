using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellShopItem : MonoBehaviour
{
    #region Define

    [SerializeField] private SpineObject spineObject;
    [SerializeField] private Image imgPrice;
    [SerializeField] private Text textPrice;
    [SerializeField] private Image imgAvatar;
    [SerializeField] private Button buttonBuy;
    [SerializeField] private GameObject goBought;
    
    #endregion

    #region Properties

    private object _data;
    private System.Action<object> _cbBuy;
    
    #endregion

    #region Core MonoBehavior

    private void Awake()
    {
        UIHelper.AddButtonClickNormal(buttonBuy, OnClickBuy);
    }

    #endregion

    #region Public Method

    public void SetData(object data)
    {
        _data = data;
        imgAvatar.gameObject.SetActive(data is ItemAvatar);
        bool isOwn = false;
        ItemConsumable itemPrice = new ItemConsumable();
        if (data is CharacterModel charModel)
        {
            SetSpine("_Common/Characters/" + charModel.ItemKey + "/" + charModel.ItemKey);
            isOwn = PlayerInventory.Instance.ItemCharacterManager.GetListItemKey().Contains(charModel.ItemKey);
            itemPrice = charModel.Price;
        }
        else if (data is ItemBullet bullet)
        {
            SetSpine("_Common/ItemBullet/" + bullet.ItemKey + "/" + bullet.ItemKey);
            isOwn = PlayerInventory.Instance.ItemBulletManager.GetListItemKey().Contains(bullet.ItemKey);
            itemPrice = bullet.Price;
        }
        else if (data is ItemAvatar avatar)
        {
            spineObject.HideObject();
            imgAvatar.sprite = Resources.Load<Sprite>("_Common/ItemAvatar/" + avatar.ItemKey);
            isOwn = PlayerInventory.Instance.ItemAvatarManager.GetListItemKey().Contains(avatar.ItemKey);
            itemPrice = avatar.Price;
        }
        imgPrice.sprite = Resources.Load<Sprite>("_Common/ItemConsumable/" + itemPrice.ItemKey);
        textPrice.text = itemPrice.ItemNumber.ToString();
        //
        buttonBuy.gameObject.SetActive(!isOwn);
        goBought.SetActive(isOwn);
    }
    
    public void SetCbBuy(System.Action<object> cb)
    {
        _cbBuy = cb;
    }
    
    #endregion

    #region Private Method

    private void SetSpine(string path)
    {
        spineObject.ShowObject();
        spineObject.LoadSpine(path);
        spineObject.PlayAnimationLoop("idle");
    }

    private void OnClickBuy()
    {
        _cbBuy?.Invoke(_data);
    }
    
    #endregion
}
