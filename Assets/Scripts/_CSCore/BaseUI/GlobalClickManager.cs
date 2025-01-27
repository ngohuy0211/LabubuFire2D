using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GlobalClickManager : Singleton<GlobalClickManager>, IPointerClickHandler
{
    public event System.Action OnGlobalClick;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnGlobalClick != null)
            OnGlobalClick.Invoke();
    }
}