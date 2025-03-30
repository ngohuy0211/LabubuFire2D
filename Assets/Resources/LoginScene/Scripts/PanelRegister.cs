using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelRegister : MonoBehaviour
{
    #region Define
    [SerializeField] private InputField inputUserName;
    [SerializeField] private InputField inputEmail;
    [SerializeField] private InputField inputPassword;
    [SerializeField] private Button buttonRegister;
    [SerializeField] private Button buttonClose;
    
    #endregion

    #region Properties

    private System.Action _cbClose;
    
    #endregion

    #region Core MonoBehavior

    void Start()
    {
        UIHelper.AddButtonClickClose(buttonClose, OnClosePanel);
        UIHelper.AddButtonClickNormal(buttonRegister, OnClickRegister);
    }

    #endregion

    #region Public Method

    public void SetCloseCb(System.Action cb)
    {
        _cbClose = cb;
    }
    
    #endregion

    #region Private Method

    private void OnClickRegister()
    {
        string email = inputEmail.text.Trim().ToLower();
        string password = inputPassword.text.Trim();
        string username = inputUserName.text.Trim();
        SettingManager.SaveEmailLogin(email);
        SettingManager.SavePassword(password);
        FirebaseManager.Instance.RegisterUser(email, password, username);
    }
    
    private void OnClosePanel()
    {
        _cbClose?.Invoke();
    }
    
    #endregion
}
