using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelForgetPassword : MonoBehaviour
{
    #region Define

    [SerializeField] private InputField inputEmail;
    [SerializeField] private Button buttonSendMail;
    [SerializeField] private Button buttonClose;
    
    #endregion

    #region Properties

    private System.Action _actCloseCb;
    
    #endregion

    #region Core MonoBehavior

    void Start()
    {
        UIHelper.AddButtonClickClose(buttonClose, OnClickClose);
        UIHelper.AddButtonClickNormal(buttonSendMail, OnClickSendMail);
    }
    

    #endregion

    #region Public Method

    public void SetCloseCb(System.Action cb)
    {
        _actCloseCb = cb;
    }
    
    #endregion

    #region Private Method

    private void OnClickClose()
    {
        _actCloseCb?.Invoke();
    }
    
    private void OnClickSendMail()
    {
        string email = inputEmail.text.Trim().ToLower();
        FirebaseManager.Instance.ForgetPasswordSubmit(email);
    }
    
    #endregion
}
