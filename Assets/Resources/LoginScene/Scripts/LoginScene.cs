using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : BaseScene
{
    [SerializeField] PanelLogin panelLogin;
    [SerializeField] PanelRegister panelRegister;
    [SerializeField] PanelForgetPassword panelForgetPass;
    [SerializeField] PanelInitData panelInitData;

    protected override void Awake()
    {
        base.Awake();
        FirebaseManager.Instance.InitFirebase();
        SetPanelStatus(LoginPanelType.LOGIN);
        //
        ScreenTouchIndicator.GetInstance();
        FirebaseManager.Instance.LoginDoneCb = delegate
        {
            SetPanelStatus(LoginPanelType.INIT);
            panelInitData.StartLoadData();
        };
        FirebaseManager.Instance.RegisterDoneCb = () => SetPanelStatus(LoginPanelType.LOGIN);
        //
        panelLogin.SetClickRegisterCb(() => SetPanelStatus(LoginPanelType.REGISTER));
        panelLogin.SetClickForgotPassCb(() => SetPanelStatus(LoginPanelType.FORGET));
        panelRegister.SetCloseCb(() => SetPanelStatus(LoginPanelType.LOGIN));
        panelForgetPass.SetCloseCb(() => SetPanelStatus(LoginPanelType.LOGIN));
    }

    private void SetPanelStatus(LoginPanelType type)
    {
        panelLogin.gameObject.SetActive(type == LoginPanelType.LOGIN);
        panelRegister.gameObject.SetActive(type == LoginPanelType.REGISTER);
        panelForgetPass.gameObject.SetActive(type == LoginPanelType.FORGET);
        panelInitData.gameObject.SetActive(type == LoginPanelType.INIT);
    }
}
