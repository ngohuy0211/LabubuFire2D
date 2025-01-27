using System;
using UnityEngine;
using DG.Tweening;

public class UIAnchor : UITween
{
    [SerializeField] private RectTransform target;
    [SerializeField] private Vector2 startState;
    [SerializeField] private Vector2 finishState;

    private void Reset()
    {
        target = transform as RectTransform;
    }

    public override void SetToStartState()
    {
        target.anchoredPosition = startState;
    }

    public override void SetToFinishState()
    {
        target.anchoredPosition = finishState;
    }

    public override void DoMotion(Action callback = null)
    {
        SetToStartState();
        target?.DOAnchorPos(finishState, Duration)
            .SetEase(ease)
            .SetDelay(delay)
            .OnComplete(() => callback?.Invoke());
    }

    [ContextMenu("Set Start")]
    private void StartToTarget()
    {
        startState = target.anchoredPosition;
    }
    [ContextMenu("Set Finish")]
    private void FinishToTarget()
    {
        finishState = target.anchoredPosition;
    }
}
