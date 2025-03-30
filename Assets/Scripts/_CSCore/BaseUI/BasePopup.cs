using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BasePopup : MonoBehaviour
{
    [SerializeField] GameObject goContent;
    [SerializeField] protected bool setToLast = true;
    
    private DoTweenPopup _tween;
    private ScrollRect[] _scrollArr;
    private Canvas _canvasOverride;

    
    public System.Action closeDoneCB { get; set; }
    public System.Action openDoneCB { get; set; }
    
    protected virtual void Awake()
    {
        if (setToLast)
        {
            Canvas canvasParrent = gameObject.GetComponentInParent<Canvas>();

            if (_canvasOverride == null) _canvasOverride = gameObject.AddComponent<Canvas>();

            if (canvasParrent != null)
            {
                _canvasOverride.overrideSorting = true;
                _canvasOverride.sortingLayerName = canvasParrent.sortingLayerName;
                gameObject.AddComponent<GraphicRaycaster>();
                StaticData.LayerOrderCurrent += 100;
                _canvasOverride.sortingOrder = StaticData.LayerOrderCurrent;
            }
        }

        if (goContent != null)
        {
            _tween = goContent.GetComponent<DoTweenPopup>();
            if (_tween == null)
            {
#if UNITY_EDITOR
                Debug.Log("----- Thêm DoTweenPopup vào " + this.name + " để custom type show -----");
#endif
                _tween = goContent.AddComponent<DoTweenPopup>();
            }

            _tween.ShowPopup(delegate()
            {
                openDoneCB?.Invoke();
            });
        }
        
    }

    public bool IsShow()
    {
        return this.gameObject.activeInHierarchy;
    }

    protected virtual void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePopup();
        }
#endif
    }


    protected virtual void ClosePopup()
    {
        if (goContent != null)
        {
            if (_tween == null) closeDoneCB?.Invoke();
            else
                _tween.ClosePopup(delegate()
                {
                    closeDoneCB?.Invoke();
                    Destroy(this.gameObject);
                });
        }
        else
        {
            if (this != null)
                Destroy(this.gameObject);
        }
    }

    protected virtual void ClosePopupNotEffect()
    {
        Destroy(this.gameObject);
    }
    
    
    /// <summary>
    /// Dùng khi Popup k cần truyền dữ liệu hoặc action
    /// Dùng khi Popup cần truyền dữ liệu hoặc action nhưng cần fix cứng số lượng dữ liệu hoặc action
    /// Override lại 2 func bên dưới để lấy dữ liệu hoặc action đã truyền
    /// </summary>
    /// <param name="pathPopup"></param>
    /// <param name="lstData"></param>
    /// <param name="lstAction"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T ShowPopup<T>(string pathPopup, List<object> lstData = null, List<Action> lstAction = null) where T : BasePopup
    {
        GameObject prefab = ResourceHelper.LoadPrefab(pathPopup);
        
        GameObject goPopup = Instantiate(prefab, CanvasHelper.GetCanvas().transform, false);
        
        T popup = goPopup.GetComponent<T>();

        if (lstData != null)
            popup.InitializePopupData(lstData);
        if (lstData != null)
            popup.InitializePopupAction(lstAction);
        
        return popup;
    }

    protected virtual void InitializePopupData(List<object> lstData)
    {
        
    }

    protected virtual void InitializePopupAction(List<Action> lstAction)
    {
        
    }
}