using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.U2D;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using AOCSCore.SafeAreaUI;
using Object = System.Object;

public class SharedUtils
{
    public static void LoadSpineSkeletonCanvas(SkeletonGraphic ani, string asset, string animation = "",
        bool loop = true)
    {
        //Debug.Log ("--- Load spine: " + asset);
        string path = asset + "_SkeletonData";
        //string path2 = asset + "_Material";
        UnityEngine.Object ret = Resources.Load<UnityEngine.Object>(path);
        ani.skeletonDataAsset = (Spine.Unity.SkeletonDataAsset)ret;
        //UnityEngine.Object retMat = Resources.Load<UnityEngine.Object>(path2);
        //ani.material = (Material)retMat;
        ani.Initialize(true);
        if (!string.IsNullOrEmpty(animation))
        {
            ani.startingAnimation = animation;
            ani.AnimationState.SetAnimation(0, animation, loop);
        }
    }

    public static void
        LoadSpineSkeletonGraphic(SkeletonGraphic ani, string pathSkeletonData) //;, string animation, bool loop)
    {
        string path = pathSkeletonData + "_SkeletonData";
        UnityEngine.Object ret = Resources.Load<UnityEngine.Object>(path);
        ani.skeletonDataAsset = (Spine.Unity.SkeletonDataAsset)ret;
        ani.Initialize(true);
    }

    public static void
        LoadSpineSkeleton(SkeletonAnimation ani, string pathSkeletonData) //;, string animation, bool loop)
    {
        UnityEngine.Object ret = Resources.Load<UnityEngine.Object>(pathSkeletonData + "_SkeletonData");
        ani.skeletonDataAsset = (Spine.Unity.SkeletonDataAsset)ret;
        ani.Initialize(true);
        //ani.AnimationState.SetAnimation (0, animation, loop);
    }

    public string GetLastElementInPath(string path)
    {
        string ret = "";
        string[] arrStr = path.Split('/');
        if (arrStr.Length > 0)
        {
            ret = arrStr[arrStr.Length - 1];
        }

        //
        return ret;
    }

