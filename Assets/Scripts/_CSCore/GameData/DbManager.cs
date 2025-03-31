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

    private List<ItemConsumable> _lstItemConsumable = new List<ItemConsumable>();
    private List<ItemAvatar> _lstItemAvatar = new List<ItemAvatar>();
    private List<ItemBullet> _lstItemBullet = new List<ItemBullet>();
    private List<ItemDrop> _lstItemDrop = new List<ItemDrop>();
    private Dictionary<int, MapModel> _dictMapModel = new Dictionary<int, MapModel>();
    private List<CharacterModel> _lstCharacterModel = new List<CharacterModel>();
    
    #endregion

    #region List DB name

    private const string ItemConsum = "ItemConsum";
    private const string ItemAvatar = "ItemAvatar";
    private const string ItemBullet = "ItemBullet";
    private const string ItemDrop = "ItemDrop";
    private const string MapModel = "MapModel";
    private const string CharacterModel = "CharacterModel";
    
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
        lstAction.Add(new LoadingInitDataAction(ItemConsum, delegate
        {
            string dbFile = "db_item_consumable";
            string json = ResourceHelper.LoadDbTextContent(dbFile);
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("------ Error loading db file: " + dbFile);
                return;
            }

            dicFileDb.Add(ItemConsum, json);
        }));
        
        lstAction.Add(new LoadingInitDataAction(ItemAvatar, delegate
        {
            string dbFile = "db_avatar";
            string json = ResourceHelper.LoadDbTextContent(dbFile);
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("------ Error loading db file: " + dbFile);
                return;
            }

            dicFileDb.Add(ItemAvatar, json);
        }));
        
        lstAction.Add(new LoadingInitDataAction(ItemBullet, delegate
        {
            string dbFile = "db_bullet";
            string json = ResourceHelper.LoadDbTextContent(dbFile);
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("------ Error loading db file: " + dbFile);
                return;
            }

            dicFileDb.Add(ItemBullet, json);
        }));
        
        lstAction.Add(new LoadingInitDataAction(ItemDrop, delegate
        {
            string dbFile = "db_item_drop";
            string json = ResourceHelper.LoadDbTextContent(dbFile);
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("------ Error loading db file: " + dbFile);
                return;
            }

            dicFileDb.Add(ItemDrop, json);
        }));
        
        lstAction.Add(new LoadingInitDataAction(MapModel, delegate
        {
            string dbFile = "db_map";
            string json = ResourceHelper.LoadDbTextContent(dbFile);
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("------ Error loading db file: " + dbFile);
                return;
            }

            dicFileDb.Add(MapModel, json);
        }));
        
        lstAction.Add(new LoadingInitDataAction(CharacterModel, delegate
        {
            string dbFile = "db_character";
            string json = ResourceHelper.LoadDbTextContent(dbFile);
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("------ Error loading db file: " + dbFile);
                return;
            }

            dicFileDb.Add(CharacterModel, json);
        }));

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
        lstAction.Add(new LoadingInitDataAction(ItemConsum,
            delegate { this.LoadDbItemConsumable((string)dicFileDb[ItemConsum]); }));
        
        lstAction.Add(new LoadingInitDataAction(ItemAvatar,
            delegate { this.LoadDbItemAvatar((string)dicFileDb[ItemAvatar]); }));
        
        lstAction.Add(new LoadingInitDataAction(ItemBullet,
            delegate { this.LoadDbItemBullet((string)dicFileDb[ItemBullet]); }));
        
        lstAction.Add(new LoadingInitDataAction(ItemDrop,
            delegate { this.LoadDbItemDrop((string)dicFileDb[ItemDrop]); }));
        
        lstAction.Add(new LoadingInitDataAction(MapModel,
            delegate { this.LoadDbMapModel((string)dicFileDb[MapModel]); }));
        
        lstAction.Add(new LoadingInitDataAction(CharacterModel,
            delegate { this.LoadDbCharacterModel((string)dicFileDb[CharacterModel]); }));
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

    #region Item Consumable

    private void LoadDbItemConsumable(string json)
    {
        
    }

    #endregion

    #region Item Avatar

    private void LoadDbItemAvatar(string json)
    {
        
    }

    #endregion

    #region Item Bullet

    private void LoadDbItemBullet(string json)
    {
        
    }

    #endregion

    #region Item Drop

    private void LoadDbItemDrop(string json)
    {
        
    }    

    #endregion
    
    #region Map Model

    private void LoadDbMapModel(string json)
    {
        
    }    

    #endregion
    
    #region Character Model

    private void LoadDbCharacterModel(string json)
    {
        
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