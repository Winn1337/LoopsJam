using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public HingeJoint armJoint;
    public HingeJoint hipJoint;
    public float upVel = 250f;
    public float upForce = 250f;
    public float downVel = -100f;
    public float downForce = -100f;

    public HingeJoint[] legJoints;

    void Awake()
    {
        hipJoint.useMotor = true;
    }

    void Update()
    {
        bool up = Input.GetKey(KeyCode.Z);
        //JointMotor motor = hipJoint.motor;
        //motor.targetVelocity = up ? upVel : downVel;
        //motor.force = up ? upForce : downForce;
        //hipJoint.motor = motor;

        foreach (var joint in legJoints)
        {
            JointMotor motor = joint.motor;
            motor.targetVelocity = up ? upVel : downVel;
            motor.force = up ? upForce : downForce;
            joint.motor = motor;
        }

        if (Input.GetKey(KeyCode.X) && armJoint)
            Destroy(armJoint);
    }
}
