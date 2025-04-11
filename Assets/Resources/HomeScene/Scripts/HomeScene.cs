using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScene : BaseScene
{
    #region Define

    [SerializeField] private PlayerAvatar playerAvatar;
    [SerializeField] private SpineObject spineChar;
    [SerializeField] private Button buttonCharInfo, buttonPlay, buttonTop, buttonShop, buttonIAP;
    
    #endregion

    #region Core MonoBehavior

    protected override void Awake()
    {
        base.Awake();
        UIHelper.AddButtonClickNormal(buttonCharInfo, OnClickCharInfo);
        UIHelper.AddButtonClickNormal(buttonPlay, OnClickPlay);
        UIHelper.AddButtonClickNormal(buttonTop, OnClickTop);
        UIHelper.AddButtonClickNormal(buttonShop, OnClickShop);
        UIHelper.AddButtonClickNormal(buttonIAP, OnClickIAP);
    }

    protected override void Start()
    {
        base.Start();
        UserModel userModel = GameContext.Instance.UserModel; 
        LoadSpineChar(userModel.characterUsingId);
        playerAvatar.SetData(userModel.avatarUsingId);
        playerAvatar.SetClickAvatarCb(OnClickAvatar);
    }

    #endregion

    #region Private Method

    private void LoadSpineChar(int charId)
    {
        string pathSpine = "_Common/Characters/" + charId + "/" + charId;
        spineChar.LoadSpine(pathSpine);
    }

    private void OnClickAvatar()
    {
        //Popup Setting
    }

    private void OnClickCharInfo()
    {
        
    }
    
    private void OnClickPlay()
    {
        
    }
    
    private void OnClickTop()
    {
        BasePopup.ShowPopup<PopupTop>(PopupPath.POPUP_USER_TOP);
    }
    
    private void OnClickShop()
    {
        BasePopup.ShowPopup<PopupShop>(PopupPath.POPUP_SHOP);
    }
    
    private void OnClickIAP()
    {
        
    }

    #endregion
}
