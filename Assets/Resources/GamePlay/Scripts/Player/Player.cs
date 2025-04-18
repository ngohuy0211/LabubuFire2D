using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Define

    [SerializeField] private SpineAnimationObject skelPlayer;
    [SerializeField] private Transform tfSpawnBullet;
    
    #endregion

    #region Public Method

    public Transform GetSpawnBulletTf()
    {
        return tfSpawnBullet;
    }

    public void SetDataPlayer(CharacterModel currChar)
    {
        if (currChar == null) return;
        
        string pathPlayerSkel = "_Common/Characters/" + currChar.ItemKey + "/" + currChar.ItemKey;
        skelPlayer.LoadSpine(pathPlayerSkel);
    }
    
    #endregion
}
