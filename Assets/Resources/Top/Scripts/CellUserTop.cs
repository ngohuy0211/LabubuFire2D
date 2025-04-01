using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellUserTop : MonoBehaviour
{
    #region Define

    [SerializeField] private PlayerAvatar playerAvatar;
    [SerializeField] private Text userName;
    [SerializeField] private Text userLevel;

    #endregion
    
    #region Public Method

    public void SetData(UserModel user)
    {
        playerAvatar.SetData(user.avatarUsingId);
        userName.text = user.userDisplayName;
        userLevel.text = user.currentMapLevel.ToString();
    }

    #endregion
}