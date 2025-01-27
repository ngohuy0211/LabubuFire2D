using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AnimAutoFadeOn : MonoBehaviour
{
    private CanvasGroup m_CanvasGroup;
    private void Awake()
    {
        m_CanvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        m_CanvasGroup.alpha = 0;
    }

    private void Start()
    {
        StartCoroutine(DelayFade());
    }

    IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(1.5f);
        m_CanvasGroup.DOFade(1, 0.5f);
    }
}
