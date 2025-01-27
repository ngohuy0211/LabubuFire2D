using System;
using UnityEngine;
using DG.Tweening;

public class UIRotate : UITween
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 startState = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField] private Vector3 finishState = Vector3.one;

    private void Reset()
    {
        target = transform;
    }

    public override void SetToStartState()
    {
        target.localRotation = Quaternion.Euler(startState);
    }

    public override void SetToFinishState()
    {
        target.localRotation = Quaternion.Euler(finishState);
    }

    public override void DoMotion(Action callback = null)
    {
        SetToStartState();
        target?.DOLocalRotate(finishState, Duration)
            .SetEase(ease)
            .SetDelay(delay)
            .OnComplete(() => callback?.Invoke());
    }

    [ContextMenu("Set Start")]
    private void StartToTarget()
    {
        startState = target.localRotation.eulerAngles;
    }
    [ContextMenu("Set Finish")]
    private void FinishToTarget()
    {
        finishState = target.localRotation.eulerAngles;
    }
}
