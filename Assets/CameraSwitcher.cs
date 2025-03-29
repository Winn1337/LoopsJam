using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cams;
    AudioListener[] audioListeners;

    public bool toggle;
    public RenderTexture renderTexture;
    int active;

#if UNITY_EDITOR
    void OnValidate()
    {
        if (cams == null || cams.Length == 0)
            return;

        if (toggle)
        {
            DoToggle();
            toggle = false;
            return;
        }

        audioListeners = new AudioListener[cams.Length];
        for (int i = 0; i < cams.Length; i++)
        {
            if (!cams[i])
                continue;

            audioListeners[i] = cams[i].GetComponent<AudioListener>();
        }
    }
#endif

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (Input.GetKeyDown(KeyCode.C))
            DoToggle();
    }

    void DoToggle()
    {
        cams[active].targetTexture = renderTexture;
        audioListeners[active].enabled = false;

        active = (active + 1) % cams.Length;

        cams[active].targetTexture = null;
        audioListeners[active].enabled = true;
    }
}
