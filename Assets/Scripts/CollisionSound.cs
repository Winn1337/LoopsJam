using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioClip metalClip;
    public LayerMask metalLayer;

    public AudioClip[] thuds;
    public AudioClip[] hurt;
    public float painThreshold = 5f;

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
            AudioManager.PlaySFX(metalClip, collision.contacts[0].point, volume);
        else
            AudioManager.PlaySFX(thuds[Random.Range(0, thuds.Length)], collision.contacts[0].point, volume * 1.5f);

        if (collision.relativeVelocity.sqrMagnitude > painThreshold * painThreshold)
            AudioManager.PlayHurtSFX(hurt[Random.Range(0, hurt.Length)], transform.parent ? transform.parent.position : transform.position, 1);
    }
}
