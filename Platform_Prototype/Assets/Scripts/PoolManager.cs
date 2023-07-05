using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance;
    [SerializeField]
    private GameObject _bullet_prefab;
    [SerializeField]
    private GameObject _bullet_container;
    [SerializeField]
    private List<GameObject> _bullet_pool;
    [SerializeField]
    private int _bullets;
    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("PoolManager is null");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _bullet_pool = Generate_Bullets(_bullets);
    }

    private List<GameObject> Generate_Bullets(int num_of_bullets)
    {
        for (int i = 0; i < num_of_bullets; ++i)
        {
            GameObject bullet = Instantiate(_bullet_prefab);
            bullet.transform.parent = _bullet_container.transform;
            bullet.SetActive(false);
            _bullet_pool.Add(bullet);
        }

        return _bullet_pool;
    }

    public GameObject Request_Bullet()
    {
        foreach (var bullet in _bullet_pool)
        {
            if (bullet.activeInHierarchy == false)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        GameObject new_bullet = Instantiate(_bullet_prefab);
        new_bullet.transform.parent = _bullet_container.transform;
        _bullet_pool.Add(new_bullet);

        return new_bullet;
    }
}
