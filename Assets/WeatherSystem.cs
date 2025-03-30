using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    public float strength;

    Rigidbody[] rbs;

    void Start()
    {
        rbs = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);
    }

    void FixedUpdate()
    {
        foreach (var rb in rbs)
        {
            rb.AddForce(transform.forward * strength);
        }
    }
}
