using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public ParticleSystem particles;

    public void Save()
    {
        GameManager gameManager = FindAnyObjectByType<GameManager>();

        gameManager.GetSave(out Vector3 savedPos, out _);

        Vector3 pos = transform.position - Vector3.right * 1.1f;

        if (savedPos == pos) return;

        gameManager.Save(pos, Vector3.forward * -90f);
        particles.Play();
        
        AudioManager.PlaySFX(AudioManager.instance.Clips.checkpoint, transform.position, 1);
    }
}
