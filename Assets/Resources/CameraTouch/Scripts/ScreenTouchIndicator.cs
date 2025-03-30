using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScreenTouchIndicator : MonoBehaviour
{
    private static ScreenTouchIndicator _instance;
    
    public static ScreenTouchIndicator GetInstance()
    {
        if (_instance == null)
        {
            GameObject go = new GameObject("TouchIndicator");
            DontDestroyOnLoad(go);
            //
            ScreenTouchIndicator script = go.AddComponent<ScreenTouchIndicator>();
            _instance = script;
        }

        //
        return _instance;
    }

    private List<TouchEffect> _listTouchEffect = new List<TouchEffect>();

    public void PlayTouchEffect(Vector3 mousePos)
    {
        GameObject goCamera = GameObject.Find("CameraTouchEffect");
        //
        if (goCamera == null)
        {
            return;
        }

        TouchEffect freeTouchEffect = null;
        //
        foreach (TouchEffect touchEffect in _listTouchEffect)
        {
            if (!touchEffect.onDuty)
            {
                freeTouchEffect = touchEffect;
                break;
            }
        }
        //
        //instance one
        if (freeTouchEffect == null)
        {
            GameObject prefab = Resources.Load<GameObject>("CameraTouch/Prefabs/TouchEffect");
            GameObject goEffect = Instantiate(prefab, transform, false);
            TouchEffect newTouchEffect = goEffect.GetComponent<TouchEffect>();
            _listTouchEffect.Add(newTouchEffect);
            //
            freeTouchEffect = newTouchEffect;
        }

        //
        Camera camPopup = goCamera.GetComponent<Camera>();
        //
        Vector3 posEffect = camPopup.ScreenToWorldPoint(mousePos);
        posEffect.z = 0;
        //
        freeTouchEffect.SetPosition(posEffect);
        freeTouchEffect.Play();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            this.PlayTouchEffect(mousePos);
        }
    }
}