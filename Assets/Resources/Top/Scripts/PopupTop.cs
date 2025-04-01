using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UI;

public class PopupTop : BasePopup
{
    #region Define

    [SerializeField] private VerticalPoolGroup verPoolUser;
    [SerializeField] private Button buttonClose;
    
    #endregion

    #region Properties

    #endregion

    #region Core MonoBehavior

    protected override void Awake()
    {
        base.Awake();
        UIHelper.AddButtonClickClose(buttonClose, ClosePopup);
        verPoolUser.howToUseCellData(delegate(GameObject go, object data)
        {
            CellUserTop cell = go.GetComponent<CellUserTop>();
            cell.SetData((UserModel)data);
        });
        FirebaseManager.Instance.GetUserTopDoneCb = delegate
        {
            verPoolUser.setAdapter(Utils.GenListObject(GameContext.Instance.LstUsersModel));
        };
    }

    protected void Start()
    {
        SetData();
    }

    #endregion
    
    #region Private Method

    private void SetData()
    {
        FirebaseManager.Instance.GetAllUserTop100();
    }
    
    #endregion
}
