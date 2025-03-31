using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioClip metalClip;
    public LayerMask metalLayer;

    public AudioClip[] thuds;

    private void OnCollisionEnter(Collision collision)
    {
        float relvel = Mathf.Abs(Vector3.Dot(collision.relativeVelocity, collision.contacts[0].normal));
        float volume = Mathf.InverseLerp(0, 8, relvel);

        if ((metalLayer & (1 << collision.gameObject.layer)) != 0)
        {
            AudioSource.PlayClipAtPoint(metalClip, collision.contacts[0].point, volume);
        }

        AudioSource.PlayClipAtPoint(thuds[Random.Range(0, thuds.Length)], collision.contacts[0].point, volume * 1.5f);
    }
}
