using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance => _instance;
    private int _score = 0;
    public int Score
    {
        set
        {
            _score = value;
            _score_text_value.text = value.ToString();
        }
        get
        {
            return _score;
        }
    }

    private Transform _current_spawn_point = null;

    [SerializeField]
    private GameObject _player = null;

    [SerializeField]
    private int _max_lifes = 3;
    public int Max_Lifes => _max_lifes;
    [SerializeField]
    private Image _life_image = null;
    [SerializeField]
    private Transform _life_parent = null;
    [SerializeField]
    private TextMeshProUGUI _score_text_value = null;

    private int _current_lifes = 0;
    public int Current_Lifes => _current_lifes;

    private CinematicMovement _cinematic = null;

    [Header("PauseScreen")]
    [SerializeField]
    Slider _volume_slider = null;
    [SerializeField]
    Button _pause_button = null;
    [SerializeField]
    Sprite _pause_sprite = null;
    [SerializeField]
    Sprite _play_sprite = null;
    private GameObject _pause_canvas = null;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _current_spawn_point = GameObject.FindGameObjectWithTag("InitialSpawnPoint").transform;
        _pause_canvas = GameObject.FindGameObjectWithTag("PauseCanvas");
        _pause_canvas.SetActive(false);

        _current_lifes = _max_lifes;
        Score = 0;

        if (_life_parent.childCount == 0)
        {
            for (int i = 0; i < _max_lifes; ++i)
            {
                Instantiate(_life_image, _life_parent);
            }
        }
        else
        {
            for (int i = 0; i < _life_parent.childCount; ++i)
            {
                _life_parent.GetChild(i).gameObject.SetActive(true);
            }
        }

        if (SceneManager.GetActiveScene().name == "Level1")
        {
            _cinematic = GameObject.FindGameObjectWithTag("Cinematic").GetComponent<CinematicMovement>();
            if (_cinematic != null)
            {
                _cinematic.LaunchCinematic();
            }

        }

        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 1);
            _volume_slider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        else
        {
            _volume_slider.value = PlayerPrefs.GetFloat("MusicVolume");
        }
    }

    public void Remove_Life()
    {
        _current_lifes = Mathf.Max(_current_lifes - 1, 0);

        for (int i = 0; i < _life_parent.childCount; ++i)
        {
            _life_parent.GetChild(i).gameObject.SetActive(i < _current_lifes);
        }

        if (_current_lifes == 0)
        {
            //GameOver
            SceneManager.LoadScene("GameOverScreen");
        }
        else
        {
            Respawn_Player();
        }

    }

    public void Restore_Life()
    {
        ++_current_lifes;

        for (int i = 0; i < _current_lifes; ++i)
        {
            if (_life_parent.GetChild(i).gameObject != null)
            {
                _life_parent.GetChild(i).gameObject.SetActive(i < _current_lifes);
            }
        }
    }


    private void Respawn_Player()
    {
        CharacterController char_controller = _player.GetComponent<CharacterController>();

        if (char_controller != null)
        {
            char_controller.enabled = false;
        }

        _player.transform.position = _current_spawn_point.position;

        if (char_controller != null)
        {
            char_controller.enabled = true;
        }
    }

    public void Add_Score(int score)
    {
        Score += score;
    }

    public void Goal_Reached()
    {
        SceneManager.LoadScene("VictoryScreen");
    }

    public void Set_Current_Spawn_Point(Transform t)
    {
        _current_spawn_point = t;
    }

    public void ChangeVolume()
    {
        AudioListener.volume = _volume_slider.value;
        PlayerPrefs.SetFloat("MusicVolume", _volume_slider.value);
    }

    public void On_Pause_Click()
    {
        if (_pause_canvas.activeSelf)
        {
            _pause_button.image.sprite = _pause_sprite;
            Time.timeScale = 1;
            _pause_canvas.SetActive(false);
        }
        else if (!_pause_canvas.activeSelf)
        {
            _pause_button.image.sprite = _play_sprite;
            Time.timeScale = 0;
            _pause_canvas.SetActive(true);
        }
    }

    public void Go_To_Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
