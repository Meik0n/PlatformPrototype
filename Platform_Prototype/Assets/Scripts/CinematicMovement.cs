using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicMovement : MonoBehaviour
{
    private GameObject _camera = null;
    [SerializeField]
    private Transform _waypoint_end = null;
    [SerializeField]
    private Transform _waypoint_start = null;
    [SerializeField]
    private float _movement_speed = 10.0f;
    [SerializeField]
    private float _distance_to_stop = 0.1f;
    [SerializeField]
    private float _time_stopped = 2.0f;
    private GameObject _player = null;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private IEnumerator Cinematic_Coroutine()
    {
        _player.SetActive(false);
        _camera = GetComponentInChildren<Camera>().gameObject;
        _camera.transform.position = _waypoint_start.position;

        while (Vector3.Distance(_camera.transform.position, _waypoint_end.transform.position) > _distance_to_stop)
        {
            _camera.transform.position = Vector3.MoveTowards(_camera.transform.position, _waypoint_end.transform.position, _movement_speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(_time_stopped);
        _player.SetActive(true);
        gameObject.SetActive(false);
        yield break;
    }

    public void LaunchCinematic()
    {
        StartCoroutine(Cinematic_Coroutine());
    }
}
