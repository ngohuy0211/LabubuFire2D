using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TranslateController : MonoBehaviour
{
	public GameObject m_GoTranslate;
	//
	public GameObject m_GoPoint1, m_GoPoint2;

    private System.Action m_OnCompleteShowCb;
    private System.Action m_OnCompleteHideCb;

    public void ShowObject(bool value)
    {
        m_GoTranslate.SetActive(value);
    }
    
    public void SetToPoint1()
    {
        Vector2 p1 = CanvasHelper.GetPosition(m_GoPoint1);
        CanvasHelper.SetPosition(m_GoTranslate, p1);
    }

    public void SetToPoint2()
    {
        Vector2 p2 = CanvasHelper.GetPosition(m_GoPoint2);
        CanvasHelper.SetPosition(m_GoTranslate, p2);
    }

    public void MakeTranslateShow(float duration,Ease easeType = Ease.Linear)
    {
        Vector2 p1 = CanvasHelper.GetPosition(m_GoPoint1);
        Vector2 p2 = CanvasHelper.GetPosition(m_GoPoint2);   
        //
        CanvasHelper.SetPosition(m_GoTranslate, p1);
        //
        m_GoTranslate.GetComponent<RectTransform>().DOAnchorPos(p2, duration)
            .From(p1)
            .SetEase(easeType).OnComplete(delegate
            {
                m_OnCompleteShowCb?.Invoke();
            });
    }
    
    public void MakeTranslateHide(float duration,Ease easeType = Ease.Linear)
    {
        Vector2 p2 = CanvasHelper.GetPosition(m_GoPoint1);
        Vector2 p1 = CanvasHelper.GetPosition(m_GoPoint2);   
        //
        CanvasHelper.SetPosition(m_GoTranslate, p1);
        //
        m_GoTranslate.GetComponent<RectTransform>().DOAnchorPos(p2, duration)
            .From(p1)
            .SetEase(easeType).OnComplete(delegate
            {
                m_OnCompleteHideCb?.Invoke();
            });
    }

    public void SetCbOnCompleteShow(System.Action cb)
    {
        this.m_OnCompleteShowCb = cb;
    }
    
    public void SetCbOnCompleteHide(System.Action cb)
    {
        this.m_OnCompleteHideCb = cb;
    }
    
}
