using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleManager : SingletonFree<BattleManager>
{
    #region Define
    
    [SerializeField] private int numBullet = 1;
    [SerializeField] private Transform tfSpawnPlayer;
    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private GameObjectPool poolBullet;
    [SerializeField] private GameObjectPool poolItemDrop;
    [SerializeField] private BattleUI battleUI;
    
    #endregion

    #region Properties

    private Camera _mainCamera;

    //Player
    private PlayerController _playerController;

    //Map
    private MapModel _currMap;

    //Bullet
    private float _bulletFireTimer = 0;
    private BulletFactory _bulletFactory;

    //ItemDrop
    private ItemDropFactory _itemDropFactory;
    private float _timer;
    
    //UI data
    private int _point = 0;

    #endregion

    #region Core MonoBehavior

    protected override void Awake()
    {
        base.Awake();
        //Mode Test
        DbManager.GetInstance().LoadAllDb();
        _currMap = DbManager.GetInstance().GetMapModel(1, 1);
        //End Mode Test

        _mainCamera = Camera.main;
        _bulletFactory = new BulletFactory(poolBullet);
        _itemDropFactory = new ItemDropFactory(poolItemDrop);
        // SetMapData(GameContext.Instance.CurrMapSelect);
        SetMapData(_currMap);
    }

    #endregion

    #region Map

    private void SetMapData(MapModel currMap)
    {
        _currMap = currMap;
        //
        SpawnChar();
    }

    #endregion

    #region Character

    private void SpawnChar()
    {
        GameObject goChar = Instantiate(prefabPlayer, tfSpawnPlayer, false);
        _playerController = goChar.GetComponent<PlayerController>();
        _playerController.SetActShoot(PlayerShoot);
        _playerController.SetDieCb(PlayerDie);
    }

    private void PlayerDie()
    {
        Destroy(_playerController.gameObject);
        Time.timeScale = 0;
    }

    private void PlayerShoot(Transform tfSpawnBullet, float speedFire)
    {
        SpawnBullet(tfSpawnBullet, speedFire);
    }

    private void SpawnBullet(Transform tfSpawnBullet, float speedFire)
    {
        _bulletFireTimer += Time.deltaTime;
        if (_bulletFireTimer >= (1 / speedFire))
        {
            _bulletFireTimer = 0f;
            _bulletFactory.CreateBullets(tfSpawnBullet.position, numBullet);
        }
    }
    
    #endregion

    #region Item Drop

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _currMap.SpawnInterval)
        {
            SpawnItemDrop();
            _timer = 0;
        }
    }

    private void SpawnItemDrop()
    {
        // Viewport điểm (0,1) và (1,1)
        Vector3 leftTop = _mainCamera.ViewportToWorldPoint(new Vector3(0, 1, _mainCamera.nearClipPlane + 1f));
        Vector3 rightTop = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, _mainCamera.nearClipPlane + 1f));

        // Random vị trí x trong khoảng từ leftTop.x đến rightTop.x
        float randomX = Random.Range(leftTop.x, rightTop.x);
        float y = leftTop.y;

        Dictionary<int, int> dictItemDropRate = _currMap.DictItemDropRate;
        foreach (KeyValuePair<int, int> item in dictItemDropRate)
        {
            float roll = Random.Range(0f, 100f);
            if (roll <= item.Value)
            {
                ItemDrop itemDrop = DbManager.GetInstance().GetItemDropCopy(item.Key);
                Vector3 spawnPos = new Vector3(randomX, y, 0f);
                _itemDropFactory.CreateItemDrop(spawnPos, itemDrop, OnItemDropDestroy);
            }
        }
    }

    private void OnItemDropDestroy(ItemDrop itemDrop)
    {
        //
    }
    #endregion
}