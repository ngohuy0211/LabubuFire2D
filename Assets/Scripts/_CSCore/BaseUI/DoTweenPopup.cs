using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DoTweenPopup : MonoBehaviour
{
    public PopupShowOptions popupShowType = PopupShowOptions.SCALE_AND_FADE;
    public float delay = 0f, delay1 = 0f, delay2 = 0f;
    public float duration = 0.2f, durationFade = 0.1f, duration1 = 0.2f, duration2 = 0.2f;
    public Vector2 startPoint, startPoint1;
    public Vector2 startScale;
    [Range(0f, 1f)] public float startFade;
    public Vector3 startRotate;
    public Color startColor = Color.white;
    public Ease open = Ease.OutCubic, open1 = Ease.OutCubic, open2 = Ease.OutCubic;
    public Ease close = Ease.OutCubic, close1 = Ease.OutCubic, close2 = Ease.OutCubic;

    private Vector2 donePos;
    private RectTransform target;
    private Vector2 doneScale;
    private CanvasGroup cvg;
    private Graphic graphic;
    private Action openCb, closeCb;
    private ScrollRect[] arrScroll;

    public void ShowPopup(Action openDoneCB)
    {
        target = this.GetComponent<RectTransform>();
        donePos = new Vector2(target.anchoredPosition.x, target.anchoredPosition.y);
        doneScale = new Vector2(target.localScale.x, target.localScale.y);
        this.openCb = openDoneCB;
        switch (popupShowType)
        {
            case PopupShowOptions.TRANSITION:
                target.anchoredPosition = startPoint;
                target?.DOAnchorPos(donePos, duration)
                            .SetEase(open)
                            .SetDelay(delay)
                            .OnComplete(() => openDoneCB?.Invoke());
                break;
            case PopupShowOptions.SCALE_AND_FADE:
                cvg = cvg == null ? gameObject.AddComponent<CanvasGroup>() : cvg;
                cvg.alpha = startFade;
                cvg?.DOFade(1f, durationFade);
                target.localScale = startScale;
                target?.DOScale(doneScale, duration)
                    .SetEase(open)
                    .SetDelay(delay)
                    .OnComplete(() => openDoneCB?.Invoke()); 
                break;
            case PopupShowOptions.SCALE:
                target.localScale = startScale;
                target?.DOScale(doneScale, duration)
                            .SetEase(open)
                            .SetDelay(delay)
                            .OnComplete(() => openDoneCB?.Invoke()); 
                break;
            case PopupShowOptions.FADE:
                cvg = cvg == null ? gameObject.AddComponent<CanvasGroup>() : cvg;
                cvg.alpha = startFade;
                cvg?.DOFade(1f, duration)
                            .SetEase(open)
                            .SetDelay(delay)
                            .OnComplete(() => openDoneCB?.Invoke());
                break;
            case PopupShowOptions.ROTATE:
                target.localEulerAngles = (startRotate);
                target?.DOLocalRotate(Vector3.one, duration)
                            .SetEase(open)
                            .SetDelay(delay)
                            .OnComplete(() => openDoneCB?.Invoke());
                break;
            case PopupShowOptions.COLOR:
                graphic = graphic == null ? graphic = GetComponent<Graphic>() : graphic;
                graphic?.DOColor(Color.white, duration)
                            .SetEase(open)
                            .SetDelay(delay)
                            .OnComplete(() => openDoneCB?.Invoke());
                break;
            case PopupShowOptions.SCALE_TRANSITION:
                target.localScale = startScale;
                target?.DOScale(doneScale, duration)
                           .SetEase(open)
                           .SetDelay(delay);
                target.anchoredPosition = startPoint;
                target?.DOAnchorPos(donePos, duration1)
                            .SetEase(open1)
                            .SetDelay(delay1);
                float maxDelay = Utils.getMax(delay + duration, delay1 + duration1);
                if (openDoneCB != null) Invoke("OpenCB", maxDelay);
                break;
            case PopupShowOptions.TRANS_SCALE_AND_FADE:
                target.localScale = new Vector2(0.1f, 0.1f);
                target.anchoredPosition = startPoint;
                target?.DOScale(startScale, duration).SetEase(open).SetDelay(delay);
                target?.DOAnchorPos(startPoint1, duration)
                            .SetEase(open)
                            .SetDelay(delay)
                            .OnComplete(() =>
                            {
                                target?.DOScale(doneScale, duration1)
                                                         .SetEase(open1)
                                                         .SetDelay(delay1);
                                target?.DOAnchorPos(donePos, duration2)
                                                          .SetEase(open2)
                                                          .SetDelay(delay2);
                            });
                maxDelay = Utils.getMax(delay + duration + delay1 + duration1, delay + duration + delay2 + duration2);
                if (openDoneCB != null) Invoke("OpenCB", maxDelay);
                cvg = cvg == null ? gameObject.AddComponent<CanvasGroup>() : cvg;
                cvg.alpha = startFade;
                cvg?.DOFade(1f, durationFade);
                break;
            default:
                openDoneCB?.Invoke();
                break;
        }
    }

    public void ClosePopup(Action closeCB)
    {
        if (target == null)
        {
            closeCB?.Invoke();
        }
        else
        {
            this.closeCb = closeCB;
            switch (popupShowType)
            {
                case PopupShowOptions.TRANSITION:
                    target?.DOAnchorPos(startPoint, duration)
                                .SetEase(close)
                                .SetDelay(delay)
                                .OnComplete(() =>
                                {
                                    closeCB?.Invoke();
                                });
                    break;
                case PopupShowOptions.SCALE_AND_FADE:
                    cvg = cvg == null ? gameObject.AddComponent<CanvasGroup>() : cvg;
                    cvg?.DOFade(0f, durationFade);
                    target?.DOScale(startScale, duration)
                        .SetEase(close)
                        .SetDelay(delay)
                        .OnComplete(() =>
                        {
                            closeCB?.Invoke();
                        });
                    break;
                case PopupShowOptions.SCALE:
                    target?.DOScale(startScale, duration)
                                .SetEase(close)
                                .SetDelay(delay)
                                .OnComplete(() =>
                                {
                                    closeCB?.Invoke();
                                });
                    break;
                case PopupShowOptions.FADE:
                    cvg = cvg == null ? gameObject.AddComponent<CanvasGroup>() : cvg;
                    cvg?.DOFade(startFade, duration)
                                .SetEase(close)
                                .SetDelay(delay)
                                .OnComplete(() =>
                                {
                                    closeCB?.Invoke();
                                });
                    break;
                case PopupShowOptions.ROTATE:
                    target?.DOLocalRotate(startRotate, duration)
                                .SetEase(close)
                                .SetDelay(delay)
                                .OnComplete(() =>
                                {
                                    closeCB?.Invoke();
                                });
                    break;
                case PopupShowOptions.COLOR:
                    graphic = graphic == null ? graphic = GetComponent<Graphic>() : graphic;
                    graphic?.DOColor(startColor, duration)
                                .SetEase(close)
                                .SetDelay(delay)
                                .OnComplete(() => closeCB?.Invoke());
                    break;
                case PopupShowOptions.SCALE_TRANSITION:
                    float maxDu = Utils.getMax(duration, duration1);
                    target?.DOScale(startScale, duration)
                               .SetEase(close);
                    target?.DOAnchorPos(startPoint, duration1)
                                .SetEase(close1);
                    if (closeCB != null) Invoke("CloseCB", maxDu);
                    break;
                case PopupShowOptions.TRANS_SCALE_AND_FADE: // 3 tham số
                    maxDu = Utils.getMax(duration1, duration2);
                    target?.DOScale(startScale, duration1)
                               .SetEase(close1);
                    target?.DOAnchorPos(startPoint1, duration2)
                                .SetEase(close2);
                    target?.DOAnchorPos(startPoint, duration).SetEase(close).SetDelay(maxDu);
                    target?.DOScale(new Vector2(0.05f, 0.03f), duration).SetEase(close).SetDelay(maxDu);

                    if (closeCB != null) Invoke("CloseCB", duration + maxDu);
                    
                    cvg = cvg == null ? gameObject.AddComponent<CanvasGroup>() : cvg;
                    cvg?.DOFade(0f, durationFade);
                    break;
                default:
                    closeCB?.Invoke();
                    break;
            }
        }
    }

    void CloseCB()
    {
        closeCb?.Invoke();
        // #if UNITY_EDITOR
        //         Debug.Log("close call back");
        // #endif
    }

    void OpenCB()
    {
        openCb?.Invoke();
        // #if UNITY_EDITOR
        //         Debug.Log("OpenCB call back");
        // #endif
    }

#if UNITY_EDITOR  // Chỉ dùng cho editor thôi
    public void SetStartPoint() // only editor
    {
        startPoint = this.GetComponent<RectTransform>().anchoredPosition;
    }
    public void SetStartPoint1() // only editor
    {
        startPoint1 = this.GetComponent<RectTransform>().anchoredPosition;
    }
    public void SetStartScale() // only editor
    {
        startScale = this.GetComponent<RectTransform>().localScale;
    }
    public void SetStartRotate() // only editor
    {
        startRotate = this.GetComponent<RectTransform>().localRotation.eulerAngles;
    }
#endif
}
