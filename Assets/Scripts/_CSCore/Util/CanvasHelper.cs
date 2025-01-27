using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasHelper : MonoBehaviour
{
    public static Vector2 GetCanvasSize(GameObject goCanvas = null)
    {
        if (goCanvas == null)
        {
            goCanvas = GameObject.Find("Canvas");
        }

        //
        if (goCanvas == null)
        {
            Debug.LogError("----- Can not find canvas on scene");
            return Vector2.one;
        }

        //
        RectTransform rectTf = goCanvas.GetComponent<RectTransform>();
        return rectTf.sizeDelta;
    }


    public static GameObject GetCanvas()
    {
        GameObject goCanvas = GameObject.Find("Canvas");
        GameObject goCanvasPopup = GameObject.Find("CanvasPopup");
        GameObject goCanvasExpand = GameObject.Find("CanvasExpand");
        GameObject goCanvasTopMost = GameObject.Find("CanvasTopMost");
        //
        if (goCanvasTopMost != null) return goCanvasTopMost;
        else if (goCanvasExpand != null) return goCanvasExpand;
        else if (goCanvasPopup != null) return goCanvasPopup;
        else if (goCanvas != null) return goCanvas;

        return null;
    }

    public static Vector2 GetSize(GameObject go)
    {
        RectTransform rectTf = go.GetComponent<RectTransform>();
        if (rectTf != null)
        {
            return new Vector2(rectTf.rect.width, rectTf.rect.height);
        }
        else
        {
            return Vector2.one;
        }
    }

    public static void SetSize(GameObject go, Vector2 size)
    {
        RectTransform rectTf = go.GetComponent<RectTransform>();

        if (rectTf != null)
        {
            rectTf.sizeDelta = size;
        }
    }

    public static Vector2 GetPosition(GameObject go)
    {
        RectTransform rectTf = go.GetComponent<RectTransform>();
        if (rectTf != null)
        {
            return rectTf.anchoredPosition;
        }
        else
        {
            return Vector2.zero;
        }
    }

    public static void DestroyObject(GameObject go)
    {
        if (go == null)
            return;
        //
        GameObject.Destroy(go);
    }

    public static void SetPosition(GameObject go, Vector2 position)
    {
        RectTransform rectTf = go.GetComponent<RectTransform>();
        if (rectTf != null)
        {
            rectTf.anchoredPosition = position;
        }
    }


    public static void SetPosition(MonoBehaviour mono, Vector2 position)
    {
        RectTransform rectTf = mono.gameObject.GetComponent<RectTransform>();
        if (rectTf != null)
        {
            rectTf.anchoredPosition = position;
        }
    }

    public static void SetScale(MonoBehaviour mono, float scale)
    {
        RectTransform rectTf = mono.gameObject.GetComponent<RectTransform>();
        if (rectTf != null)
        {
            rectTf.localScale = new Vector3(scale, scale, 1f);
        }
    }


    public static void SetScale(GameObject go, float scale)
    {
        RectTransform rectTf = go.GetComponent<RectTransform>();
        if (rectTf != null)
        {
            rectTf.localScale = new Vector3(scale, scale, 1f);
        }
    }

    public static void SetScale(GameObject go, Vector2 scale)
    {
        RectTransform rectTf = go.GetComponent<RectTransform>();
        if (rectTf != null)
        {
            rectTf.localScale = new Vector3(scale.x, scale.y, 1f);
        }
    }

    public static Vector2 GetPosition(MonoBehaviour mono)
    {
        RectTransform rectTf = mono.gameObject.GetComponent<RectTransform>();
        if (rectTf != null)
        {
            return rectTf.anchoredPosition;
        }
        else
        {
            return Vector2.zero;
        }
    }

    public static void ClearChild(GameObject gameObject)
    {
        if (gameObject == null)
            return;
        //
        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        //
        gameObject.transform.DetachChildren();
    }


    public static void ClearChild(Transform transform)
    {
        if (transform == null)
            return;
        //
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        //
        transform.DetachChildren();
    }

    public static void DisableButton(MonoBehaviour obj)
    {
        if (obj == null)
            return;
        Button btn = obj.GetComponent<Button>();
        if (btn != null)
            btn.interactable = false;
    }

    public static void EnableButton(MonoBehaviour obj)
    {
        if (obj == null)
            return;
        Button btn = obj.GetComponent<Button>();
        if (btn != null)
            btn.interactable = true;
    }

    public static void DisableButton(GameObject goButton)
    {
        if (goButton == null)
            return;
        Button btn = goButton.GetComponent<Button>();
        if (btn != null)
            btn.interactable = false;
    }

    public static void EnableButton(GameObject goButton)
    {
        if (goButton == null)
            return;
        Button btn = goButton.GetComponent<Button>();
        if (btn != null)
            btn.interactable = true;
    }
}