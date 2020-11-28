using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private int bulletCount;
    [SerializeField]
    private Bullet bulletPrefab;

    private List<Bullet> bulletPool = new List<Bullet>();
    public static BulletManager instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        for (int i = 0; i < bulletCount; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab);
            bulletPool.Add(bullet);
        }
    }
    public static void Shoot(Vector2 position, Vector2 direction, float speed, float distance)
    {
        if (instance.bulletPool.Count > 0)
        {
            instance.bulletPool[0].Shoot(position, direction, speed, distance);
            instance.bulletPool.Remove(instance.bulletPool[0]);
        }
        else
        {
            Bullet bullet = Instantiate(instance.bulletPrefab);
            bullet.Shoot(position, direction, speed, distance);
        }
    }
    public static void Reload(Bullet bullet)
    {
        instance.bulletPool.Add(bullet);
    }
    
}
