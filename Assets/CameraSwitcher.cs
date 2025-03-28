using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraSwitcher : MonoBehaviour
{
    public RenderTexture renderTexture;
    public Camera[] cameras;
    int active;

    GameObject[] gameObjects;
    AudioListener[] audioListeners;

    void Awake()
    {
        gameObjects = new GameObject[cameras.Length];
        audioListeners = new AudioListener[cameras.Length];

        for (int i = 0; i < cameras.Length; i++)
        {
            gameObjects[i] = cameras[i].gameObject;
            audioListeners[i] = cameras[i].GetComponent<AudioListener>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (Input.GetKeyDown(KeyCode.C))
        {
            //gameObjects[active].SetActive(false);
            cameras[active].targetTexture = renderTexture;
            audioListeners[active].enabled = false;

            active = (active + 1) % cameras.Length;

            //cameras[active].gameObject.SetActive(true);
            cameras[active].targetTexture = null;
            audioListeners[active].enabled = true;
        }
    }
}
