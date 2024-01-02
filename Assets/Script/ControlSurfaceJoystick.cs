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
        // Ensure the controller position is not bellow the handle.
        Vector3 localTargetPosition = transform.parent.parent.InverseTransformPoint(targetPosition);
        Vector3 fixedPosition = transform.parent.parent.TransformPoint(new Vector3(localTargetPosition.x, Mathf.Max(localTargetPosition.y, 0.1f), localTargetPosition.z));

        base.SetControllerTransforms(fixedPosition, targetRotation);
        Plane.SetPitchInput(currentValue.x * 2 - 1);
        Plane.SetRollInput(-(currentValue.z * 2 - 1));
        Plane.SetYawInput(-(currentValue.z * 2 - 1));
    }

    public override void PressTrigger()
    {
        WeaponManager weaponManager = Plane.GetComponent<WeaponManager>();
        if (!weaponManager)
            return;

        weaponManager.BeginShoot();
    }

    public override void ReleaseTrigger()
    {
        WeaponManager weaponManager = Plane.GetComponent<WeaponManager>();
        if (!weaponManager)
            return;

        weaponManager.EndShoot();
    }
}