using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _speed = 15f;

    private void OnEnable()
    {
        Invoke("Hide", 1f);
    }


    private void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
