using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using SimpleJSON;
using Newtonsoft.Json;
using Spine.Unity;
using UnityEditor;

public class DbManager
{
    private static DbManager instance;

    public static DbManager GetInstance()
    {
        if (instance == null)
        {
            instance = new DbManager();
        }


        return instance;
    }

    private DbManager()
    {
        HasInitDb = false;
        HasInitDbLoadFile = false;
    }

    #region Define data
    
    #endregion

    #region List DB name
    
    #endregion

    #region READ DB

    Dictionary<string, object> dicFileDb = new Dictionary<string, object>();
    private List<TrackingDbLoad> _listTracking = new List<TrackingDbLoad>();
    public bool HasInitDbLoadFile { set; get; }
    public bool HasInitDb { set; get; }

    public void LoadAllDBFile(MonoBehaviour mono, float starterSliderValue, float amountSliderValue,
        System.Action<float> progressedCb)
    {
        mono.StartCoroutine(DelayLoadAllDb(starterSliderValue, amountSliderValue, progressedCb));
    }

    private IEnumerator DelayLoadAllDb(float starterSliderValue, float amountSliderValue,
        System.Action<float> progressedCb)
    {
        yield return new WaitForEndOfFrame();
        //
        //TrackTime("");
        //
        List<LoadingInitDataAction> lstAction = new List<LoadingInitDataAction>();


        int numAction = lstAction.Count;
        if (numAction < 0)
        {
            yield break;
        }

        float sliderEach = amountSliderValue / numAction;
        float sliderValue = starterSliderValue;
        //
        progressedCb(sliderValue);
        foreach (LoadingInitDataAction loadAction in lstAction)
        {
            loadAction.Action.Invoke();
            //TrackTime(loadAction.m_Name);
            //
            sliderValue += sliderEach;
            //
            progressedCb?.Invoke(sliderValue);
            yield return new WaitForEndOfFrame();
        }

        this.HasInitDbLoadFile = true;
        //print out tracking results
        _listTracking.Sort(delegate(TrackingDbLoad t1, TrackingDbLoad t2)
        {
            if (t1.TimeLoading > t2.TimeLoading)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        });
        //
        float totalTime = 0f;
        foreach (TrackingDbLoad track in _listTracking)
        {
            Debug.LogErrorFormat("--- Time load {0}: {1}", track.Name, track.TimeLoading);
            totalTime += track.TimeLoading;
        }
    }

    public static void ClearInstance()
    {
        instance = null;
    }

    public void LoadAllDb2(MonoBehaviour mono, float starterSliderValue,
        float amountSliderValue, System.Action<float> progressedCb)
    {
        CallLoadDbWithProgressInformation(mono, starterSliderValue, amountSliderValue, progressedCb);
    }

    private void CallLoadDbWithProgressInformation(MonoBehaviour mono = null, float starterSliderValue = 0f,
        float amountSliderValue = 0f, System.Action<float> progressedCb = null)
    {
        //
        DelayLoadAllDbThread(starterSliderValue, amountSliderValue, progressedCb);
    }

    private void DelayLoadAllDbThread(float starterSliderValue, float amountSliderValue,
        System.Action<float> progressedCb)
    {
        //Debug.LogError("--- Start loading db ---");

        List<LoadingInitDataAction> lstAction = new List<LoadingInitDataAction>();
        //
        
        //
        int numAction = lstAction.Count;
        if (numAction < 0)
        {
            //  yield break;
        }

        float sliderEach = amountSliderValue / numAction;
        float sliderValue = starterSliderValue;
        //
        progressedCb(sliderValue);
        foreach (LoadingInitDataAction loadAction in lstAction)
        {
            loadAction.Action.Invoke();
            //TrackTime(loadAction.m_Name);
            //
            sliderValue += sliderEach;
            //
            progressedCb?.Invoke(sliderValue);
            //yield return new WaitForEndOfFrame();
        }

        this.HasInitDb = true;
        //
        //print out tracking results
        _listTracking.Sort(delegate(TrackingDbLoad t1, TrackingDbLoad t2)
        {
            if (t1.TimeLoading > t2.TimeLoading)
                return -1;
            else
                return 1;
        });
        //
        float totalTime = 0f;
        foreach (TrackingDbLoad track in _listTracking)
        {
            Debug.LogErrorFormat("--- Time load {0}: {1}", track.Name, track.TimeLoading);
            totalTime += track.TimeLoading;
        }
        //
        //Debug.LogError("------- TOTAL: " + totalTime);
    }

    #endregion
}

public class TrackingDbLoad
{
    public string Name { get; private set; }
    public float TimeLoading { get; private set; }

    public TrackingDbLoad(string name, float time)
    {
        this.Name = name;
        this.TimeLoading = time;
    }
}