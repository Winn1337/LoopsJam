using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public HingeJoint armJoint;
    public float upVel = 250f;
    public float upForce = 250f;
    public float downVel = -100f;
    public float downForce = -100f;

    public HingeJoint[] legJoints;

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
    }
}
