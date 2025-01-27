using System.Collections.Generic;
using System;
using UnityEngine;

public class CacheManager : Singleton<CacheManager>
{
    private Dictionary<string, ItemCache> m_DictData; // nếu đã dùng get và set thì hãy để private
    private Dictionary<string, GameObject> mCacheGo;

    #region CACHED WITH DURATION

    protected override void Initialize()
    {
        base.Initialize();
        m_DictData = new Dictionary<string, ItemCache>();
        mCacheGo = new Dictionary<string, GameObject>();
    }


    public void SetCacheGo(String key, GameObject go)
    {
        if (!mCacheGo.ContainsKey(key)) mCacheGo[key] = go;
        else
            mCacheGo.Add(key, go);
    }

    public GameObject GetCacheGo(String key)
    {
        if (mCacheGo.ContainsKey(key)) return mCacheGo[key];
        return null;
    }


    public void AddCacheItemWithDuration(string key, object data, float cacheDuration)
    {
        ItemCache itmCache = new ItemCache()
        {
            m_ItemKey = key,
            m_ItemData = data,
            m_DurationCacheInSec = cacheDuration,
            m_CacheMoment = DateTime.Now,
        };
        m_DictData[key] = itmCache;
    }


    //Time < 0 la cache ma ko co time, sau khi tat app se reset
    public object GetCacheDurationData(string key)
    {
        if (!m_DictData.ContainsKey(key))
            return null;
        //
        ItemCache itmCache = m_DictData[key];
        float totalSecElapsed = (float)DateTime.Now.Subtract(itmCache.m_CacheMoment).TotalSeconds;
        //
        return itmCache.m_DurationCacheInSec > 0
            ? (totalSecElapsed < itmCache.m_DurationCacheInSec ? itmCache.m_ItemData : null)
            : itmCache.m_ItemData;
    }

    public void ClearCache()
    {
        m_DictData.Clear();
        mCacheGo.Clear();
    }

    public class ItemCache
    {
        public string m_ItemKey = "-1";

        public object m_ItemData = null;

        //
        public System.DateTime m_CacheMoment;
        public float m_DurationCacheInSec = 100;
    }

    #endregion

    #region CACHED WITH PLAYER PREF

    public static string LoadPrefString(string key, string defaultValue = "")
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }

    public static void SetPrefString(string key, string value)
    {
        if (value == null)
            value = "";
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }

    public static float LoadPrefFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }

    public static void SetPrefFloat(string key, float value = 0)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

    public static int LoadPrefInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public static void SetPrefInt(string key, int value = 0)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    #endregion
}


public class CacheDataKey
{
    
}