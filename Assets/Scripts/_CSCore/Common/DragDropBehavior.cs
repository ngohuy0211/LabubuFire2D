using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropBehavior : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public bool m_EnableDragDrop = false;
    
    private Vector2 m_SizeCanvas;
    private RectTransform m_RectTransform;
    private Vector2 m_PointBeginDrag;
    private Vector2 m_PosContentBeginDrag;
    private Vector2 m_PointDown = Vector2.zero;
    private int m_IndexCurrentMap = 0;

    private System.Action m_CbMoveDone;

    public virtual void Start()
    {
        GameObject goCanvas = CanvasHelper.GetCanvas();
        RectTransform rectCanvas = goCanvas.GetComponent<RectTransform> ();
        m_SizeCanvas = new Vector2 (rectCanvas.rect.width, rectCanvas.rect.height);
        // Debug.Log("============== SIZE CANVAS ==============");
        // Debug.Log(goCanvas.name);
        // Debug.Log(m_SizeCanvas);
        //
        m_RectTransform = GetComponent<RectTransform>();
    }

    public void MoveToPosition(Vector3 pos)
    {
        if (m_RectTransform == null)
            return;

        this.gameObject.transform.DOMove(pos, 0.2f).From(this.gameObject.transform.position).SetEase(Ease.Linear).OnComplete(
            delegate
            {
                m_CbMoveDone?.Invoke();
            });
    }

    public void SetCbMoveDone(System.Action cb)
    {
        m_CbMoveDone = cb;
    }

    public void SetPosition(Vector2 pos)
    {
        if (m_RectTransform != null)
            m_RectTransform.anchoredPosition = pos;
    }

    public Vector2 GetPosition()
    {
        if (m_RectTransform != null)
            return m_RectTransform.anchoredPosition;
        return Vector2.zero;
    }

    #region Event Handler implementation
    
    public void OnPointerDown(PointerEventData eventData)
    {
        this.gameObject.transform.SetAsLastSibling();
        //
        Vector2 posPointer = eventData.position;
        m_PointDown = ConvertMousePointerToCanvasPosition(posPointer);
        //
        OnItemPointDown();
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        Vector2 posPointer = eventData.position;
        Vector2 pointUp = ConvertMousePointerToCanvasPosition(posPointer);
        //
        float disc = Vector2.Distance(m_PointDown, pointUp);
        //
        if (disc < 20)
        {
            OnItemClick();
            OnItemPointUp();
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!m_EnableDragDrop)
            return;
        if (m_RectTransform == null)
            return;
        //
        // Debug.Log("HUY DEBUG: ------------ Begin");
        Vector2 posPointer = eventData.position;
        Vector2 pointCanvas = ConvertMousePointerToCanvasPosition(posPointer);
        m_PointBeginDrag = pointCanvas;
        m_PosContentBeginDrag = m_RectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!m_EnableDragDrop)
            return;
        if (m_RectTransform == null)
            return;
        // Debug.Log("HUY DEBUG: ------------ Drag");
        //
        Vector2 posPointer = eventData.position;
        // Debug.Log(posPointer);
        Vector2 pointCanvas = ConvertMousePointerToCanvasPosition(posPointer);
        // Debug.Log(pointCanvas);
        Vector2 offset = pointCanvas - m_PointBeginDrag;
        // Debug.Log(offset);
        Vector2 posNew = m_PosContentBeginDrag + offset;
        // Debug.Log(posNew);
        m_RectTransform.anchoredPosition = posNew;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!m_EnableDragDrop)
            return;
        // Debug.Log("HUY DEBUG: ------------ End Drag");
        if (m_RectTransform != null)
            OnItemDrop (m_RectTransform.anchoredPosition);
    }
    
    #endregion
    
    private Vector2 ConvertMousePointerToCanvasPosition(Vector2 mousePointer)
    {
        Vector3 vpPointRate = Camera.main.ScreenToViewportPoint(mousePointer);
        // Debug.Log("======================== Convert =====================");
        // Debug.Log(mousePointer);
        // Debug.Log(vpPointRate);
        // Debug.Log("======================== Convert =====================");
        float tmpX = vpPointRate.x * m_SizeCanvas.x;
        float tmpY = vpPointRate.y * m_SizeCanvas.y;
        //
        Vector2 pointCanvas = new Vector2(tmpX - m_SizeCanvas.x / 2, tmpY - m_SizeCanvas.y / 2);
        // Debug.Log(pointCanvas);
        //
        return pointCanvas;
    }
    
    public virtual void OnItemClick()
    {
	    
    }

    public virtual void OnItemDrop(Vector2 dropPosition)
    {

    }

    public virtual void OnItemPointUp()
    {
        
    }
    
    public virtual void OnItemPointDown()
    {
        
    }
}
