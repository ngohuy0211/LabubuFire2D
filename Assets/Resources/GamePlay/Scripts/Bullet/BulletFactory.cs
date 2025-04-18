using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletFactory
{
    private GameObjectPool _poolMachine;

    public BulletFactory(GameObjectPool poolMachine)
    {
        _poolMachine = poolMachine;
    }

    public void CreateBullets(Vector3 spawnPos, int numBullet)
    {
        Vector2[] directions = GetBulletDirections(numBullet);
        foreach (var dir in directions)
        {
            GameObject bullet = _poolMachine.GetPooledObject(dir);
            bullet.transform.position = spawnPos;
        }
    }

    private Vector2[] GetBulletDirections(int level, float spreadPerLevel = 10f)
    {
        if (level <= 0) return new Vector2[0];

        Vector2[] directions = new Vector2[level];

        float centerAngle = 90f;
        //1 level thì có level - 1 khoảng cách
        float totalSpread = spreadPerLevel * (level - 1);
        float startAngle = centerAngle - totalSpread / 2f;

        for (int i = 0; i < level; i++)
        {
            //Góc
            float angle = startAngle + i * spreadPerLevel;
            //Giá trị radian của góc
            float rad = angle * Mathf.Deg2Rad;
            //Cos - x, Sin - y
            directions[i] = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
        }

        return directions;
    }

}
