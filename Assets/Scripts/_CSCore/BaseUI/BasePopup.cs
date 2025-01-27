using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BasePopup : MonoBehaviour
{
    [SerializeField] GameObject m_GoContent;
    [SerializeField] protected bool m_SetToLast = true;
    
    private DoTweenPopup tween;
    private ScrollRect[] scrollArr;
    private Canvas m_CanvasOverride;

    
    public System.Action closeDoneCB { get; set; }
    public System.Action openDoneCB { get; set; }
    
    private void Awake()
    {
        if (m_SetToLast)
        {
            Canvas canvasParrent = gameObject.GetComponentInParent<Canvas>();

            if (m_CanvasOverride == null) m_CanvasOverride = gameObject.AddComponent<Canvas>();

            if (canvasParrent != null)
            {
                m_CanvasOverride.overrideSorting = true;
                m_CanvasOverride.sortingLayerName = canvasParrent.sortingLayerName;
                gameObject.AddComponent<GraphicRaycaster>();
                StaticData.LayerOrderCurrent += 100;
                m_CanvasOverride.sortingOrder = StaticData.LayerOrderCurrent;
            }
        }

        if (m_GoContent != null)
        {
            tween = m_GoContent.GetComponent<DoTweenPopup>();
            if (tween == null)
            {
#if UNITY_EDITOR
                Debug.Log("----- Thêm DoTweenPopup vào " + this.name + " để custom type show -----");
#endif
                tween = m_GoContent.AddComponent<DoTweenPopup>();
            }

            tween.ShowPopup(delegate()
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


    public virtual void ClosePopup()
    {
        if (m_GoContent != null)
        {
            if (tween == null) closeDoneCB?.Invoke();
            else
                tween.ClosePopup(delegate()
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

    public virtual void ClosePopupNotEffect()
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