using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class EventCollider2D : MonoBehaviour
{
    private Collider2D collider2D;
    private Action cb;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    private void Start()
    {
        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
    }

    private void OnMouseDown()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        if (results.Count == 1 && collider2D != null) cb.Invoke();
    }


    public void OnClickCB(Action cb)
    {
        this.cb = cb;
    }
}