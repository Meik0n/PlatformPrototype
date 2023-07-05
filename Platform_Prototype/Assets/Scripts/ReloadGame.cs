using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ReloadGame : MonoBehaviour
{
    public void Restart_Game()
    {
        string level = PlayerPrefs.GetString("Level", "Level1");
        SceneManager.LoadScene(level);
    }

    public void Go_To_Level_1()
    {
        SceneManager.LoadScene("Level1");
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.Save();
    }

    public void Go_To_Level_2()
    {
        SceneManager.LoadScene("Level2");
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.Save();
    }

    public void Go_To_Level_3()
    {
        SceneManager.LoadScene("Level3");
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.Save();
    }

    public void Exit_Game()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
}
