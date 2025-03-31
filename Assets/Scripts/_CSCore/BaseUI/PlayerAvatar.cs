using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAvatar : MonoBehaviour
{
    #region Define

    [SerializeField] private Image imgAvatar;
    
    #endregion

    #region Properties

    private System.Action _actClickCb;

    #endregion

    #region Core MonoBehavior

    private void Start()
    {
        UIHelper.AddButtonClickNormal(this.gameObject.GetComponent<Button>(), OnClickAvatar);
    }

    #endregion

    #region Public Method

    public void SetData(int avatarId)
    {
        if (avatarId > -1)
            imgAvatar.sprite =
                ResourceHelper.LoadSprite("_Common/ItemAvatar/" + avatarId);
        else
            FirebaseManager.Instance.LoadImage(imgAvatar, GameContext.Instance.UserModel.userUrlAvatar);
    }

    public void SetClickAvatarCb(System.Action cb)
    {
        _actClickCb = cb;
    }

    #endregion

    #region Private Method

    private void OnClickAvatar()
    {
        _actClickCb?.Invoke();
    }

    #endregion
}
