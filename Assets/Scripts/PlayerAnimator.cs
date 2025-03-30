using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Transform leftLeg;
    public Transform rightLeg;

    public Transform leftModel;
    public Transform rightModel;

    void Update()
    {
        Vector3 local = leftModel.localEulerAngles;
        leftModel.localEulerAngles = new Vector3(leftLeg.localEulerAngles.z + 180, 180, 0);

        local = rightModel.localEulerAngles;
        rightModel.localEulerAngles = new Vector3(rightLeg.localEulerAngles.z + 180, 180, 0);
    }
}
