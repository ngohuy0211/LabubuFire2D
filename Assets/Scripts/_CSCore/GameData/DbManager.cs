using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
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
    private Dictionary<int, List<MapModel>> _dictMapModel = new Dictionary<int, List<MapModel>>();
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
            delegate { this.LoadDbItemConsumable((string) dicFileDb[ItemConsum]); }));

        lstAction.Add(new LoadingInitDataAction(ItemAvatar,
            delegate { this.LoadDbItemAvatar((string) dicFileDb[ItemAvatar]); }));

        lstAction.Add(new LoadingInitDataAction(ItemBullet,
            delegate { this.LoadDbItemBullet((string) dicFileDb[ItemBullet]); }));

        lstAction.Add(new LoadingInitDataAction(ItemDrop,
            delegate { this.LoadDbItemDrop((string) dicFileDb[ItemDrop]); }));

        lstAction.Add(new LoadingInitDataAction(MapModel,
            delegate { this.LoadDbMapModel((string) dicFileDb[MapModel]); }));

        lstAction.Add(new LoadingInitDataAction(CharacterModel,
            delegate { this.LoadDbCharacterModel((string) dicFileDb[CharacterModel]); }));
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

    #region TEST

    public void LoadAllDb()
    {
        LoadDbItemConsumable(ResourceHelper.LoadDbTextContent("db_item_consumable"));
        LoadDbItemBullet(ResourceHelper.LoadDbTextContent("db_bullet"));
        LoadDbItemDrop(ResourceHelper.LoadDbTextContent("db_item_drop"));
        LoadDbMapModel(ResourceHelper.LoadDbTextContent("db_map"));
        LoadDbCharacterModel(ResourceHelper.LoadDbTextContent("db_character"));
        LoadDbItemAvatar(ResourceHelper.LoadDbTextContent("db_avatar"));
    }

    #endregion
    
    #region Item Consumable

    [CanBeNull]
    public ItemConsumable GetItemConsumableCopy(int itemKey)
    {
        return (ItemConsumable) _lstItemConsumable.Find(c => c.ItemKey == itemKey).Clone();
    }

    private void LoadDbItemConsumable(string json)
    {
        _lstItemConsumable = new List<ItemConsumable>();
        JSONArray jArr = JSONArray.Parse(json).AsArray;
        for (int i = 0; i < jArr.Count; i++)
        {
            JSONClass jObj = jArr[i].AsObject;
            ItemConsumable itemConsum = new ItemConsumable();
            itemConsum.ItemKey = jObj["id"].AsInt;
            itemConsum.ItemName = jObj["name"].Value;
            itemConsum.ItemDesc = jObj["desc"].Value;
            itemConsum.Type = jObj["type"].AsInt;
            _lstItemConsumable.Add(itemConsum);
        }
    }

    #endregion

    #region Item Avatar

    [CanBeNull]
    public ItemAvatar GetItemAvatarCopy(int itemKey)
    {
        return (ItemAvatar) _lstItemAvatar.Find(c => c.ItemKey == itemKey).Clone();
    }

    public List<ItemAvatar> GetListAvatar()
    {
        return _lstItemAvatar;
    }

    private void LoadDbItemAvatar(string json)
    {
        _lstItemAvatar = new List<ItemAvatar>();
        JSONArray jArr = JSONArray.Parse(json).AsArray;
        for (int i = 0; i < jArr.Count; i++)
        {
            JSONClass jObj = jArr[i].AsObject;
            ItemAvatar itemAvatar = new ItemAvatar();
            itemAvatar.ItemKey = jObj["id"].AsInt;
            itemAvatar.ItemName = jObj["name"].Value;
            List<int> lstPrice = Utils.StringToList<int>(jObj["price"].Value);
            itemAvatar.Price = this.GetItemConsumableCopy(lstPrice[0]);
            if (itemAvatar.Price != null) itemAvatar.Price.ItemNumber = lstPrice[1];
            _lstItemAvatar.Add(itemAvatar);
        }
    }

    #endregion

    #region Item Bullet

    [CanBeNull]
    public ItemBullet GetItemBulletCopy(int itemKey)
    {
        return (ItemBullet) _lstItemBullet.Find(c => c.ItemKey == itemKey).Clone();
    }

    public List<ItemBullet> GetListBullet()
    {
        return _lstItemBullet;
    }

    private void LoadDbItemBullet(string json)
    {
        _lstItemBullet = new List<ItemBullet>();
        JSONArray jsonArr = JSONArray.Parse(json).AsArray;
        for (int i = 0; i < jsonArr.Count; i++)
        {
            JSONClass jObj = jsonArr[i].AsObject;
            ItemBullet itemBullet = new ItemBullet();
            itemBullet.ItemKey = jObj["id"].AsInt;
            itemBullet.ItemName = jObj["name"].Value;
            itemBullet.Damage = jObj["damage"].AsInt;
            List<int> lstPrice = Utils.StringToList<int>(jObj["price"].Value);
            itemBullet.Price = this.GetItemConsumableCopy(lstPrice[0]);
            if (itemBullet.Price != null) itemBullet.Price.ItemNumber = lstPrice[1];
            _lstItemBullet.Add(itemBullet);
        }
    }

    #endregion

    #region Item Drop

    [CanBeNull]
    public ItemDrop GetItemDropCopy(int itemKey)
    {
        return (ItemDrop) _lstItemDrop.Find(c => c.ItemKey == itemKey).Clone();
    }

    private void LoadDbItemDrop(string json)
    {
        _lstItemDrop = new List<ItemDrop>();
        JSONArray jsonArr = JSONArray.Parse(json).AsArray;
        for (int i = 0; i < jsonArr.Count; i++)
        {
            JSONClass jObj = jsonArr[i].AsObject;
            ItemDrop itemDrop = new ItemDrop();
            itemDrop.ItemKey = jObj["id"].AsInt;
            itemDrop.ItemName = jObj["name"].Value;
            itemDrop.Hp = jObj["hp"].AsInt;
            itemDrop.Speed = jObj["speed"].AsFloat;
            itemDrop.Type = (ItemDropType) jObj["type"].AsInt;
            itemDrop.Value = jObj["valueAdd"].AsInt;
            itemDrop.Quality = (ItemQuality) jObj["quality"].AsInt;
            itemDrop.Damage = jObj["damage"].AsInt;
            _lstItemDrop.Add(itemDrop);
        }
    }

    #endregion

    #region Map Model

    public MapModel GetMapModel(int mapId, int id)
    {
        return _dictMapModel[mapId].Find(c=>c.ID == id);
    }

    private void LoadDbMapModel(string json)
    {
        _dictMapModel = new Dictionary<int, List<MapModel>>();
        JSONArray jsonArr = JSONArray.Parse(json).AsArray;
        for (int i = 0; i < jsonArr.Count; i++)
        {
            JSONClass jObj = jsonArr[i].AsObject;
            MapModel mapModel = new MapModel();
            mapModel.ID = jObj["id"].AsInt;
            mapModel.MapID = jObj["map"].AsInt;
            mapModel.Level = jObj["level"].AsInt;
            List<int> lstItemDropRate = Utils.StringToList<int>(jObj["item_drop"].Value);
            for (int j = 0; j < lstItemDropRate.Count; j += 2)
            {
                if (mapModel.DictItemDropRate.ContainsKey(lstItemDropRate[j]))
                    mapModel.DictItemDropRate[lstItemDropRate[j]] = lstItemDropRate[j + 1];
                else
                    mapModel.DictItemDropRate.Add(lstItemDropRate[j], lstItemDropRate[j + 1]);
            }

            mapModel.RequirePoint = jObj["require_point"].AsInt;
            mapModel.WinCoin = jObj["win_coin"].AsInt;
            mapModel.Time = jObj["time"].AsInt;
            mapModel.SpawnInterval = jObj["spawn_interval"].AsFloat;
            //
            if (_dictMapModel.ContainsKey(mapModel.MapID))
                _dictMapModel[mapModel.MapID].Add(mapModel);
            else
                _dictMapModel.Add(mapModel.MapID, new List<MapModel>(){mapModel});
        }
    }

    #endregion

    #region Character Model

    [CanBeNull]
    public CharacterModel GetCharacterCopy(int itemKey)
    {
        return (CharacterModel) _lstCharacterModel.Find(c => c.ItemKey == itemKey).Clone();
    }

    public List<CharacterModel> GetListCharacter()
    {
        return _lstCharacterModel;
    }
    
    private void LoadDbCharacterModel(string json)
    {
        _lstCharacterModel = new List<CharacterModel>();
        JSONArray jsonArr = JSONArray.Parse(json).AsArray;
        for (int i = 0; i < jsonArr.Count; i++)
        {
            JSONClass jObj = jsonArr[i].AsObject;
            CharacterModel character = new CharacterModel();
            character.ItemKey = jObj["id"].AsInt;
            character.ItemName = jObj["name"].Value;
            character.Hp = jObj["hp"].AsInt;
            character.SpeedFire = jObj["speedFire"].AsInt;
            character.ItemDesc = jObj["desc"].Value;
            List<int> lstPrice = Utils.StringToList<int>(jObj["price"].Value);
            character.Price = this.GetItemConsumableCopy(lstPrice[0]);
            if (character.Price != null) character.Price.ItemNumber = lstPrice[1];
            _lstCharacterModel.Add(character);
        }
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