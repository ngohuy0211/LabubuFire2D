using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GuiUtil
{
    public static void CopyToClipboard(this string str)
    {
        GUIUtility.systemCopyBuffer = str;
    }
}
