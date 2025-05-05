using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    public struct AudioClips
    {
        public AudioClip enterUI;
        public AudioClip exitUI;
        public AudioClip clickUI;
        public AudioClip checkpoint;
    }

    public static AudioManager instance;

    public AudioMixerGroup sfxMixerGroup;

    ObjectPool<AudioSource> audioSourcePool;
    AudioSource hurtAudioSource;
    Camera mainCamera;

    public AudioClips Clips;

    void Awake()
    {
        audioSourcePool = new ObjectPool<AudioSource>(CreateAudioSource, OnGetAudioSource, OnReleaseAudioSource, OnDestroyAudioSource);
        hurtAudioSource = CreateAudioSource();
        mainCamera = Camera.main;
        instance = this;
    }

    AudioSource CreateAudioSource()
    {
        GameObject audioSourceObject = new GameObject("AudioSource");
        AudioSource audioSource = audioSourceObject.AddComponent<AudioSource>();
        audioSourceObject.transform.SetParent(transform);
        return audioSource;
    }

    void OnGetAudioSource(AudioSource audioSource)
    {
        audioSource.gameObject.SetActive(true);
    }

    void OnReleaseAudioSource(AudioSource audioSource)
    {
        audioSource.gameObject.SetActive(false);
        audioSource.Stop();
    }

    void OnDestroyAudioSource(AudioSource audioSource)
    {
        Destroy(audioSource.gameObject);
    }

    void PlaySoundEffect(AudioClip audioClip, Vector3 position, float volume)
    {
        AudioSource audioSource = audioSourcePool.Get();
        audioSource.transform.position = position == Vector3.zero ? mainCamera.transform.position : position;
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.spatialBlend = 1f;
        audioSource.outputAudioMixerGroup = sfxMixerGroup;
        audioSource.Play();
        StartCoroutine(ReleaseAudioSource(audioSource));
    }

    public static void PlaySFX(AudioClip audioClip, Vector3 position, float volume)
    {
        if (audioClip == null)
            return;

        instance.PlaySoundEffect(audioClip, position, volume);
    }

    void PlayHurtSoundEffect(AudioClip audioClip, Vector3 position, float volume)
    {
        //if (hurtAudioSource.isPlaying)
        //    return;

        hurtAudioSource.transform.position = position;
        hurtAudioSource.clip = audioClip;
        hurtAudioSource.volume = volume;
        hurtAudioSource.spatialBlend = 1f;
        hurtAudioSource.outputAudioMixerGroup = sfxMixerGroup;
        hurtAudioSource.Play();
    }

    public static void PlayHurtSFX(AudioClip audioClip, Vector3 position, float volume)
    {
        instance.PlayHurtSoundEffect(audioClip, position, volume);
    }

    IEnumerator ReleaseAudioSource(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSourcePool.Release(audioSource);
    }
}
