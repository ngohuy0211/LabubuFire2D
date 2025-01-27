using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    public GameObject m_GoCanvas;

    // Use this for initialization
    void Start()
    {
        RectTransform rectBgr = GetComponent<RectTransform>();
        if (rectBgr == null)
            return;
        //
        Vector2 sizeBgr = rectBgr.sizeDelta;
        //
        Vector2 sizeCv = CanvasHelper.GetCanvasSize(m_GoCanvas);
        float scalePreferX = sizeCv.x / sizeBgr.x;
        float scalePreferY = sizeCv.y / sizeBgr.y;
        //Debug.LogFormat("------ Canvas size:{0} / {1}", sizeCv.x, sizeCv.y);
        //Debug.LogFormat("------ Background size:{0} / {1}", sizeBgr.x, sizeBgr.y);
        //Debug.Log("------ Scale prefer x,y: " + scalePreferX + ", " + scalePreferY);
        //      //
        float scale = Mathf.Max(scalePreferX, scalePreferY);
        rectBgr.localScale = new Vector3(scale, scale, 1f);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
