using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AOCSCore.SafeAreaUI
{
    public class SafeAreaAdapterV1 : MonoBehaviour
    {
        public Canvas m_Canvas;

        //
        public AnchorType m_AnchorType = AnchorType.NONE;

        // Start is called before the first frame update
        void Start()
        {
            DoIt();
        }

        private void DoIt()
        {
            if (m_Canvas == null)
            {
                GameObject goCanvasTmp = CanvasHelper.GetCanvas();
                if (goCanvasTmp == null || goCanvasTmp.GetComponent<Canvas>() == null)
                {
                    return;
                }
                else
                {
                    m_Canvas = goCanvasTmp.GetComponent<Canvas>();
                }
            }

            if (m_AnchorType == AnchorType.NONE)
                return;
            //
            RectTransform rect = m_Canvas.GetComponent<RectTransform>();
            float w = rect.rect.width;
            float h = rect.rect.height;
            //
            Vector2 cvSize = new Vector2(w, h);
            //Debug.LogFormat("--------- Cv size for safe area {0},{1}", w, h);
            //
            PaddingInfo safePadding = SharedUtils.CalculateSafePadding(cvSize);
            //adjust padding

            safePadding.m_Left = 0.75f * safePadding.m_Left;
            safePadding.m_Right = 0.75f * safePadding.m_Right;
            safePadding.m_Bottom = 0.75f * safePadding.m_Bottom;
            safePadding.m_Top = 0.75f * safePadding.m_Top;
            //test
#if UNITY_EDITOR
//        safePadding.m_Left = 50;
//        safePadding.m_Right = 50;
//        safePadding.m_Bottom = 50;
//        safePadding.m_Top = 50;
#endif
            //
            float xOffset = 0;
            float yOffset = 0;
            //
            switch (m_AnchorType)
            {
                case AnchorType.TOP:
                {
                    yOffset = -safePadding.m_Top;
                    break;
                }
                case AnchorType.BOTTOM:
                {
                    yOffset = safePadding.m_Top;
                    break;
                }
                case AnchorType.LEFT:
                {
                    xOffset = safePadding.m_Left;
                    break;
                }
                case AnchorType.RIGHT:
                {
                    xOffset = -safePadding.m_Right;
                    break;
                }
                case AnchorType.TOP_LEFT:
                {
                    xOffset = safePadding.m_Left;
                    yOffset = -safePadding.m_Top;
                    break;
                }
                case AnchorType.TOP_RIGHT:
                {
                    xOffset = -safePadding.m_Right;
                    yOffset = -safePadding.m_Top;
                    break;
                }
                case AnchorType.BOTTOM_LEFT:
                {
                    xOffset = safePadding.m_Left;
                    yOffset = safePadding.m_Bottom;
                    break;
                }
                case AnchorType.BOTTOM_RIGHT:
                {
                    xOffset = -safePadding.m_Right;
                    yOffset = safePadding.m_Bottom;
                    break;
                }
            }

            //
            Vector2 currentPos = CanvasHelper.GetPosition(gameObject);
            Vector2 newPos = currentPos + new Vector2(xOffset, yOffset);
            //
            CanvasHelper.SetPosition(gameObject, newPos);
        }
    }

    public enum AnchorType
    {
        NONE = 0,
        TOP_LEFT = 1,
        TOP_RIGHT = 2,
        BOTTOM_LEFT = 3,
        BOTTOM_RIGHT = 4,

        //
        LEFT = 5,
        RIGHT = 6,
        TOP = 7,
        BOTTOM = 8
    }

    public class PaddingInfo
    {
        public float m_Left = 0;
        public float m_Right = 0;
        public float m_Bottom = 0;

        public float m_Top = 0;

        //
        public string m_StrDebug = "";

        public PaddingInfo(float left, float right, float top, float bottom)
        {
            m_Left = left;
            m_Right = right;
            m_Bottom = bottom;
            m_Top = top;
        }
    }
}