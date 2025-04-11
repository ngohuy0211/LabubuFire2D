using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupShop : BasePopup, ITabListener
{
    #region Define

    [SerializeField] private GridPoolGroup gridPool;
    [SerializeField] private Text textTitle;
    [SerializeField] private Button buttonClose, buttonClose1;
    [SerializeField] private TabManager tabManager;
    
    #endregion

    #region Properties

    private const int TAB_CHAR = 1;
    private const int TAB_BULLET = 2;
    private const int TAB_AVATAR = 3;
    private int _currBtnTabId = 1;
    
    #endregion

    #region Core MonoBehavior

    protected override void Awake()
    {
        base.Awake();
        UIHelper.AddButtonClickClose(buttonClose, ClosePopup);
        UIHelper.AddButtonClickClose(buttonClose1, ClosePopup);
        //
        if (tabManager != null)
        {
            tabManager.m_Listener = this;
            tabManager.SetTabActive(TAB_CHAR);
        }
        //
        gridPool.howToUseCellData(delegate(GameObject go, object data)
        {
            CellShopItem cell = go.GetComponent<CellShopItem>();
            cell.SetData(data);
            cell.SetCbBuy(delegate(object o)
            {
                ItemConsumable itemCost = new ItemConsumable();
                if (o is CharacterModel)
                    itemCost = ((CharacterModel) o).Price;
                else if (o is ItemBullet)
                    itemCost = ((ItemBullet) o).Price;
                else if (o is ItemAvatar)
                    itemCost = ((ItemAvatar) o).Price;
                switch (itemCost.ItemKey)
                {
                    //
                    case (int)ItemKey.COIN when itemCost.ItemNumber > GameContext.Instance.UserModel.coin:
                        Toast.ShowUp("Không đủ tiền xu");
                        return;
                    case (int)ItemKey.COIN:
                        GameContext.Instance.UserModel.coin -= itemCost.ItemNumber;
                        break;
                    case (int)ItemKey.GEM when itemCost.ItemNumber > GameContext.Instance.UserModel.gem:
                        Toast.ShowUp("Không đủ kim cương");
                        return;
                    case (int)ItemKey.GEM:
                        GameContext.Instance.UserModel.gem -= itemCost.ItemNumber;
                        break;
                }
                //
                PlayerInventory.Instance.UpdateInventory(o);
                gridPool.reloadDataToVisibleCell();
            });
        });
        OnTabActive(0, TAB_CHAR);
    }

    #endregion

    #region Public Method

    public void OnTabActive(int tabId, int buttonTabId)
    {
        textTitle.text = buttonTabId switch
        {
            TAB_CHAR => "Nhân vật",
            TAB_BULLET => "Vũ khí",
            TAB_AVATAR => "Ảnh đại diện",
            _ => "Error"
        };
        
        switch (buttonTabId)
        {
            case TAB_CHAR:
                gridPool.setAdapter(Utils.GenListObject(DbManager.GetInstance().GetListCharacter()));
                break;
            case TAB_BULLET:
                gridPool.setAdapter(Utils.GenListObject(DbManager.GetInstance().GetListBullet()));
                break;
            default:
                gridPool.setAdapter(Utils.GenListObject(DbManager.GetInstance().GetListAvatar()));
                break;
        }
    }
    
    #endregion
}
