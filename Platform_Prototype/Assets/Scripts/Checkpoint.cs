using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private Transform _spawnpoint = null;
    [SerializeField]
    private GameObject _object_to_disable = null;
    [SerializeField]
    private GameObject _object_to_enable = null;
    private bool _flag = false;

    private void Start()
    {
        _object_to_disable.SetActive(true);
        _object_to_enable.SetActive(false);
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!_flag && other.CompareTag("Player"))
        {
            GameManager.Instance.Set_Current_Spawn_Point(_spawnpoint);
            _object_to_disable.SetActive(false);
            _object_to_enable.SetActive(true);

            _flag = true;
        }
    }
}
