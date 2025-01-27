using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIColor : UITween {
    [SerializeField] private Graphic target;
    [SerializeField] private Color startState = Color.white;
    [SerializeField] private Color finishState = Color.white;

    private void Reset() {
        target = GetComponent<Graphic>();
    }

    public override void SetToStartState() {
        target.color = startState;
    }

    public override void SetToFinishState() {
        target.color = finishState;
    }

    public override void DoMotion(Action callback = null) {
        SetToStartState();
        target?.DOColor(finishState, Duration)
            .SetEase(ease)
            .SetDelay(delay)
            .OnComplete(() => callback?.Invoke());
    }

    [ContextMenu("Set Start")]
    private void StartToTarget() {
        startState = target.color;
    }
    [ContextMenu("Set Finish")]
    private void FinishToTarget() {
        finishState = target.color;
    }
}
