using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Transform upperBody;
    public HingeJoint armJoint;
    public float upVel = 250f;
    public float upForce = 250f;
    public float downVel = -100f;
    public float downForce = -100f;

    public HingeJoint[] legJoints;
    public Transform handPos;
    public float handRadius;
    public LayerMask barMask;
    public float easyHandRadius;
    public float hardHandRadius;

    public Vector3 armAnchor;
    public Vector3 armAxis;

    public AudioClip[] jumpSounds;

    Transform lastBar;

    public Transform[] handIndices;
    public Transform[] thumbs;

    float[] originalHandX;
    float[] originalThumbX;

    void Awake()
    {
        foreach(var joint in legJoints)
            joint.useMotor = true;

        originalHandX = new float[handIndices.Length];
        originalThumbX = new float[thumbs.Length];

        for (int i = 0; i < handIndices.Length; i++)
            originalHandX[i] = handIndices[i].localEulerAngles.x;

        for (int i = 0; i < thumbs.Length; i++)
            originalThumbX[i] = thumbs[i].localEulerAngles.x;

        FindAnyObjectByType<CameraFollow>().toFollow = handPos;
    }

    void Update()
    {
        bool up = Input.GetKey(KeyCode.Z);

        foreach (var joint in legJoints)
        {
            JointMotor motor = joint.motor;
            motor.targetVelocity = up ? upVel : downVel;
            motor.force = up ? upForce : downForce;
            joint.motor = motor;
        }

        if (Input.GetKeyDown(KeyCode.X) && armJoint)
        {
            AudioManager.PlaySFX(jumpSounds[Random.Range(0, jumpSounds.Length)], transform.position, 1);
            OpenHand();
            Destroy(armJoint);
        }
    }

    bool load;

    private void FixedUpdate()
    {
        if (!armJoint)
        {
            Collider[] bars = Physics.OverlapSphere(handPos.position, handRadius, barMask);

            if (bars.Length > 0)
            {
                if (bars[0].transform == lastBar)
                    return;

                Vector3 relPos = bars[0].transform.position - handPos.position;
                upperBody.position += relPos;
                upperBody.eulerAngles = new Vector3(0, 0, upperBody.eulerAngles.z);

                armJoint = upperBody.AddComponent<HingeJoint>();
                armJoint.anchor = armAnchor;
                armJoint.axis = armAxis;

                lastBar = bars[0].transform;
                CloseHand();
            }
        }
        else
        {
            upperBody.eulerAngles = new Vector3(0, 0, upperBody.eulerAngles.z);
        }
    }

    public void SetDifficulty(float val)
    {
        handRadius = Mathf.Lerp(easyHandRadius, hardHandRadius, val);
    }

    void OpenHand()
    {
        Vector3 rot;
        for (int i = 0; i < handIndices.Length; i++)
        {
            rot = handIndices[i].localEulerAngles;
            rot.x = originalHandX[i];
            handIndices[i].localEulerAngles = rot;
        }

        for (int i = 0; i < thumbs.Length; i++)
        {
            rot = thumbs[i].localEulerAngles;
            rot.x = originalThumbX[i];
            thumbs[i].localEulerAngles = rot;
        }
    }

    void CloseHand()
    {
        Vector3 rot;
        for (int i = 0; i < handIndices.Length; i++)
        {
            rot = handIndices[i].localEulerAngles;
            rot.x = 40;
            handIndices[i].localEulerAngles = rot;
        }

        for (int i = 0; i < thumbs.Length; i++)
        {
            rot = thumbs[i].localEulerAngles;
            rot.x = 40;
            thumbs[i].localEulerAngles = rot;
        }
    }
}
