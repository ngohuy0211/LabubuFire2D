using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelLogin : MonoBehaviour
{
    #region Define

    [SerializeField] private InputField inputEmail;
    [SerializeField] private InputField inputPassword;
    [SerializeField] private Button buttonLogin;
    [SerializeField] private Button buttonRegister;
    [SerializeField] private Button buttonForgotPassword;
    [SerializeField] private Button buttonLoginGoogle;
    [SerializeField] private Button buttonLoginFacebook;
    
    #endregion

    #region Properties

    private System.Action _actClickRegisterCb;
    private System.Action _actClickForgotPasswordCb;
    
    #endregion

    #region Core MonoBehavior

    protected void Awake()
    {
        UIHelper.AddButtonClickNormal(buttonLogin, OnClickLogin);
        UIHelper.AddButtonClickNormal(buttonRegister, OnClickRegister);
        UIHelper.AddButtonClickNormal(buttonForgotPassword, OnClickForgotPass);
        UIHelper.AddButtonClickNormal(buttonLoginGoogle, OnClickLoginGoogle);
        UIHelper.AddButtonClickNormal(buttonLoginFacebook, OnClickLoginFacebook);
    }

    private void OnEnable()
    {
        string emailCached = SettingManager.GetUsername();
        if (!string.IsNullOrEmpty(emailCached))
            inputEmail.text = emailCached;
        string password = SettingManager.GetPassword();
        if (!string.IsNullOrEmpty(password))
            inputPassword.text = password;
    }

    #endregion

    #region Public Method

    public void SetClickRegisterCb(System.Action cb)
    {
        _actClickRegisterCb = cb;
    }
    
    public void SetClickForgotPassCb(System.Action cb)
    {
        _actClickForgotPasswordCb = cb;
    }
    
    #endregion

    #region Private Method

    private void OnClickLogin()
    {
        string email = inputEmail.text.Trim().ToLower();
        string password = inputPassword.text.Trim();
        SettingManager.SaveEmailLogin(email);
        SettingManager.SavePassword(password);
        FirebaseManager.Instance.LoginWithEmail(email, password);
    }
    
    private void OnClickRegister()
    {
        _actClickRegisterCb?.Invoke();
    }
    
    private void OnClickForgotPass()
    {
        _actClickForgotPasswordCb?.Invoke();
    }
    
    private void OnClickLoginGoogle()
    {
        FirebaseManager.Instance.LoginWithFb();
    }
    
    private void OnClickLoginFacebook()
    {
        FirebaseManager.Instance.LoginWithGoogle();
    }
    
    #endregion
}
