using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField]
    private GameObject platform_to_elevate = null;

    public void Elevate()
    {
        Animation anim = platform_to_elevate.GetComponent<Animation>();
        anim.Play();
        gameObject.SetActive(false);
    }
}
