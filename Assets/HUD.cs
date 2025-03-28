using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public Rigidbody rb;
    public TextMeshProUGUI angVelTM;

    void Update()
    {
        angVelTM.text = Mathf.Abs(rb.angularVelocity.z).ToString("00.00");
    }
}
