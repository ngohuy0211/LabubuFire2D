using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public abstract class HorizontalOrVerticalPoolGroup : BasePoolGroup
{
    //
    // Constructors
    //
    protected HorizontalOrVerticalPoolGroup()
    {
    }

    //
    // Method
    //

    /// <summary>
    /// <para>Define the way how you use multiple cell size.</para>
    /// <para></para>
    /// <para>Param: delegate (int 'index of element in adapter')</para>
    /// </summary>
    public void howToUseCellSize(CellSizeDelegate func)
    {
        cellSizeCallback = func;
    }

    public virtual void scrollTo(int index, float duration = 0f, bool cancelIfFitInMask = false)
    {
    }

    /// <summary>
    /// <para>Return size of element at 'index' of adapter.</para>
    /// <para></para>
    /// <para>Param: Index of element in adapter</para>
    /// </summary>
    protected Vector2 getElementSize(int index)
    {
        if (cellSizeCallback != null)
            return cellSizeCallback(index);
        return getCellSize();
    }

    protected void checkToAddAtleast3Cells()
    {
        if (adapter.Count == 0)
            return;
        int _1stIndex = 0;
        int _2ndIndex = 1;
        int _3rdIndex = 2;

        bool _1stFound = false;
        bool _2ndFound = false;
        bool _3rdFound = false;

        foreach (UIPoolObject po in listPool)
        {
            if (po.index == _1stIndex)
                _1stFound = true;
            else if (po.index == _2ndIndex)
                _2ndFound = true;
            else if (po.index == _3rdIndex)
                _3rdFound = true;
        }

        if (_2ndIndex > (adapter.Count - 1))
            _2ndFound = true;
        if (_3rdIndex > (adapter.Count - 1))
            _3rdFound = true;

        if (!_1stFound)
            getPooledObject(_1stIndex);
        if (!_2ndFound)
            getPooledObject(_2ndIndex);
        if (!_3rdFound)
            getPooledObject(_3rdIndex);
    }

    //
    // Overide
    //
    public override void setAdapter(List<object> adapter, bool toFirst = true)
    {
        base.setAdapter(adapter, toFirst);
        //==
        Canvas.ForceUpdateCanvases();
        calcSizeDelta();
        resetPool();
        checkToAddAtleast3Cells();
        updateData();
        //==
        if (toFirst)
            scrollToFirst();

        ActivateAllObjects(false);

        if (isActiveAndEnabled && canShowAnim)
        {
            if (m_TypeShow == PoolTypeShow.STEP_BY_STEP)
            {
                m_ScrollRect.enabled = false;
                StartCoroutine(DelayActive());
            }
            else if (m_TypeShow == PoolTypeShow.STEP_MIX_SCALE)
            {
                m_ScrollRect.enabled = false;
                StartCoroutine(DelayActiveAndScale());
            }
            else if (m_TypeShow == PoolTypeShow.STEP_MIX_FADE)
            {
                m_ScrollRect.enabled = false;
                StartCoroutine(DelayActiveAndFade());
            }
            else if (m_TypeShow == PoolTypeShow.NONE)
                ActivateAllObjects(true);

            canShowAnim = false;
        }
        else
            ActivateAllObjects(true);
    }

    /// <summary>
    /// Huy
    /// When anim is playing, block scroll
    /// Change tab = set all active true and not play anim
    /// </summary>
    /// <returns></returns>
    #region Anim Pool

    private void ActivateAllObjects(bool value)
    {
        for (int i = 0; i < adapter.Count; i++)
        {
            GameObject go = getGameObject(i);
            if (go != null) go.SetActive(value);
        }
    }

    private void OnDisable()
    {
        m_ScrollRect.enabled = true;
        ActivateAllObjects(true);
    }

    IEnumerator DelayActiveAndFade()
    {
        for (int i = 0; i < adapter.Count; i++)
        {
            yield return new WaitForSeconds(0.04f);
            GameObject go = getGameObject(i);

            if (go != null)
            {
                CanvasGroup canvas = go.gameObject.AddComponent<CanvasGroup>();
                if (canvas != null)
                {
                    canvas.alpha = 0;
                    canvas.DOFade(1, 0.3f);
                }

                go.SetActive(true);
            }

            if (i >= adapter.Count - 1)
                m_ScrollRect.enabled = true;
        }
    }

    IEnumerator DelayActiveAndScale()
    {
        for (int i = 0; i < adapter.Count; i++)
        {
            yield return new WaitForSeconds(0.04f);
            GameObject go = getGameObject(i);
            if (go != null)
            {
                go.SetActive(true);
                RectTransform rectGo = go.GetComponent<RectTransform>();
                rectGo.localScale = Vector3.zero;
                rectGo.DOScale(new Vector3(1, 1, 1), 0.5f)
                    .SetEase(Ease.OutBack);
            }

            if (i >= adapter.Count - 1)
                m_ScrollRect.enabled = true;
        }
    }

    IEnumerator DelayActive()
    {
        for (int i = 0; i < adapter.Count; i++)
        {
            yield return new WaitForSeconds(0.04f);
            GameObject go = getGameObject(i);

            if (go != null)
                go.SetActive(true);
            if (i >= adapter.Count - 1)
                m_ScrollRect.enabled = true;
        }
    }

    #endregion

    /// <summary>
    /// <para>Scroll pool group to position of last element of adapter.</para>
    /// <para></para>
    /// <para>Param: Duration for scrolling from beginning to end.</para>
    /// </summary>
    public override void scrollToLast(float duration = 0)
    {
        if (adapter.Count <= 0)
            return;
        scrollTo(adapter.Count - 1, duration, false);
    }
}
//end of class