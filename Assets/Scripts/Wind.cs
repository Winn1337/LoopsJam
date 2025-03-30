using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using Random = UnityEngine.Random;

public class Wind : MonoBehaviour
{
    public Camera mainCamera;

    public Vector2[] angles;
    public float minRotFreq;
    public float maxRotFreq;
    public float minTime;
    public float maxTime;
    public float minStrength;
    public float maxStrength;

    int i;
    float time;
    float timer;
    float rotFreq;
    public float strength;
    public float audioVolume;

    Rigidbody[] rbs;
    WindZone zone;
    AudioSource audio;

    void Start()
    {
        rbs = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
        zone = GetComponentInChildren<WindZone>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 rot;

        timer += Time.deltaTime;
        if (timer > time)
        {
            timer = 0;
            i = Random.Range(0, angles.Length);
            rotFreq = Random.Range(minRotFreq, maxRotFreq);
            time = Random.Range(minTime, maxTime);
            strength = Random.Range(minStrength, maxStrength);
            StopAllCoroutines();
            StartCoroutine(LerpAudio(Mathf.InverseLerp(minStrength, maxStrength, strength) * audioVolume));
            zone.windMain = strength;

            rot = transform.eulerAngles;

            if (Random.Range(0, 2) == 1)
                rot.y = 90;
            else
                rot.y = -90;

            transform.eulerAngles = rot;
        }

        rot = transform.eulerAngles;
        rot.x = Mathf.Lerp(angles[i].x, angles[i].y, Mathf.Sin(rotFreq * Time.time) * 0.5f + 0.5f);
        transform.eulerAngles = rot;

        
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);
        //zone.transform.position = new Vector3(
        //    mainCamera.transform.position.x - transform.forward.x * 10f,
        //    mainCamera.transform.position.y - transform.forward.y * 6.5f,
        //    0);

        foreach (var rb in rbs)
        {
            rb.AddForce(transform.forward * strength);
        }
    }

    IEnumerator LerpAudio(float to)
    {
        float from = audio.volume;
        float t = 0;

        while (t < 1)
        {
            yield return null;
            t += Time.deltaTime;
            audio.volume = Mathf.Lerp(from, to, t);
        }
    }
}
