using System;
using UnityEngine;

public class UITransition : MonoBehaviour
{
    [SerializeField] private UITween showMotion;
    [SerializeField] private UITween hideMotion;

    public void DoShow()
    {
        DoShow(null);
    }

    public void DoShow(Action callback = null)
    {
        hideMotion?.SetToStartState();
        showMotion?.DoMotion(callback);
    }

    public void DoHide()
    {
        DoHide(null);
    }

    public void DoHide(Action callback = null)
    {
        hideMotion?.DoMotion(callback);
    }
}
