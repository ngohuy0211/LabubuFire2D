using System;
using UnityEngine;
using DG.Tweening;

public class UIFade : UITween {
    [SerializeField] private CanvasGroup target;
    [SerializeField, Range(0f, 1f)] private float startState = 0;
    [SerializeField, Range(0f, 1f)] private float finishState = 1;

    private void Reset() {
        target = GetComponent<CanvasGroup>();
    }

    public override void SetToStartState() {
        target.alpha = startState;
    }

    public override void SetToFinishState() {
        target.alpha = finishState;
    }

    public override void DoMotion(Action callback = null) {
        SetToStartState();
        target?.DOFade(finishState, Duration)
            .SetEase(ease)
            .SetDelay(delay)
            .OnComplete(() => callback?.Invoke());
    }

    [ContextMenu("Set Start")]
    private void StartToTarget() {
        startState = target.alpha;
    }
    [ContextMenu("Set Finish")]
    private void FinishToTarget() {
        finishState = target.alpha;
    }
}
