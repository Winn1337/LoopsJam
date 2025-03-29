using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform toFollow;
    public Vector3 offset;
    public bool updateOffsetOnStart;
    public float strength;

    private void Start()
    {
        if (updateOffsetOnStart)
            offset = transform.position - toFollow.position;
    }
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, toFollow.position + offset, strength * Time.deltaTime);
    }
}
