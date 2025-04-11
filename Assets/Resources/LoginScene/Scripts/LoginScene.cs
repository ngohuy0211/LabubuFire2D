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

    private bool _isLeftApp = false;
    
    protected override void Awake()
    {
        base.Awake();
        ScreenTouchIndicator.GetInstance();
        SetPanelStatus(LoginPanelType.NONE);
        //
        FirebaseManager.Instance.InitFirebase();
        FirebaseManager.Instance.GetDataAppDoneCb = delegate
        {
            SetPanelStatus(LoginPanelType.LOGIN);
            FirebaseManager.Instance.LoginDoneCb += CheckVersion;
            FirebaseManager.Instance.RegisterDoneCb = () => SetPanelStatus(LoginPanelType.LOGIN);
            //
            panelLogin.SetClickRegisterCb(() => SetPanelStatus(LoginPanelType.REGISTER));
            panelLogin.SetClickForgotPassCb(() => SetPanelStatus(LoginPanelType.FORGET));
            panelRegister.SetCloseCb(() => SetPanelStatus(LoginPanelType.LOGIN));
            panelForgetPass.SetCloseCb(() => SetPanelStatus(LoginPanelType.LOGIN));  
        };
    }

    private void SetPanelStatus(LoginPanelType type)
    {
        panelLogin.gameObject.SetActive(type == LoginPanelType.LOGIN);
        panelRegister.gameObject.SetActive(type == LoginPanelType.REGISTER);
        panelForgetPass.gameObject.SetActive(type == LoginPanelType.FORGET);
        panelInitData.gameObject.SetActive(type == LoginPanelType.INIT);
    }

    private void CheckVersion()
    {
        if (GameContext.Instance.CurrentVersionApp != Constants.VERSION)
        {
            PopupMessage.ShowUp("Thông báo", "Đã có phiên bản mới, cần cập nhật để có thêm nhiều tính năng", "Cập nhật",
                delegate
                {
#if UNITY_ANDROID
                    StartCoroutine(OpenURL(GameContext.Instance.LinkDownAndroid));
#elif UNITY_IOS
                    StartCoroutine(OpenURL(GameContext.Instance.LinkDownIos));
#endif
                });
        }
        else
        {
            SetPanelStatus(LoginPanelType.INIT);
            panelInitData.StartLoadData();
        }
    }
    
    IEnumerator OpenURL(string url)
    {
        yield return new WaitForSeconds(0.5f);
        Application.OpenURL(url);
    }
    
    private void OnApplicationPause(bool pauseStatus)
    {
        _isLeftApp = true;
    }
}
