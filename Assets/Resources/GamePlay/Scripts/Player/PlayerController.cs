using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player _player;
    private Camera _mainCamera;
    private Transform _tfSpawnBullet;
    private CharacterModel _currCharacter;
    private GameContext _gameContext;
    private CircleCollider2D _collider;

    private void Awake()
    {
        _gameContext = GameContext.Instance;
    }

    void Start()
    {
        _mainCamera = Camera.main;
        _currCharacter =
            PlayerInventory.Instance.ItemCharacterManager.GetItemByKey(_gameContext.UserModel.characterUsingId);
        //
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
            if (_collider.OverlapPoint(mousePosition)) transform.position = ClampToScreenBounds(mousePosition);
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;
            if (_collider.OverlapPoint(touchPos)) transform.position = ClampToScreenBounds(touchPosition);
        }
#endif
    }

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
}