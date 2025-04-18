using System;
using System.Collections;
using System.Collections.Generic;
using ChiuChiuCSCore;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;

public class Toast : MonoBehaviour
{
    [SerializeField] Text text1;
    [SerializeField] GameObject panelToastText;
    [SerializeField] protected bool setToLast = true;
    Canvas m_CanvasOverride;
    private float MAX_WIDTH = 450;

    public void Awake()
    {
        HideAllToast();
    }

    public void Start()
    {
        if (setToLast)
        {
            Canvas canvasParrent = gameObject.GetComponentInParent<Canvas>();

            if (m_CanvasOverride == null)
                m_CanvasOverride = gameObject.AddComponent<Canvas>();

            if (canvasParrent != null)
            {
                m_CanvasOverride.overrideSorting = true;
                m_CanvasOverride.sortingLayerName = canvasParrent.sortingLayerName;
                gameObject.AddComponent<GraphicRaycaster>();
                StaticData.LayerOrderCurrent += 100;
                m_CanvasOverride.sortingOrder = StaticData.LayerOrderCurrent;
            }
        }
    }


    void PlayAniMove(int endValue)
    {
        Tween tween = panelToastText.transform.DOLocalMoveY(endValue, 1f);
        tween.SetEase(Ease.OutCirc);
        tween.OnComplete(delegate() { PlayAniFade(endValue); });
    }

    void PlayAniFade(int endValue)
    {
        Tween tween = panelToastText.GetComponent<CanvasGroup>().DOFade(endValue, 0.3f);
        tween.OnComplete(delegate()
        {
            /*Time.timeScale = timeScale;*/
            Destroy(gameObject);
        });
    }

    public void HideAllToast()
    {
        panelToastText.gameObject.SetActive(false);
    }

    public void SetText(string text)
    {
        HideAllToast();
        if (string.IsNullOrEmpty(text)) return;
        panelToastText.gameObject.SetActive(true);
        text1.text = text;
        text1.color = Color.white;
        PlayAniMove(0);
    }
    
    public static GameObject ShowUp(string text)
    {
        GameObject prefabToast = ResourceHelper.LoadPrefab("_Common/Prefabs/PopupToast");
        GameObject goToast = Instantiate(prefabToast, CanvasHelper.GetCanvas().transform, false);
        goToast.GetComponent<Toast>().SetText(text);
        return goToast;
    }
}