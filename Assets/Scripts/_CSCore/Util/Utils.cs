using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;

class Utils
{
    public static List<T> ToList<T>(T data)
    {
        List<T> lst = new List<T>();
        lst.Add(data);
        return lst;
    }

    public static bool IntToBool(int intValue)
    {
        return intValue == 1;
    }

    public static bool IsStringEmpty(string str)
    {
        if (str == null || str.Trim().Equals(""))
        {
            return true;
        }

        //
        return false;
    }

    public static float getMax(params float[] num)
    {
        float max = num[0];
        for (int i = 1; i < num.Length; i++)
        {
            if (max < num[i]) max = num[i];
        }

        return max;
    }

    public static float getMin(params float[] num)
    {
        if (num == null) return 0f;
        float min = num[0];
        for (int i = 1; i < num.Length; i++)
        {
            if (min > num[i]) min = num[i];
        }

        return min;
    }

    public static int getMax(params int[] num)
    {
        if (num == null) return 0;
        int max = num[0];
        for (int i = 1; i < num.Length; i++)
        {
            if (max < num[i]) max = num[i];
        }

        return max;
    }

    public static int getMin(params int[] num)
    {
        if (num == null) return 0;
        int min = num[0];
        for (int i = 1; i < num.Length; i++)
        {
            if (min > num[i]) min = num[i];
        }

        return min;
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

    public static byte[] Selialize(object data)
    {
        try
        {
            MemoryStream ms = new MemoryStream();
            var binaryWriter = new BinaryWriter(ms);
            binaryWriter.Write(data.ToString());
            binaryWriter.Flush();
            byte[] tmp = ms.ToArray();
            return tmp;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        return null;
    }

    public static object DeserializeObject(byte[] bytes)
    {
        if (bytes == null)
        {
            return null;
        }

        MemoryStream memRead = new MemoryStream(bytes);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();

            object obj = formatter.Deserialize(memRead);
            return obj;
        }
        catch (Exception e)
        {
            Debug.Log("Failed to deserialize. Reason: " + e.Message);
        }
        finally
        {
            memRead.Close();
        }

        //
        return null;
    }
    
    public static List<object> GenListObject<T>(List<T> list)
    {
        List<object> lstObj = new List<object>();
        foreach (T t in list)
        {
            lstObj.Add(t);
        }

        //
        return lstObj;
    }

    public static bool IsLongNumber(string value)
    {
        try
        {
            long x = 0L;
            return long.TryParse(value, out x);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        return false;
    }

    public static List<T> StringToList<T>(string strJsonArr)
    {
        List<T> lst = new List<T>();
        if (string.IsNullOrEmpty(strJsonArr)) return lst;
        JSONArray jsonArr = JSON.Parse(strJsonArr).AsArray;
        for (int i = 0; i < jsonArr.Count; i++)
        {
            string value = jsonArr[i].Value;
            lst.Add((T) Convert.ChangeType(value, typeof(T)));
        }

        return lst;
    }

    public static List<List<int>> StringTo2ListInt(string data)
    {
        List<List<int>> lst2 = new List<List<int>>();
        JSONArray jsonArr = JSON.Parse(data).AsArray;
        for (int i = 0; i < jsonArr.Count; i++)
        {
            JSONArray value = jsonArr[i].AsArray;
            List<int> lst = new List<int>();
            for (int j = 0; j < value.Count; j++)
            {
                lst.Add(value[j].AsInt);
            }

            lst2.Add(lst);
        }

        return lst2;
    }

    public static List<List<long>> To2ListLong(string data)
    {
        List<List<long>> lst2 = new List<List<long>>();
        JSONArray jsonArr = JSON.Parse(data).AsArray;
        for (int i = 0; i < jsonArr.Count; i++)
        {
            JSONArray value = jsonArr[i].AsArray;
            List<long> lst = new List<long>();
            for (int j = 0; j < value.Count; j++)
            {
                lst.Add(value[j].AsLong);
            }

            lst2.Add(lst);
        }

        return lst2;
    }

    public static StatusType GetStatus(int status)
    {
        if (status == (int) StatusType.LOCK) return StatusType.LOCK;
        else if (status == (int) StatusType.INPROGRESS) return StatusType.INPROGRESS;
        else if (status == (int) StatusType.CAN_COLLECT) return StatusType.CAN_COLLECT;
        else return StatusType.DONE;
    }

    public static bool BoolStatus(long status)
    {
        return status == 1;
    }


    public static List<long> JSONArray2ListLong(JSONArray jsonArr)
    {
        List<long> lst = new List<long>();
        for (int i = 0; i < jsonArr.Count; i++)
        {
            lst.Add(jsonArr[i].AsLong);
        }

        return lst;
    }

    public static bool IsOkName(string name)
    {
        if (name.Contains("<") || name.Contains(">") || name.Contains("\"") || name.Contains("\'") ||
            name.Contains("(") ||
            name.Contains(")") || name.Contains("update") || name.Contains("insert") || name.Contains("[") ||
            name.Contains("]") ||
            name.Contains("select") || name.Contains("*") || string.IsNullOrEmpty(name))
        {
            return false;
        }

        return true;
    }

    public static long CastStrToLong(String valueCast)
    {
        long ret = 0;
        long n1k = 1000L;
        long n1m = n1k * n1k;
        long n1b = n1m * n1k;
        long n1t = n1b * n1k;
        long n1q = n1t * n1k;
        if (IsLongNumber(valueCast))
        {
            return long.Parse(valueCast);
        }

        String[] kNum = valueCast.ToLower().Split('k');
        if (kNum.Length == 2)
        {
            if (IsLongNumber(kNum[1]))
                ret = long.Parse(kNum[1]);
        }

        if (IsLongNumber(kNum[0]))
        {
            if (IsLongNumber(kNum[0]))
                ret += long.Parse(kNum[0]) * n1k;
        }
        else
        {
            String[] mNum = kNum[0].Split('m');
            if (mNum.Length == 2)
            {
                if (IsLongNumber(mNum[1]))
                    ret += long.Parse(mNum[1]) * n1k;
            }

            if (IsLongNumber(mNum[0]))
            {
                if (IsLongNumber(mNum[0]))
                    ret += long.Parse(mNum[0]) * n1m;
            }
            else
            {
                String[] bNum = mNum[0].Split('b');
                if (bNum.Length == 2)
                {
                    if (IsLongNumber(bNum[1]))
                        ret += long.Parse(bNum[1]) * n1m;
                }

                if (IsLongNumber(bNum[0]))
                {
                    if (IsLongNumber(bNum[0]))
                        ret += long.Parse(bNum[0]) * n1b;
                }
                else
                {
                    String[] tNum = bNum[0].Split('t');
                    if (tNum.Length == 2)
                    {
                        if (IsLongNumber(tNum[1]))
                            ret += long.Parse(tNum[1]) * n1b;
                    }

                    if (IsLongNumber(tNum[0]))
                    {
                        if (IsLongNumber(tNum[0]))
                            ret += long.Parse(tNum[0]) * n1t;
                    }
                    else
                    {
                        String[] qNum = tNum[0].Split('q');
                        if (qNum.Length == 2)
                        {
                            if (IsLongNumber(qNum[1]))
                                ret += long.Parse(qNum[1]) * n1t;
                        }

                        if (IsLongNumber(qNum[0]))
                        {
                            if (IsLongNumber(qNum[0]))
                                ret += long.Parse(qNum[0]) * n1q;
                        }
                    }
                }
            }
        }

        return ret;
    }

    public static string GetCp()
    {
        return SettingManager.GetCp();
    }

    public static string FormatLongToString(long valueCast, bool isSplit = true)
    {
        string ret = "";
        long n1k = 1000L;
        long n1m = n1k * n1k;
        long n1b = n1m * n1k;
        long n1t = n1b * n1k;
        long n1q = n1t * n1k;
        if (valueCast > n1q)
        {
            int n = (int) (valueCast / n1q);
            ret += n + "q";
            valueCast = (valueCast % n1q);
        }

        if (valueCast > n1t)
        {
            int n = (int) (valueCast / n1t);
            ret += n + "t";
            valueCast = valueCast % n1t;
        }

        if (valueCast > n1b)
        {
            int n = (int) (valueCast / n1b);
            ret += n + "b";
            valueCast = valueCast % n1b;
        }

        if (valueCast > n1m)
        {
            int n = (int) (valueCast / n1m);
            ret += n + "m";
            valueCast = valueCast % n1m;
        }

        if (valueCast > n1k)
        {
            int n = (int) (valueCast / n1k);
            ret += n + "k";
            valueCast = valueCast % n1k;
        }

        if (valueCast > 0)
        {
            ret += valueCast;
        }

        int splitLength = 8;
        if (isSplit)
        {
            if (ret.Length > splitLength)
            {
                ret = ret.Split('k')[0] + "k";
            }

            if (ret.Length > splitLength)
            {
                ret = ret.Split('m')[0] + "m";
            }

            if (ret.Length > splitLength)
            {
                ret = ret.Split('b')[0] + "b";
            }

            if (ret.Length > splitLength)
            {
                ret = ret.Split('t')[0] + "t";
            }

            if (ret.Length > splitLength)
            {
                ret = ret.Split('q')[0] + "q";
            }

            return ret.ToUpper();
        }

        return ret.ToUpper();
    }


    public static string GetNetworkOperatorName()
    {
#if UNITY_ANDROID
        try
        {
            AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityPlayerActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            //
            AndroidJavaObject TM = new AndroidJavaObject("com.grepgame.lib.GrepLib");
            string carrierName = TM.CallStatic<string>("getCarrierName", unityPlayerActivity);
            if (carrierName == null)
                carrierName = "";
            return carrierName;
        }
        catch (System.Exception e)
        {
            return "";
        }
#elif UNITY_IOS
        return "";
			//return GgLibIOS.getOperatorName();
#else
        return "";
#endif
    }

    public static string Md5Sum(string input)
    {
        byte[] hashBytes = MD5CryptoServiceProvider.GetMd5Bytes(input);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }

    public static void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
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
            lstInt.Add((int) l);
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

    public static void PrintArrayLong(string content, long[] list, bool isError = false)
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
            Debug.Log(content + tmp);
        }
        else
        {
            Debug.Log(content + tmp);
        }
    }

