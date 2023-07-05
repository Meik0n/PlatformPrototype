using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{

    [SerializeField]
    private int _health = 1;
    //public int Health => _health;

    [SerializeField]
    private GameObject _death_effect;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.Remove_Life();
        }
    }
    protected void Die()
    {
        Instantiate(_death_effect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    public void Loose_Life()
    {
        --_health;
        if (_health <= 0)
        {
            Die();
        }
    }
}