    public static byte[] ObjectToByteArray(Object obj)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
    }

    // Convert a byte array to an Object
    public static Object ByteArrayToObject(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return obj;
        }
    }

    public static float getAngle(Vector3 fromPosition, Vector3 toPosition)
    {
        Vector3 targetDir = toPosition - fromPosition;


        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        if (angle < 0f) angle += 360f;

        return angle;
    }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static PaddingInfo CalculateSafePadding(Vector2 canvasSize)
    {
        int screenW = Screen.width;
        int screenH = Screen.height;
        //
        Rect rectSafe = Screen.safeArea;
        //test
        //rectSafe = new Rect(123,63,2172,1062);
        //
        float paddingLeftPx = rectSafe.x;
        float paddingTopPx = rectSafe.y;
        //
        float paddingRightPx = screenW - paddingLeftPx - rectSafe.width;
        float paddingBotPx = screenH - paddingTopPx - rectSafe.height;
        //
        //int canvasH = Constants.CANVAS_HEIGHT;
        //int canvasW = (int)(1f * canvasH * screenW / screenH);
        //
        float paddingLeft = (paddingLeftPx / screenW * canvasSize.x);
        float paddingRight = (paddingRightPx / screenW * canvasSize.x);
        float paddingTop = (paddingTopPx / screenH * canvasSize.y);
        float paddingBottom = (paddingBotPx / screenH * canvasSize.y);
        //
        string strDebug = "";
        strDebug += "ScreenW: " + screenW;
        strDebug += "\n" + "ScreenH: " + screenH;
        strDebug += "\n" + "RectSafeX: " + rectSafe.x;
        strDebug += "\n" + "RectSafeY: " + rectSafe.y;
        strDebug += "\n" + "RectSafeW: " + rectSafe.width;
        strDebug += "\n" + "RectSafeH: " + rectSafe.height;
        strDebug += "\n" + "=============";
        strDebug += "\n" + "PaddingLeft: " + paddingTopPx;
        strDebug += "\n" + "PaddingRight: " + paddingRightPx;
        strDebug += "\n" + "PaddingTop: " + paddingTopPx;
        strDebug += "\n" + "PaddingBottom: " + paddingBotPx;
        //
        PaddingInfo padding = new PaddingInfo(paddingLeft, paddingRight, paddingTop, paddingBottom);
        padding.m_StrDebug = strDebug;
        return padding;
    }

    #region FORMAT_NUMBER

    private static string FormatPrefixZero(long value)
    {
        if (value < 10)
        {
            return "0" + value;
        }
        else
        {
            return value.ToString();
        }
    }

    public static string FormatA(long value, bool roundUp = false)
    {
        if (value >= 1000000000)
            return FormatB(value, roundUp);
        else if (value >= 1000000)
            return FormatM(value, roundUp);
        else
            return FormatK(value, roundUp);
    }

    public static string FormatM(long value, bool roundUp = false)
    {
        // value = 10000012;
        //1m
        if (value < 1000000)
        {
            return FormatK(value, roundUp);
        }
        else
        {
            if (roundUp)
            {
                long tmp1 = value / 1000000;
                return FormatThreeDot(tmp1) + "M";
            }
            else
            {
                float tmp = value * 1f / 1000000;
                //    //
                bool isInteger = (tmp == Mathf.Floor(tmp));
                if (isInteger)
                {
                    return tmp + "M";
                }
                else
                {
                    string strM = tmp.ToString("0.0");
                    if (strM.EndsWith(".0") && strM.Length > 2)
                    {
                        strM = strM.Substring(0, strM.Length - 2);
                    }

                    return strM + "M";
                }
            }
        }

        // return value.ToString();
    }

    public static string FormatB(long value, bool roundUp = false)
    {
        if (roundUp)
        {
            long tmp1 = value / 1000000000;
            return FormatThreeDot(tmp1) + "B";
        }
        else
        {
            float tmp = value * 1f / 1000000000;
            //    //
            bool isInteger = (tmp == Mathf.Floor(tmp));
            if (isInteger)
            {
                return tmp + "B";
            }
            else
            {
                string strM = tmp.ToString("0.0");
                if (strM.EndsWith(".0") && strM.Length > 2)
                {
                    strM = strM.Substring(0, strM.Length - 2);
                }

                return strM + "B";
            }
        }
    }

    public static string FormatK(long value, bool roundUp = false)
    {
        if (value < 10000)
        {
            return FormatThreeDot(value);
        }
        else
        {
            if (roundUp)
            {
                long tmp1 = value / 1000;
                return FormatThreeDot(tmp1) + "K";
            }
            else
            {
                float tmp = value * 1f / 1000;
                //
                bool isInteger = (tmp == Mathf.Floor(tmp));
                if (isInteger)
                {
                    return tmp + "K";
                }
                else
                {
                    string strK = tmp.ToString("0.0");
                    if (strK.EndsWith(".0") && strK.Length > 2)
                    {
                        strK = strK.Substring(0, strK.Length - 2);
                    }

                    //
                    return strK + "K";
                }
            }
        }
    }

    public static string FormatVan(long value)
    {
        if (value < 1000000)
        {
            return FormatThreeDot(value);
        }
        else
        {
            long tmp = value / 10000;
            return FormatThreeDot(tmp) + " vạn";
        }
    }

    public static string FormatFloat(float value, int numberDigitAfterComma = 1)
    {
        if (numberDigitAfterComma <= 0)
        {
            return ((int)value).ToString();
        }
        else
        {
            string strFormat = "0.";
            for (int i = 0; i < numberDigitAfterComma; i++)
            {
                strFormat = strFormat + "0";
            }

            //
            return value.ToString(strFormat);
        }
    }

    public static string FormatThreeDot(long value)
    {
        //tam thoi bo di
        return value.ToString();
        //string sign = "";
        //if (value < 0) {
        //	sign = "-";
        //}
        ////
        //long absValue = value;
        //if (value < 0) {
        //	absValue *= -1;
        //}
        ////
        //string strValue = absValue.ToString ();
        //int length = strValue.Length;
        ////
        //int count = 0;
        //StringBuilder builder = new StringBuilder ();
        //for (int i = length - 1; i >= 0; i--) {
        //	builder.Insert (0, strValue [i]);
        //	count++;
        //	if (count == 3 && i > 0) {
        //		builder.Insert (0, ".");
        //		count = 0;
        //	}
        //}
        ////
        //return sign + builder.ToString ();
    }

    public static string FormatThousandSeperator(long value)
    {
        string sign = "";
        if (value < 0)
        {
            sign = "-";
        }

        //
        long absValue = value;
        if (value < 0)
        {
            absValue *= -1;
        }

        //
        string strValue = absValue.ToString();
        int length = strValue.Length;
        //
        int count = 0;
        StringBuilder builder = new StringBuilder();
        for (int i = length - 1; i >= 0; i--)
        {
            builder.Insert(0, strValue[i]);
            count++;
            if (count == 3 && i > 0)
            {
                builder.Insert(0, ".");
                count = 0;
            }
        }

        //
        return sign + builder.ToString();
    }

    #endregion

    public static GameObject LoadPrefab(string pathPrefab)
    {
        GameObject prefab = Resources.Load<GameObject>(pathPrefab);
        if (prefab != null)
        {
            return prefab;
        }
        else
        {
            Debug.Log("------- No prefab :" + pathPrefab);
            return null;
        }
    }

    public static T RandomListValue<T>(List<T> lst)
    {
        if (lst == null || lst.Count == 0)
            return default(T);
        //
        int count = lst.Count;
        int rnd = SharedUtils.RandomInt(0, count - 1);
        //
        return lst[rnd];
    }

    public static bool GetAChance(int percent)
    {
        int tmp = RandomInt(0, 100);
        return tmp < percent;
    }

    public static float RandomFloat(float from, float to)
    {
        return UnityEngine.Random.Range(from, to);
    }

    public static int RandomInt(int from, int to)
    {
        if (to - from <= 1)
        {
            return from;
        }

        return UnityEngine.Random.Range(from, to);
    }

    public static List<string> ListLongToListString(List<long> lstInput)
    {
        List<string> lstString = new List<string>();
        foreach (long l in lstInput)
        {
            lstString.Add(l.ToString());
        }

        //
        return lstString;
    }

    public static List<int> ListLongToListInt(List<long> lstInput)
    {
        List<int> lstInt = new List<int>();
        foreach (long l in lstInput)
        {
            lstInt.Add((int)l);
        }

        //
        return lstInt;
    }

    public static List<long> ListIntToListLong(List<int> lstInput)
    {
        List<long> lstLong = new List<long>();
        foreach (int i in lstInput)
        {
            lstLong.Add(i);
        }

        //
        return lstLong;
    }

    public static string ConvertListToArrayJson<T>(List<T> list)
    {
        if (list == null)
        {
            return "[]";
        }

        string arrList = "[";
        for (int i = 0; i < list.Count; i++)
        {
            if (i < list.Count - 1)
            {
                arrList = arrList + list[i].ToString() + ",";
            }
            else
            {
                arrList = arrList + list[i].ToString();
            }
        }

        arrList = arrList + "]";
        return arrList;
    }

    public static string ConvertListToString<T>(List<T> list)
    {
        string arrList = "[";
        for (int i = 0; i < list.Count; i++)
        {
            if (i < list.Count - 1)
            {
                arrList = arrList + list[i].ToString() + ",";
            }
            else
            {
                arrList = arrList + list[i].ToString();
            }
        }

        arrList = arrList + "]";
        return arrList;
    }


    public static void PrintList<T>(List<T> list, bool error = false)
    {
        string tmp = "";
        for (int i = 0; i < list.Count; i++)
        {
            if (i < list.Count - 1)
            {
                tmp = tmp + list[i].ToString() + ",";
            }
            else
            {
                tmp = tmp + list[i].ToString();
            }
        }

        //
        if (error)
        {
            Debug.Log(tmp);
        }
        else
        {
            Debug.Log(tmp);
        }
    }

    public static void PrintArrayInt(int[] list, bool isError = false)
    {
        string tmp = "";
        for (int i = 0; i < list.Length; i++)
        {
            if (i < list.Length - 1)
            {
                tmp = tmp + list[i].ToString() + ",";
            }
            else
            {
                tmp = tmp + list[i].ToString();
            }
        }

        //
        if (isError)
        {
            Debug.Log(tmp);
        }
        else
        {
            Debug.Log(tmp);
        }
    }

    public static List<long> ConvertListIntToLong(List<int> lstInt)
    {
        List<long> ret = new List<long>();
        //
        foreach (int i in lstInt)
        {
            ret.Add(i);
        }

        //
        return ret;
    }

    //
    public static List<int> ConvertListLongToInt(List<long> lstLong)
    {
        List<int> ret = new List<int>();
        //
        foreach (long l in lstLong)
        {
            ret.Add((int)l);
        }

        //
        return ret;
    }

    public static T GetListValue<T>(List<T> lstValue, int index)
    {
        if (lstValue == null)
            return default(T);
        //
        if (lstValue.Count > index)
        {
            return lstValue[index];
        }

        return default(T);
    }

    public static void ShuffleList<T>(List<T> lstInput)
    {
        if (lstInput.Count <= 1)
        {
            return;
        }

        for (int i = 0; i < lstInput.Count; i++)
        {
            int idxRnd = RandomInt(1, lstInput.Count - 1);
            T tmp = lstInput[i];
            lstInput[i] = lstInput[idxRnd];
            lstInput[idxRnd] = tmp;
        }
    }

    public static List<T> CloneList<T>(List<T> lstInput)
    {
        List<T> ret = null;
        if (lstInput == null)
        {
            return ret;
        }

        //
        ret = new List<T>();
        for (int i = 0; i < lstInput.Count; i++)
        {
            ret.Add(lstInput[i]);
        }

        //
        return ret;
    }

    public static string GetLastPathElement(string pathInput)
    {
        if (string.IsNullOrEmpty(pathInput))
        {
            return "";
        }

        //
        string[] arrElm = pathInput.Split('/');
        //
        if (arrElm.Length > 0)
        {
            return arrElm[arrElm.Length - 1];
        }
        else
        {
            return "";
        }
    }

    public static void SetImgSizeNativeWithPercent(Image img, float percent)
    {
        img.SetNativeSize();
        //
        RectTransform rect = img.GetComponent<RectTransform>();
        Vector2 nativeSize = rect.sizeDelta;
        rect.sizeDelta = new Vector2(nativeSize.x * percent, nativeSize.y * percent);
    }
}