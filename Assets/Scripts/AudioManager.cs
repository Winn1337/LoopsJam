using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixerGroup sfxMixerGroup;

    ObjectPool<AudioSource> audioSourcePool;

    void Awake()
    {
        audioSourcePool = new ObjectPool<AudioSource>(CreateAudioSource, OnGetAudioSource, OnReleaseAudioSource, OnDestroyAudioSource);
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
        audioSource.transform.position = position;
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.spatialBlend = 1f;
        audioSource.outputAudioMixerGroup = sfxMixerGroup;
        audioSource.Play();
        StartCoroutine(ReleaseAudioSource(audioSource));
    }

    public static void PlaySFX(AudioClip audioClip, Vector3 position, float volume)
    {
        instance.PlaySoundEffect(audioClip, position, volume);
    }

    IEnumerator ReleaseAudioSource(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSourcePool.Release(audioSource);
    }
}
