using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupShop : BasePopup, ITabListener
{
    #region Define

    [SerializeField] private GridPoolGroup gridPoolChar;
    [SerializeField] private GridPoolGroup gridPoolBullet;
    [SerializeField] private Text textTitle;
    [SerializeField] private Button buttonClose, buttonClose1;
    [SerializeField] private TabManager tabManager;
    
    #endregion

    #region Properties

    private const int TAB_CHAR = 1;
    private const int TAB_BULLET = 2;
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
            OnTabActive(0, TAB_CHAR);
        }
    }

    #endregion

    #region Public Method

    public void OnTabActive(int tabId, int buttonTabId)
    {
        
    }
    
    #endregion

    #region Private Method

    #endregion
}
