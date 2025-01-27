using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCountup : MonoBehaviour
{
    public Text m_TextTime;
    private string m_Infix = "";

    private string m_PosInfix = "";

    //
    private int m_TimeCountup = 0;
    private int m_MaxTime = 0;
    private ActionRepeatTimer m_Timer;

    private bool haveOnlySec = false;
    private bool setPause = false;

    private bool isTimeStartZero = false;

    // Use this for initialization
    void Start()
    {
        m_Timer = new ActionRepeatTimer(1f, delegate()
        {
            if (!setPause)
            {
                //Debug.Log(m_TimeCountdown);
                if (this.isTimeStartZero)
                {
                    m_TimeCountup++;
                }
                else
                {
                    if (m_TimeCountup > 0 && m_TimeCountup < m_MaxTime)
                        m_TimeCountup++;
                }
                if (haveOnlySec)
                    m_TextTime.text = m_Infix + m_TimeCountup + m_PosInfix;
                else
                    m_TextTime.text = m_Infix + TimeUtils.FormatHourFromSec(m_TimeCountup) + m_PosInfix;
            }
        });
    }

    public int GetTimeLeft()
    {
        return m_TimeCountup;
    }

    public void SetText(string text)
    {
        m_TimeCountup = -1;
        m_TextTime.text = text;
    }

    public void SetCountupTime(int timeStart, int maxTime = 0, string desc = "", string infix = "",
        string posInfix = "", bool haveOnlySec = false)
    {
        m_Infix = infix;
        m_PosInfix = posInfix;
        m_MaxTime = maxTime;
        m_TimeCountup = timeStart;
        this.haveOnlySec = haveOnlySec;
        if (timeStart > 0)
        {
            if (haveOnlySec)
                m_TextTime.text = m_Infix + timeStart + m_PosInfix;
            else
                m_TextTime.text = m_Infix + TimeUtils.FormatHourFromSec(timeStart) + m_PosInfix;
        }
        else
            m_TextTime.text = desc;
    }
    public void SetCountupTimeZero(int timeStart, int maxTime = 0, string desc = "", string infix = "",
        string posInfix = "", bool haveOnlySec = false, bool isTimeStartZero = true)
    {
        m_Infix = infix;
        m_PosInfix = posInfix;
        m_MaxTime = maxTime;
        m_TimeCountup = timeStart;
        this.isTimeStartZero = isTimeStartZero;
        this.haveOnlySec = haveOnlySec;
        if (timeStart >= 0)
        {
            if (haveOnlySec)
                m_TextTime.text = m_Infix + timeStart + m_PosInfix;
            else
                m_TextTime.text = m_Infix + TimeUtils.FormatHourFromSec(timeStart) + m_PosInfix;   
        }
        else
            m_TextTime.text = "00:00:00";

    }

    public void Pause(bool setPause)
    {
        this.setPause = setPause;
    }

    void Update()
    {
        if (m_Timer != null)
            m_Timer.UpdateTimer(Time.deltaTime);
    }
}