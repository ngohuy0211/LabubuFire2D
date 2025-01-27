using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class OnClickControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Hold duration in seconds")] [Range(0.3f, 5f)]
    public float m_HoldDuration = 1f;

    public UnityEvent m_OnLongClick;
    public UnityEvent m_OnSortClick;

    private bool m_IsPointerDown = false;
    private bool m_IsLongClicked = false;
    private float m_ElapsedTime = 0f;

    private Button m_Button;

    private void Awake()
    {
        m_Button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_IsPointerDown = true;
    }

    private void Update()
    {
        if (m_IsPointerDown && !m_IsLongClicked)
        {
            m_ElapsedTime += Time.deltaTime;
            if (m_ElapsedTime >= m_HoldDuration)
            {
                m_IsLongClicked = true;
                m_ElapsedTime = 0f;
                if (m_Button.interactable && !object.ReferenceEquals(m_OnLongClick, null))
                    m_OnLongClick.Invoke();
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (m_ElapsedTime < m_HoldDuration && !m_IsLongClicked)
        {
            if (m_Button.interactable && !object.ReferenceEquals(m_OnSortClick, null))
                m_OnSortClick.Invoke();
        }
        //
        m_IsPointerDown = false;
        m_IsLongClicked = false;
        m_ElapsedTime = 0f;
    }
}