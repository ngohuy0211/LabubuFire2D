using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHelper
{
    public static Vector2 ScreenPointToLocalPointInRect(RectTransform rect,
        Vector2 screenPoint)
    {
        Vector2 v = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint,
            Camera.main, out v);
        return v;
    }

    public static Vector2 WorldToScreenPoint(Transform tfObject)
    {
        Vector3 pointScreen = Camera.main.WorldToScreenPoint(tfObject.position);
        return pointScreen;
    }

    public static void AddButtonClickNormal(Button button, System.Action action)
    {
        button.onClick.AddListener(delegate { action?.Invoke(); });
        SoundManager.GetInstance().PlaySound("");
    }
    
    public static void AddButtonClickNormalWithNumber(Button button, System.Action<int> action, int num)
    {
        button.onClick.AddListener(delegate { action?.Invoke(num); });
        SoundManager.GetInstance().PlaySound("");
    }
    
    public static void AddButtonClickClose(Button button, System.Action action)
    {
        button.onClick.AddListener(delegate { action?.Invoke(); });
        SoundManager.GetInstance().PlaySound("");
    }
    
    public static void AddButtonClickCloseWithNumber(Button button, System.Action<int> action, int num)
    {
        button.onClick.AddListener(delegate { action?.Invoke(num); });
        SoundManager.GetInstance().PlaySound("");
    }
    
    public static void AddButtonClickBack(Button button, System.Action action)
    {
        button.onClick.AddListener(delegate { action?.Invoke(); });
        SoundManager.GetInstance().PlaySound("");
    }
    
    public static void AddButtonClickBackWithNumber(Button button, System.Action<int> action, int num)
    {
        button.onClick.AddListener(delegate { action?.Invoke(num); });
        SoundManager.GetInstance().PlaySound("");
    }
    
    public static void AddButtonClickTab(Button button, System.Action action)
    {
        button.onClick.AddListener(delegate { action?.Invoke(); });
        SoundManager.GetInstance().PlaySound("");
    }
    
    public static void AddButtonClickTabWithNumber(Button button, System.Action<int> action, int num)
    {
        button.onClick.AddListener(delegate { action?.Invoke(num); });
        SoundManager.GetInstance().PlaySound("");
    }
}