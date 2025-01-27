using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [SerializeField] private List<TabButton> m_LstButtonTab = new List<TabButton>();

    //
    [HideInInspector] public ITabListener m_Listener = null;

    //them kieu callback ve cung voi listener ben tren,
    //dung kieu nao cung duoc, nhung cb thi convenient hon
    public System.Action<int> m_TabClickCb = null;

    public System.Action m_TabAllClick = null;

    //
    [HideInInspector] private int m_CurrentTabIdActive = -1;

    public int m_TabId = 0;

    //
    [HideInInspector] public bool m_AllowAllTabOff = false;


    // Use this for initialization
    void Start()
    {
        foreach (TabButton tab in m_LstButtonTab)
        {
            if (tab != null)
            {
                tab.m_TabClickCallback = OnTabClick;
            }
        }
    }

    public void SetListButtonTab<T>(List<T> lstTabButton) where T : TabButton
    {
        m_LstButtonTab.Clear();
        foreach (T t in lstTabButton)
        {
            m_LstButtonTab.Add(t);
        }

        //
        foreach (TabButton tab in m_LstButtonTab)
        {
            if (tab != null)
                tab.m_TabClickCallback = OnTabClick;
        }
    }

    public List<TabButton> GetListButtonTab()
    {
        return m_LstButtonTab;
    }

    public void DisableAllTabButton()
    {
        foreach (TabButton tab in m_LstButtonTab)
        {
            if (tab != null)
            {
                Button btn = tab.GetComponent<Button>();
                if (btn != null)
                    btn.interactable = false;
            }
        }
    }

    public void HideTabButton(List<int> lstHideButtonIndex)
    {
        for (int i = 0; i < m_LstButtonTab.Count; i++)
        {
            if (lstHideButtonIndex.Contains(i))
                m_LstButtonTab[i].gameObject.SetActive(false);
        }
    }

    public void ResetTab()
    {
        m_CurrentTabIdActive = -1;
        //
        for (int i = 0; i < m_LstButtonTab.Count; i++)
            m_LstButtonTab[i].gameObject.SetActive(true);

        //
        foreach (TabButton tab in m_LstButtonTab)
            tab.SetOn(false);
    }

    public void SetAllActive()
    {
        m_CurrentTabIdActive = -1;
        foreach (TabButton tab in m_LstButtonTab)
            tab.SetOn(true);
    }

    public void AddTabButton(TabButton tab)
    {
        m_LstButtonTab.Add(tab);
    }

    public int GetCurButtonTabActive()
    {
        return m_CurrentTabIdActive;
    }

    public void SetTabActive(int buttonTabId)
    {
        m_CurrentTabIdActive = buttonTabId;
        //
        foreach (TabButton tab in m_LstButtonTab)
            tab.SetOn(tab.ButtonTabId == buttonTabId);
    }

    public void SetActionTabAllClick(System.Action cb)
    {
        m_TabAllClick = cb;
    }

    public void OnTabClick(int buttonTabId)
    {
        if (!m_AllowAllTabOff)
        {
            if (m_CurrentTabIdActive == buttonTabId)
            {
                if (m_TabAllClick != null) m_TabAllClick();
                if (m_TabClickCb != null) m_TabClickCb(buttonTabId);
                return;
            }

            m_CurrentTabIdActive = buttonTabId;
            //
            foreach (TabButton tab in m_LstButtonTab)
            {
                if (tab.ButtonTabId == buttonTabId)
                    tab.SetOn(true);
                else
                    tab.SetOn(false);
            }

            //
            if (m_Listener != null) m_Listener.OnTabActive(this.m_TabId, buttonTabId);
            if (m_TabClickCb != null) m_TabClickCb(buttonTabId);
        }
        else
        {
            //
            foreach (TabButton tab in m_LstButtonTab)
            {
                if (tab.ButtonTabId == buttonTabId)
                {
                    tab.SetOn(!tab.m_IsOn);
                    if (m_Listener != null) m_Listener.OnTabActive(this.m_TabId, tab.m_IsOn ? -9999 : buttonTabId);
                    if (m_TabClickCb != null) m_TabClickCb(buttonTabId);
                }
                else
                    tab.SetOn(false);
            }
        }
    }
}

public interface ITabListener
{
    void OnTabActive(int tabId, int buttonTabId);
}