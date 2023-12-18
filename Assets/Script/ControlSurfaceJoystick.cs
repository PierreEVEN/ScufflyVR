using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSurfaceJoystick : JoystickBase
{
    public bool unlocked = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void SetControllerTransforms(Vector3 targetPosition, Quaternion targetRotation)
    {
        base.SetControllerTransforms(targetPosition, targetRotation);
        Plane.SetPitchInput(currentValue.x * 2 - 1);
        Plane.SetRollInput(-(currentValue.z * 2 - 1));
    }

    public override void PressTrigger()
    {
        Plane.Shoot(null);
    }
}