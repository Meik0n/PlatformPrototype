using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_01 : Enemy
{
    private enum States
    {
        regular, growing, chasing
    }
    private States _current_state = States.regular;

    private bool _facing_right = false;
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _chase_speed = 7.0f;
    [SerializeField]
    private float _distance_to_follow = 30f;
    [SerializeField]
    private float _distance_to_turn = 3f;
    [SerializeField]
    private float _distance_to_attack = 5f;
    [SerializeField]
    private float _height_to_avoid_player = 7f;
    [SerializeField]
    private LayerMask _obstacles;
    [SerializeField]
    private LayerMask _player;
    [SerializeField]
    private Transform _detector = null;
    private Transform _target = null;

    [Header("Scale attack")]

    [SerializeField]
    private float _scale_factor_on_attack = 1f;
    private Vector3 _initial_scale;
    [SerializeField]
    private float _attack_scale_time = 1f;
    [SerializeField]
    private AnimationCurve _grow_anim = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));


    void Start()
    {
        _initial_scale = transform.localScale;
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        StartCoroutine(Regular());
    }

    private IEnumerator Regular()
    {
        _current_state = States.regular;

        while (_current_state == States.regular)
        {
            bool ground_detector = Physics.Raycast(_detector.position, Vector3.down, _distance_to_turn, _obstacles);

            if (!ground_detector)
            {
                Flip();
            }

            if (_facing_right)
            {
                if (Physics.Raycast(_detector.position, Vector3.right, _distance_to_turn, _obstacles))
                {
                    Flip();
                }
                if (Physics.Raycast(_detector.position, Vector3.right, _distance_to_follow, _player))
                {
                    StartCoroutine(Chase());
                    yield break;
                }
            }
            else if (!_facing_right)
            {
                if (Physics.Raycast(_detector.position, Vector3.left, _distance_to_turn, _obstacles))
                {
                    Flip();
                }
                if (Physics.Raycast(_detector.position, Vector3.left, _distance_to_follow, _player))
                {
                    StartCoroutine(Chase());
                    yield break;
                }
            }

            gameObject.transform.position = transform.position + (_facing_right ? Vector3.right : Vector3.left) * _speed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Chase()
    {
        _current_state = States.chasing;

        while (_current_state == States.chasing)
        {
            if (_target.position.x > transform.position.x)
            {
                if (!_facing_right)
                {
                    Flip();
                }
            }
            else if (_target.position.x < transform.position.x)
            {
                if (_facing_right)
                {
                    Flip();
                }
            }
            if (_target.position.y > transform.position.y + _height_to_avoid_player || _target.position.y < transform.position.y - _height_to_avoid_player)
            {
                StartCoroutine(Regular());
                yield break;
            }

            if (_facing_right)
            {
                if (Physics.Raycast(_detector.position, Vector3.right, _distance_to_turn, _obstacles))
                {
                    Flip();
                    StartCoroutine(Regular());
                    yield break;
                }
            }
            else if (!_facing_right)
            {
                if (Physics.Raycast(_detector.position, Vector2.left, _distance_to_turn, _obstacles))
                {
                    Flip();
                    StartCoroutine(Regular());
                    yield break;
                }
            }

            transform.Translate(Vector3.up * _chase_speed * Time.deltaTime);
            bool ground_detector = Physics.Raycast(_detector.position, Vector2.down, _distance_to_turn, _obstacles);

            if ((Vector3.Distance(transform.position, _target.position) < _distance_to_attack) && ground_detector)
            {
                StartCoroutine(Grow());
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator Grow()
    {
        _current_state = States.growing;
        var max_scale = _initial_scale;
        max_scale.y = _initial_scale.y * _scale_factor_on_attack;

        var accum_time = 0.0f;
        while (accum_time <= 1)
        {
            accum_time += Time.deltaTime / _attack_scale_time;

            transform.localScale = Vector3.Lerp(_initial_scale, max_scale, _grow_anim.Evaluate(accum_time));

            yield return null;
        }
        transform.localScale = _initial_scale;
        StartCoroutine(Chase());
        yield return null;
    }

    private void Flip()
    {
        transform.Rotate(180f, 0f, 0f);
        _facing_right = !_facing_right;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_detector.position, Vector3.down * _distance_to_turn);

        if (_facing_right)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(_detector.position, Vector3.right * _distance_to_turn);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(new Vector3(_detector.position.x, _detector.position.y + 0.5f, 0), Vector3.right * _distance_to_follow);
        }
        else if (!_facing_right)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(_detector.position, Vector3.left * _distance_to_turn);


            Gizmos.color = Color.red;
            Gizmos.DrawRay(new Vector3(_detector.position.x, _detector.position.y + 0.5f, 0), Vector3.left * _distance_to_follow);
        }
    }
}
