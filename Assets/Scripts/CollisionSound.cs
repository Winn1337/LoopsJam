using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioClip metalClip;
    public LayerMask metalLayer;

    public AudioClip[] thuds;

    private void OnCollisionEnter(Collision collision)
    {
        if ((metalLayer & (1 << collision.gameObject.layer)) != 0)
        {
            AudioSource.PlayClipAtPoint(metalClip, collision.contacts[0].point);
        }

        AudioSource.PlayClipAtPoint(thuds[Random.Range(0, thuds.Length)], collision.contacts[0].point);
    }
}
