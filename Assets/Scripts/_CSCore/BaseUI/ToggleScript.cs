using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{
    [HideInInspector]
    public bool m_IsOn = false;

    public GameObject m_GoOn, m_GoOff;

    public void SetOn()
    {
        this.m_IsOn = true;
        //
        if (m_GoOn != null)
            m_GoOn.SetActive(true);
        if (m_GoOff != null)
            m_GoOff.SetActive (false);
    }

    public void SetSprite(Sprite spriteOn, Sprite spriteOff)
    {
        Image rendererOn = this.m_GoOn.GetComponent<Image> ();
        Image rendererOff = this.m_GoOff.GetComponent<Image> ();
        //
        if (rendererOn != null)
        {
            rendererOn.sprite = spriteOn;
        }
        if (rendererOff != null)
        {
            rendererOff.sprite = spriteOff;
        }
    }

    public void SetOff()
    {
        this.m_IsOn = false;
        //
        if (m_GoOn != null)
            m_GoOn.SetActive(false);
        if (m_GoOff != null)
            m_GoOff.SetActive (true);
    }
}
