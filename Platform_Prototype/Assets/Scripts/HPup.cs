using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.Current_Lifes < GameManager.Instance.Max_Lifes)
            {
                GameManager.Instance.Restore_Life();
                gameObject.SetActive(false);
            }

        }
    }
}
