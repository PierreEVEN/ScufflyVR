using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustJoystick : JoystickBase
{
    public override void SetControllerTransforms(Vector3 targetPosition, Quaternion targetRotation)
    {
        if (Plane.ThrottleNotch)
        {
            base.SetControllerTransforms(targetPosition, targetRotation);
            Plane.SetThrustInput(currentValue.x);
        }
        else
            transform.parent.SetLocalPositionAndRotation(transform.parent.localPosition, Quaternion.Euler(-5, 0, 0));
    }

    public override void PressTrigger()
    {
        Plane.ThrottleNotch = !Plane.ThrottleNotch;
    }
}
