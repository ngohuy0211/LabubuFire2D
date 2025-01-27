using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    [SerializeField] int buttonTabId = -1;
    [SerializeField] GameObject goOn;
    [SerializeField] GameObject goOff;
    [SerializeField] Text textButtonOn;
    [SerializeField] Text textButtonOff;
    
    [HideInInspector] public bool m_IsOn = false;
    public System.Action<int> m_TabClickCallback { get; set; }
    public Action<bool> cbOut { get; set; }

    public int ButtonTabId
    {
        get
        {
            return buttonTabId;
        }
        set
        {
            buttonTabId = value;
        }
    }
    
    private void Awake()
    {
        UIHelper.AddButtonClickTab(this.gameObject.GetComponent<Button>(), ButtonTabClick);
    }
    
    public void SetOn(bool on)
    {
        m_IsOn = on;
        cbOut?.Invoke(on);
        if (on)
        {
            goOn.SetActive(true);
            goOff.SetActive(false);
        }
        else
        {
            goOff.SetActive(true);
            goOn.SetActive(false);
        }
    }

    public void SetButtonId(int id)
    {
        buttonTabId = id;
    }

    public void SetTextButton(string textBtn)
    {
        if (textButtonOn != null)
            textButtonOn.text = textBtn;
        if (textButtonOff != null)
            textButtonOff.text = textBtn;
    }

    private void ButtonTabClick()
    {
        if (m_TabClickCallback != null)
            m_TabClickCallback.Invoke(buttonTabId);
    }

    public void LoadSpriteIconOn(Sprite sp)
    {
        goOn.gameObject.GetComponent<Image>().sprite = sp;
    }
    
    public void LoadSpriteIconOff(Sprite sp)
    {
        goOff.gameObject.GetComponent<Image>().sprite = sp;
    }
}