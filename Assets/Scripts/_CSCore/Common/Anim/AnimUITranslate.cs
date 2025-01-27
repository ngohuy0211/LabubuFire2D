using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AnimUITranslate : MonoBehaviour
{
    [SerializeField] TranslateController m_TranslateController;

    void Start()
    {
        m_TranslateController.SetToPoint1();
        StartCoroutine(DelayShowAnim());
    }

    IEnumerator DelayShowAnim()
    {
        yield return new WaitForSeconds(1f);
        new DelayFunctionCalling(this,
            delegate { m_TranslateController.MakeTranslateShow(0.45f, Ease.OutBack); }, 0.1f).DoIt();
    }
}
