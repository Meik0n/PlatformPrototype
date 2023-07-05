using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    [SerializeField]
    private Vector3 _rotation_speed = Vector3.zero;

    [SerializeField]
    private AudioClip _coin_audio = null;

    [SerializeField]
    private ParticleSystem _coin_particles = null;

    [SerializeField]
    private int _score = 1;

    private void Update()
    {
        transform.Rotate(_rotation_speed * Time.deltaTime);
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(_coin_audio, transform.position);
            Instantiate(_coin_particles, transform.position, Quaternion.identity);
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            GameManager.Instance.Add_Score(_score);
        }
    }
}
