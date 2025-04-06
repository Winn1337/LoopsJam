using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restarter : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    static float timer;

    private void Awake()
    {
        timer = PlayerPrefs.GetFloat("timer", 0);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //    Restart();

        timer += Time.unscaledDeltaTime;

        TimeSpan t = TimeSpan.FromSeconds(timer);
        timerText.text = "Time wasted: " + t.ToString("hh") + '.' + t.ToString("mm") + '.' + t.ToString("ss");
    }

    public static void Restart()
    {
        PlayerPrefs.SetFloat("timer", timer);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("timer", timer);
        PlayerPrefs.Save();
    }
}
