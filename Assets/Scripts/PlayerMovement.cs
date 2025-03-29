using System.Net;
using Unity.VisualScripting;
using UnityEngine;

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
    public float helpRadius;
    public float helpForce;

    public Vector3 armAnchor;
    public Vector3 armAxis;

    Transform lastBar;

    void Awake()
    {
        foreach(var joint in legJoints)
            joint.useMotor = true;
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
            Destroy(armJoint);

        if (Input.GetKeyDown(KeyCode.A))
            Load();

        if (Input.GetKeyDown(KeyCode.S))
            Save();
    }

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
            }
            else
            {
                lastBar = null;

                bars = Physics.OverlapSphere(handPos.position, helpRadius, barMask);

                if (bars.Length > 0)
                {
                    if (bars[0].transform == lastBar)
                        return;

                    Vector3 relPos = bars[0].transform.position - handPos.position;

                    upperBody.GetComponent<Rigidbody>().AddForceAtPosition(relPos * helpForce, handPos.position);
                }
            }
        }
        else
        {
            upperBody.eulerAngles = new Vector3(0, 0, upperBody.eulerAngles.z);
        }
    }

    void SaveVector(string name, Vector3 vector)
    {
        PlayerPrefs.SetFloat(name + "x", vector.x);
        PlayerPrefs.SetFloat(name + "y", vector.y);
        PlayerPrefs.SetFloat(name + "z", vector.z);
    }

    Vector3 LoadVector(string name)
    {
        return new Vector3(
            PlayerPrefs.GetFloat(name + "x"),
            PlayerPrefs.GetFloat(name + "y"),
            PlayerPrefs.GetFloat(name + "z")
            );
    }

    void Save()
    {
        SaveVector("bodypos", upperBody.position);
        SaveVector("bodyrot", upperBody.eulerAngles);
    }

    private void Load()
    {
        if (armJoint)
            Destroy(armJoint);

        upperBody.position = LoadVector("bodypos");
        upperBody.eulerAngles = LoadVector("bodyrot");

        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void OnApplicationQuit()
    {

    }
}
