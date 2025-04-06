using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioClip metalClip;
    public LayerMask metalLayer;

    public AudioClip[] thuds;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        float relvel = Mathf.Abs(Vector3.Dot(collision.relativeVelocity, collision.contacts[0].normal));
        float volume = Mathf.InverseLerp(0, 8, relvel);

        if ((metalLayer & (1 << collision.gameObject.layer)) != 0)
        {
            audioSource.clip = metalClip;
            audioSource.volume = volume;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = thuds[Random.Range(0, thuds.Length)];
            audioSource.volume = volume * 1.5f;
            audioSource.Play();
        }
    }
}
