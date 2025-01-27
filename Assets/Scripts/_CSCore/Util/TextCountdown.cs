using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCountdown : MonoBehaviour
{
	public Text m_TextTime;
	
	private string m_Infix = "";
	private string m_PosInfix = "";
	private int m_TimeCountdown = 0;
	private ActionRepeatTimer m_Timer;
	private System.Action m_CountDownToZeroCb;
	private bool haveOnlySec = false;
	private bool setPause = false;


	// Use this for initialization
	void Start()
	{
		m_Timer = new ActionRepeatTimer(1f, delegate()
		{
			if (!setPause)
			{
				//Debug.Log(m_TimeCountdown);
				if (m_TimeCountdown > 0)
				{
					m_TimeCountdown--;
					if (m_TimeCountdown == 0)
					{
						if (m_CountDownToZeroCb != null)
						{
							m_CountDownToZeroCb();
						}
					}

					if (haveOnlySec)
					{
						m_TextTime.text = m_Infix + m_TimeCountdown + m_PosInfix;
					}
					else
					{
						m_TextTime.text = m_Infix + TimeUtils.FormatHourFromSec(m_TimeCountdown) + m_PosInfix;
					}
				}
			}
		});
	}

	public int GetTimeLeft()
	{
		return m_TimeCountdown;	
	}

	public void SetText(string text)
	{
		m_TimeCountdown = -1;
		m_TextTime.text = text;
	}

	public void SetCountDownToZeroCallback(System.Action cb)
	{
		m_CountDownToZeroCb = cb;
	}

	public void SetCountdownTime(int time,string desc="", string infix="",string posInfix="",bool haveOnlySec=false)
	{
		m_Infix = infix;
		m_PosInfix = posInfix;
		m_TimeCountdown = time;
		this.haveOnlySec = haveOnlySec;
		if (time > 0)
		{
			if (haveOnlySec)
			{
				m_TextTime.text = m_Infix + time + m_PosInfix;
			}
			else
			{
				m_TextTime.text = m_Infix + TimeUtils.FormatHourFromSec(time) + m_PosInfix;
			}
		} else
		{
			m_TextTime.text = desc;
		}
	}

	public void Pause(bool setPause) 
	{
		this.setPause = setPause;
	}

	// Update is called once per frame
	void Update () 
	{
		if (m_Timer != null)
		{
			m_Timer.UpdateTimer (Time.deltaTime);		
		}
	}
}
