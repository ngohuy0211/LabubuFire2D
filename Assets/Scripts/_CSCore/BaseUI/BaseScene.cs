﻿using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class BaseScene : MonoBehaviour
{
    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        AddCameraTouchEffect();
    }
    
    private void AddCameraTouchEffect()
    {
        GameObject goCamPopup = GameObject.Find("CameraTouchEffect");
        if (goCamPopup == null)
        {
            string path = "CameraTouch/Prefabs/CameraTouchEffect";
            GameObject prefab = Resources.Load<GameObject>(path);
            //
            goCamPopup = GameObject.Instantiate(prefab);
            goCamPopup.name = "CameraTouchEffect";
        }
    }
}