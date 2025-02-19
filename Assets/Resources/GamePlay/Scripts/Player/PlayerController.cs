using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform firePoint;
    private IBulletPattern bulletPattern;
    private BulletFactory bulletFactory;
    private Camera mainCamera;

    void Start()
    {
        bulletPattern = new BulletPatternStraight(); // Default pattern
        bulletFactory = FindObjectOfType<BulletFactory>();
        mainCamera = Camera.main;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);
            bulletPattern.Fire(firePoint, bulletFactory);
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0;
            transform.position = new Vector3(touchPosition.x, transform.position.y, transform.position.z);
            bulletPattern.Fire(firePoint, bulletFactory);
        }
#endif
    }
}