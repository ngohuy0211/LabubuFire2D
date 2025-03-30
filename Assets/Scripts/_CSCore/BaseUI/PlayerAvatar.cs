using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAvatar : MonoBehaviour
{
    #region Define

    [SerializeField] private Image imgAvatar;
    
    #endregion

    #region Core MonoBehavior

    void Start()
    {
        if (GameContext.Instance.UserModel.avatarUsingId > -1)
            imgAvatar.sprite =
                ResourceHelper.LoadSprite("_Common/ItemAvatar/" + GameContext.Instance.UserModel.avatarUsingId);
        else
            FirebaseManager.Instance.LoadImage(imgAvatar, GameContext.Instance.UserModel.userUrlAvatar);
    }

    #endregion
}
