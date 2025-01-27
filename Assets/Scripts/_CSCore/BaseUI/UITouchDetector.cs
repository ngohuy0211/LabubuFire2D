using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITouchDetector : MonoBehaviour, IPointerDownHandler,
    IPointerUpHandler,
    IBeginDragHandler,
    IDragHandler, IEndDragHandler
{
    [HideInInspector] public RectTransform _rectContainer = null;

    //
    public System.Action<Vector2> itemDragCb;
    public System.Action<Vector2> itemDropCb;
    public System.Action itemClickCb;

    public void Awake()
    {
        _rectContainer = transform.parent.GetComponent<RectTransform>();
    }


    // Use this for initialization
    public void Start()
    {
    }

    #region Event Handler implementation

    private Vector2 _pointDown = Vector2.zero;
    private Vector2 _pointLastDrag = Vector2.zero;
    private float _timePointerDown = 0;

    public void OnPointerDown(PointerEventData pointerData)
    {
        //Debug.Log("------ On pointer down");
        //
        this.gameObject.transform.SetAsLastSibling();
        //
        Vector2 posPointer = pointerData.position;
        _pointDown = ConvertMousePointerToCanvasPosition(posPointer);
        _pointLastDrag = _pointDown;
        _timePointerDown = Time.realtimeSinceStartup;
    }

    public void OnPointerUp(PointerEventData pointerData)
    {
        //Debug.Log("------ On pointer Up");
        Vector2 posPointer = pointerData.position;
        Vector2 pointUp = ConvertMousePointerToCanvasPosition(posPointer);
        //
        float elapsed = Time.realtimeSinceStartup - _timePointerDown;
        //

        if (elapsed < 0.3f)
        {
            if (itemClickCb != null)
            {
                itemClickCb.Invoke();
            }
        }
    }


    public void OnBeginDrag(PointerEventData pointerData)
    {
        //Debug.Log("------ begin drag");
        this.gameObject.transform.SetAsLastSibling();
        //
        Vector2 posPointer = pointerData.position;
        Vector2 pointCanvas = ConvertMousePointerToCanvasPosition(posPointer);
        _pointLastDrag = pointCanvas;
    }

    public void OnDrag(PointerEventData pointerData)
    {
        Vector2 posPointer = pointerData.position;
        Vector2 pointCanvas = ConvertMousePointerToCanvasPosition(posPointer);
        //

        if (itemDragCb != null)
        {
            Vector2 offset = pointCanvas - _pointLastDrag;
            _pointLastDrag = pointCanvas;
            itemDragCb.Invoke(offset);
        }
    }

    public void OnEndDrag(PointerEventData pointerData)
    {
        Vector2 posPointer = pointerData.position;
        Vector2 pointCanvas = ConvertMousePointerToCanvasPosition(posPointer);
        if (itemDropCb != null)
        {
            itemDropCb.Invoke(pointCanvas);
        }
    }


    private Vector2 ConvertMousePointerToCanvasPosition(Vector2 mousePointer)
    {
        return UIHelper.ScreenPointToLocalPointInRect(_rectContainer, mousePointer);
        // Vector2 v = Vector2.zero;
        // RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectContainer, mousePointer,
        // 	Camera.main, out v);
        // return v;
    }

    #endregion
}