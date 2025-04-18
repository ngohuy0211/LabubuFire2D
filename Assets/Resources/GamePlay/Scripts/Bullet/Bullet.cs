using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private SpineAnimationObject skelBullet;

    public void SetData(ItemBullet bullet)
    {
        // string pathBullet = "_Common/ItemBullet/" + bullet.ItemKey + "/" + bullet.ItemKey;
        // skelBullet.LoadSpine(pathBullet);
    }
}
