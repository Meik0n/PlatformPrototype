using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_01_OnTrigger : MonoBehaviour
{
    private Enemy _enemy = null;
    void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _enemy.Loose_Life();
        }
    }
}
