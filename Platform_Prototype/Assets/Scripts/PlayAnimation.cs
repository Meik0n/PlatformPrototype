using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    Animation _animation = null;

    void Start()
    {
        _animation = GetComponent<Animation>();
    }

    public void Play_Animation()
    {
        _animation.Play();
    }
}
