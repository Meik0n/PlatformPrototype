using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Bullets : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            GameObject bullet = PoolManager.Instance.Request_Bullet();
            bullet.transform.position = gameObject.transform.position;
            yield return new WaitForSeconds(1f);
        }

    }
}
