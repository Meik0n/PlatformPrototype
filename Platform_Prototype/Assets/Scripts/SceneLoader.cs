using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private string _scene_to_load = null;
    [SerializeField]
    private string _scene_to_unload = null;

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(_scene_to_load))
            {
                Scene scene_to_load = SceneManager.GetSceneByName(_scene_to_load);

                if (!scene_to_load.isLoaded)
                {
                    SceneManager.LoadSceneAsync(_scene_to_load, LoadSceneMode.Additive);
                }
            }

            if (!string.IsNullOrEmpty(_scene_to_unload))
            {
                Scene scene_to_unload = SceneManager.GetSceneByName(_scene_to_unload);


                if (scene_to_unload.isLoaded)
                {
                    SceneManager.UnloadSceneAsync(_scene_to_unload);
                    GameObject[] _initial_spawn_points = GameObject.FindGameObjectsWithTag("InitialSpawnPoint");

                    for (int i = 0; i < _initial_spawn_points.Length; ++i)
                    {
                        if (_initial_spawn_points[i].scene.buildIndex == gameObject.scene.buildIndex)
                        {
                            GameManager.Instance.Set_Current_Spawn_Point(_initial_spawn_points[i].transform);
                            break;
                        }
                    }

                    PlayerPrefs.SetString("Level", gameObject.scene.name);
                    PlayerPrefs.Save();
                }
            }
        }
    }
}
