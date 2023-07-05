using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerDestroyer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _highscore_text = null;
    private void Start()
    {
        int stored_score = PlayerPrefs.GetInt("Score", 0);

        if (GameManager.Instance != null)
        {
            int current_score = GameManager.Instance.Score;

            if (current_score > stored_score)
            {
                stored_score = current_score;
                PlayerPrefs.SetInt("Score", current_score);
                PlayerPrefs.Save();
            }

            Destroy(GameManager.Instance.gameObject);
        }

        _highscore_text.text = stored_score.ToString();
    }
}
