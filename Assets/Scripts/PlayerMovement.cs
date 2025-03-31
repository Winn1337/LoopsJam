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
    //public float helpRadius;
    //public float helpForce;
    public float easyHandRadius;
    public float hardHandRadius;

    public Vector3 armAnchor;
    public Vector3 armAxis;

    public Slider difficultySlider;

    Transform lastBar;

    public Transform[] handIndices;
    public Transform[] thumbs;

    float[] originalHandX;
    float[] originalThumbX;

    void Awake()
    {
        foreach(var joint in legJoints)
            joint.useMotor = true;

        difficultySlider.value = PlayerPrefs.GetFloat("difficulty", 0.5f);
        SetDifficulty(difficultySlider.value);

        originalHandX = new float[handIndices.Length];
        originalThumbX = new float[thumbs.Length];

        for (int i = 0; i < handIndices.Length; i++)
            originalHandX[i] = handIndices[i].localEulerAngles.x;

        for (int i = 0; i < thumbs.Length; i++)
            originalThumbX[i] = thumbs[i].localEulerAngles.x;

        //Remember to uncomment this and ApplicationQuit
        Save();
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
            OpenHand();
            Destroy(armJoint);
        }

        if (Input.GetKeyDown(KeyCode.Space) || transform.position.y < 0.5f)
            load = true;

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            Save();
    }

    bool load;

    private void FixedUpdate()
    {
        if (load)
        {
            Load();
            load = false;
        }

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
            else
            {
                lastBar = null;
                OpenHand();

                //bars = Physics.OverlapSphere(handPos.position, helpRadius, barMask);

                //if (bars.Length > 0)
                //{
                //    if (bars[0].transform == lastBar)
                //        return;

                //    Vector3 relPos = bars[0].transform.position - handPos.position;

                //    upperBody.GetComponent<Rigidbody>().AddForceAtPosition(relPos * helpForce, handPos.position);
                //}
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
        PlayerPrefs.SetFloat("difficulty", val);
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

    void Save()
    {
        SaveVector("bodypos", upperBody.position);
        SaveVector("bodyrot", upperBody.eulerAngles);
    }

    private void Load()
    {
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }

        if (armJoint)
            Destroy(armJoint);

        upperBody.position = LoadVector("bodypos");
        upperBody.eulerAngles = LoadVector("bodyrot");
    }

    private void OnApplicationQuit()
    {
        //Uncomment this and awake
        Save();
        PlayerPrefs.Save();
    }
}
