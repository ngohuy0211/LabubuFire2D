using System;
using UnityEngine;
using DG.Tweening;

public abstract class UITween : MonoBehaviour
{
    [SerializeField, Range(0f, 5f)] protected float delay = 0f;
    [SerializeField, Range(0f, 5f)] protected float duration = 0.5f;
    [SerializeField] protected Ease ease = Ease.InOutBack;

    public float Delay { get => delay; }
    public float Duration { get => duration; }

    public abstract void SetToStartState();
    public abstract void SetToFinishState();
    public abstract void DoMotion(Action callback = null);
}
