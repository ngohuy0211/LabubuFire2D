using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Properties

    private Player _player;
    private Camera _mainCamera;
    private Transform _tfSpawnBullet;
    private CharacterModel _currCharacter;
    private GameContext _gameContext;
    private CircleCollider2D _collider;
    private System.Action<Transform, float> _actShoot;
    private System.Action _charDie;

    #endregion

    #region Core MonoBehavior

    private void Awake()
    {
        _mainCamera = Camera.main;
        _gameContext = GameContext.Instance;
        // _currCharacter = PlayerInventory.Instance.ItemCharacterManager.GetItemByKey(_gameContext.UserModel.characterUsingId);
        
        //Test
        _currCharacter = DbManager.GetInstance().GetCharacterCopy(1);
        //Test
        
        _player = this.GetComponent<Player>();
        _player.SetDataPlayer(_currCharacter);
        _tfSpawnBullet = _player.GetSpawnBulletTf();
        //
        _collider = this.GetComponent<CircleCollider2D>();
    }
    
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            if (_collider.OverlapPoint(mousePosition))
            {
                transform.position = ClampToScreenBounds(mousePosition);
                // _actShoot?.Invoke(_tfSpawnBullet, _currCharacter.SpeedFire);
                _actShoot?.Invoke(_tfSpawnBullet, 3f);
            }
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;
            if (_collider.OverlapPoint(touchPos)) 
            {
                transform.position = ClampToScreenBounds(touchPosition);
                _actShoot?.Invoke(_tfSpawnBullet);
            }
        }
#endif
    }


    #endregion

    #region Public Method

    public void TakeDame(int dame)
    {
        _currCharacter.Hp -= dame;
        if (_currCharacter.Hp <= 0)
            _charDie?.Invoke();
    }

    public void SetDieCb(System.Action cb)
    {
        _charDie = cb;
    }

    public void SetActShoot(System.Action<Transform, float> cb)
    {
        _actShoot = cb;
    }
    
    #endregion

    #region Private Method

    private Vector3 ClampToScreenBounds(Vector3 positionChar)
    {
        //Duoi trai
        Vector3 min = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane));
        //Tren phai
        Vector3 max = _mainCamera.ViewportToWorldPoint(new Vector3(1, 1, _mainCamera.nearClipPlane));

        //Lay 1 nua size cua collider
        float halfWidth = _collider.bounds.extents.x;
        float halfHeight = _collider.bounds.extents.y;

        //Clamp de 1 nua nguoi cua player ko bi tran ra ngoai man hinh
        positionChar.x = Mathf.Clamp(positionChar.x, min.x + halfWidth, max.x - halfWidth);
        positionChar.y = Mathf.Clamp(positionChar.y, min.y + halfHeight, max.y - halfHeight);

        return positionChar;
    }


    #endregion
}