using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayFunctionCalling
{
    private float m_Delay = 0f;
    private System.Action m_Action;
    private MonoBehaviour m_Mono;

    //
    public DelayFunctionCalling(MonoBehaviour mono, System.Action action, float delay)
    {
        m_Mono = mono;
        m_Action = action;
        m_Delay = delay;
    }

    public void DoIt()
    {
        m_Mono.StartCoroutine(CallFunction());
    }


    private IEnumerator CallFunction()
    {
        yield return new WaitForSeconds(m_Delay);
        //
        if (m_Action != null)
        {
            m_Action();
        }
    }
}