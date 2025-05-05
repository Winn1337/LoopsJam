using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Audio;

[Serializable]
public struct SliderSetting
{
    public Slider slider;
    public string name;
    public float min;
    public float max;
    public float defaultValue;
    public UnityEvent<float> onValueChanged;
}

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject playerPrefab;

    [Header("Pause")]
    public GameObject menuCanvas;
    public GameObject cameraCanvas;

    [Header("Audio")]
    public AudioMixer audioMixer;
    public AnimationCurve audioCurve;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public Slider difficultySlider;

    GameObject player;
    PlayerMovement playerMovement;
    float timer;

    public SliderSetting[] sliders;

    void Start()
    {
        Load();

        timer = PlayerPrefs.GetFloat("timer", 0);

        foreach(var slider in sliders)
        {
            slider.slider.minValue = slider.min;
            slider.slider.maxValue = slider.max;
            slider.slider.value = PlayerPrefs.GetFloat(slider.name, slider.defaultValue);
            slider.slider.onValueChanged.AddListener((value) =>
            {
                PlayerPrefs.SetFloat(slider.name, value);
                slider.onValueChanged.Invoke(value);
            });
            slider.slider.onValueChanged.Invoke(slider.slider.value);
        }
    }

    void OnDestroy()
    {
        foreach (var slider in sliders)
            slider.slider.onValueChanged.RemoveAllListeners();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.R))
            Load();

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            Save();

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            TogglePause();

        timer += Time.deltaTime;

        TimeSpan t = TimeSpan.FromSeconds(timer);
        timerText.text = "Time wasted: " + t.ToString("hh") + '.' + t.ToString("mm") + '.' + t.ToString("ss");
    }

    void SpawnPlayer(Vector3 pos, Quaternion rot)
    {
        if (player)
            Destroy(player);

        player = Instantiate(playerPrefab, pos, rot);
        playerMovement = player.GetComponent<PlayerMovement>();

        SetDifficulty(PlayerPrefs.GetFloat("difficulty", 0.5f));
    }

    public void SetDifficulty(float value)
    {
        if (player)
            playerMovement.SetDifficulty(value);
    }

    public void SetEffectsVolume(float value)
    {
        audioMixer.SetFloat("effectsVol", Mathf.Lerp(-80, 20, audioCurve.Evaluate(value)));
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("musicVol", Mathf.Lerp(-80, 20, audioCurve.Evaluate(value)));
    }

    public void GetSave(out Vector3 pos, out Vector3 rot)
    {
        if (PlayerPrefsUtil.LoadVector("bodypos", out pos) && PlayerPrefsUtil.LoadVector("bodyrot", out rot))
            return;

        pos = transform.position;
        rot = transform.eulerAngles;
    }

    public void Load()
    {
        GetSave(out Vector3 pos, out Vector3 rot);
        SpawnPlayer(pos, Quaternion.Euler(rot));
    }

    public void Save()
    {
        if (!player)
            return;

        Save(player.transform.position, player.transform.eulerAngles);
    }

    public void Save(Vector3 pos, Vector3 rot)
    {
        PlayerPrefsUtil.SaveVector("bodypos", pos);
        PlayerPrefsUtil.SaveVector("bodyrot", rot);
    }

    public void Clear()
    {
        PlayerPrefs.DeleteAll();
        timer = 0;

        foreach(var slider in sliders)
        {
            slider.slider.value = slider.defaultValue;
            slider.slider.onValueChanged.Invoke(slider.slider.value);
        }

        Load();
    }

    public void TogglePause()
    {
        bool paused = menuCanvas.activeSelf;

        if (paused)
        {
            Time.timeScale = 1;
            menuCanvas.SetActive(false);
            //cameraCanvas.SetActive(true);
            return;
        }

        Time.timeScale = 0;
        menuCanvas.SetActive(true);
        //cameraCanvas.SetActive(false);
    }

    public void Quit()
    {
#if (UNITY_EDITOR)
        UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
        Application.Quit();
#else
        OnApplicationQuit();
#endif
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("timer", timer);
        PlayerPrefs.Save();
    }
}