    public static void PrintList<T>(List<T> list, bool isError = false)
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
        if (isError)
        {
            Debug.LogError(tmp);
        }
        else
        {
            Debug.Log(tmp);
        }
    }

    public static List<int> NewListInt()
    {
        return new List<int>();
    }

    public static List<long> NewListLong()
    {
        return new List<long>();
    }

    public static List<T> NewList<T>()
    {
        return new List<T>();
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

    public static List<long> ArrayIntToListLong(int[] arr)
    {
        List<long> ret = new List<long>();
        //
        for (int i = 0; i < arr.Length; i++)
        {
            ret.Add(arr[i]);
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
            ret.Add((int) l);
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

    public static List<long> ConvertJsonArrayToListLong(SimpleJSON.JSONArray json)
    {
        List<long> lstBn = new List<long>();
        if (json != null)
        {
            for (int i = 0; i < json.Count; i++)
            {
                lstBn.Add(json[i].AsLong);
            }
        }
        else
        {
            Debug.Log("---- Null list json array input");
        }

        return lstBn;
    }

    public static List<int> JsonArray2ListInt(SimpleJSON.JSONArray json)
    {
        List<int> lstBn = new List<int>();
        if (json != null)
        {
            for (int i = 0; i < json.Count; i++)
            {
                lstBn.Add(json[i].AsInt);
            }
        }
        else
        {
            Debug.Log("---- Null list json array input");
        }

        return lstBn;
    }

    public static object[] ConvertStringToArrayObject(string input)
    {
        input = input.Replace("goto:event:", "");
        input = input.Trim();
        int number = int.Parse(input);
        return new object[] {number};
    }

    public static Sprite LoadSpriteFromAtlas(string nameAtlas, string nameSprite)
    {
        Sprite sprite = null;
        string path = "_SpriteAtlas/";
        path += nameAtlas;
        SpriteAtlas atlas = Resources.Load<SpriteAtlas>(path);
        sprite = atlas.GetSprite(nameSprite);
        if (sprite != null)
        {
            return sprite;
        }
        else
        {
            Debug.LogError("Can found :" + nameSprite + " in atlas: " + nameAtlas);
            return sprite;
        }
    }

    public static bool IsEvenNumber(int num)
    {
        return num % 2 == 0;
    }

    public static void RectTransformSetLeft(RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void RectTransformSetRight(RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void RectTransformSetTop(RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void RectTransformSetBottom(RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }

    public static void RectTransformSetSizeDelta(RectTransform rt, float width, float height)
    {
        rt.sizeDelta = new Vector2(width, height);
    }

    public static GuildRole GetGuildRoleFromServerPosition(int guildPosition)
    {
        if (guildPosition == 0)
        {
            return GuildRole.MEMBER;
        }
        else if (guildPosition == 1)
        {
            return GuildRole.ELDER;
        }
        else if (guildPosition == 2)
        {
            return GuildRole.CO_LEADER;
        }
        else if (guildPosition == 3)
        {
            return GuildRole.LEADER;
        }
        else
        {
            return GuildRole.NONE;
        }
    }

    public static int ToGuildServerPositionFromRole(GuildRole role)
    {
        if (role == GuildRole.ELDER)
        {
            return 1;
        }
        else if (role == GuildRole.CO_LEADER)
        {
            return 2;
        }
        else if (role == GuildRole.LEADER)
        {
            return 3;
        }
        else
        {
            return 0;
        }
    }

    public static List<object> MergeListItem(List<object> lstObjectItem)
    {
        Dictionary<int, ItemModel> dictItem = new Dictionary<int, ItemModel>();
        List<object> lstItemMerge = new List<object>();
        for (int i = 0; i < lstObjectItem.Count; i++)
        {
            ItemModel item = ((ItemModel) lstObjectItem[i]).Clone();
            int itemKey = item.ItemKey;
            if (!dictItem.ContainsKey(itemKey))
                dictItem[itemKey] = item;
            else
                dictItem[itemKey].ItemNumber += item.ItemNumber;
        }

        foreach (ItemModel item in dictItem.Values)
            lstItemMerge.Add(item);
        return lstItemMerge;
    }

    public static bool IsDevModeOn()
    {
        //Set tai popup feedback sau nay se lam
        return PlayerPrefs.GetInt("dev_mode", 0) == 1;
    }

    #region FORMAT NUMBER

    public static string FormatHourFromSec(int sec)
    {
        int hour = sec / 3600;
        int tmp = sec % 3600;
        int min = tmp / 60;
        int sec2 = tmp % 60;
        //
        int day = hour / 24;
        //
        if (day > 0)
        {
            hour = hour % 24;
            return day + " ngày " + FormatPrefixZero(hour) +
                   ":" + FormatPrefixZero(min) + ":" + FormatPrefixZero(sec2);
        }
        else
            return FormatPrefixZero(hour) + ":" + FormatPrefixZero(min) + ":" + FormatPrefixZero(sec2);
    }

    private static string FormatPrefixZero(long value)
    {
        if (value < 10) return "0" + value;
        else return value.ToString();
    }

    public static string FormatNumber(long value, bool roundUp = false)
    {
        if (value < 10000) return value.ToString(); // Không cần định dạng nếu nhỏ hơn 10000

        double formattedValue;
        string suffix;

        if (value >= 1_000_000_000_000) // Nghìn tỷ
        {
            formattedValue = roundUp
                ? Math.Ceiling(value / 1_000_000_000_000.0)
                : Math.Round(value / 1_000_000_000_000.0, 2);
            suffix = "T";
        }
        else if (value >= 1_000_000_000) // Tỷ
        {
            formattedValue = roundUp ? Math.Ceiling(value / 1_000_000_000.0) : Math.Round(value / 1_000_000_000.0, 2);
            suffix = "B";
        }
        else if (value >= 1_000_000) // Triệu
        {
            formattedValue = roundUp ? Math.Ceiling(value / 1_000_000.0) : Math.Round(value / 1_000_000.0, 2);
            suffix = "M";
        }
        else // Nghìn
        {
            formattedValue = roundUp ? Math.Ceiling(value / 1_000.0) : Math.Round(value / 1_000.0, 2);
            suffix = "K";
        }

        return formattedValue.ToString("0.##") + suffix;
    }


    public static string FormatFloat(float value, int numberDigitAfterComma = 1)
    {
        if (Math.Abs(value - (int) value) == 0) //-- value==(int)valuenumberDigitAfterComma = 0;
            numberDigitAfterComma = 0;

        // Kiểm tra numberDigitAfterComma để định dạng chuỗi
        if (numberDigitAfterComma <= 0) return ((int) value).ToString();
        else return value.ToString($"F{numberDigitAfterComma}");
    }

    public static string FormatThousandSeperator(long value)
    {
        string sign = "";
        if (value < 0) sign = "-";

        //
        long absValue = value;
        if (value < 0) absValue *= -1;

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
}