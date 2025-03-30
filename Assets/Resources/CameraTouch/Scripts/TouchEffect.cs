using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

public class TouchEffect : MonoBehaviour
{
    [HideInInspector] public bool onDuty = false;
    [SerializeField] ParticleSystem particleSystem;
    private const float TIME_EFFECT = 0.8f;
    
    public System.Action touchEffectCompleteCb;

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Play()
    {
        onDuty = true;
        particleSystem.Play();
        //
        new DelayFunctionCalling(this, delegate { onDuty = false; }, TIME_EFFECT).DoIt();
    }
}