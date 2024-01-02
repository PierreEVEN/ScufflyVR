using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.XR;
using Plane = UnityEngine.Plane;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class JoystickBase : PlaneComponent
{
    private Vector3 grabInitialPosition;
    private Quaternion grabInitialRotation;

    public Vector3 minAngles;
    public Vector3 maxAngles;

    private bool grab = false;
    protected Vector3 currentValue = Vector3.zero;

    public void Grab()
    {
        grab = true;
    }

    public void Release()
    {
        grab = false;
    }

    Vector3 ProjectPointOnLocalPlane(Vector3 point, Vector3 axis)
    {
        Vector3 projection = (new Plane(axis, Vector3.zero)).ClosestPointOnPlane(
            transform.parent.parent.InverseTransformPoint(point));
        return projection;
    }

    void Update()
    {
        if (grab)
            return;
    }

    public virtual void SetControllerTransforms(Vector3 targetPosition, Quaternion targetRotation)
    {
        if (!grab)
            return;

        Vector3 currentHandlePosition = transform.position;

        Vector3 localTargetJoystickRotation = transform.parent.localRotation.eulerAngles;

        // X-Axis
        Vector3 currentXProj = ProjectPointOnLocalPlane(currentHandlePosition, Vector3.right);
        Vector3 targetXProj = ProjectPointOnLocalPlane(targetPosition, Vector3.right);
        // Y-Axis
        Vector3 currentYProj = ProjectPointOnLocalPlane(currentHandlePosition, Vector3.up);
        Vector3 targetYProj = ProjectPointOnLocalPlane(targetPosition, Vector3.up);
        // Z-Axis
        Vector3 currentZProj = ProjectPointOnLocalPlane(currentHandlePosition, Vector3.forward);
        Vector3 targetZProj = ProjectPointOnLocalPlane(targetPosition, Vector3.forward);

        Debug.DrawLine(transform.parent.position, transform.parent.parent.TransformPoint(targetXProj), Color.red);
        Debug.DrawLine(transform.parent.position, transform.parent.parent.TransformPoint(targetYProj), Color.green);
        Debug.DrawLine(transform.parent.position, transform.parent.parent.TransformPoint(targetZProj), Color.blue);
        Debug.DrawLine(transform.parent.position, transform.parent.parent.TransformPoint(currentXProj), Color.yellow);
        localTargetJoystickRotation.x += Vector3.SignedAngle(currentXProj, targetXProj, Vector3.right);
        localTargetJoystickRotation.y += Vector3.SignedAngle(currentYProj, targetYProj, Vector3.up);
        localTargetJoystickRotation.z += Vector3.SignedAngle(currentZProj, targetZProj, Vector3.forward);

        Vector3 clampedLocalJoystickPosition = new Vector3(
            Mathf.Clamp((localTargetJoystickRotation.x + 180) % 360 - 180, minAngles.x, maxAngles.x),
            Mathf.Clamp((localTargetJoystickRotation.y + 180) % 360 - 180, minAngles.y, maxAngles.y),
            Mathf.Clamp((localTargetJoystickRotation.z + 180) % 360 - 180, minAngles.z, maxAngles.z));

        // Save angle in [0 - 1] range
        currentValue = new Vector3(
            (clampedLocalJoystickPosition.x - minAngles.x) / (maxAngles.x - minAngles.x),
            (clampedLocalJoystickPosition.y - minAngles.y) / (maxAngles.y - minAngles.y),
            (clampedLocalJoystickPosition.z - minAngles.z) / (maxAngles.z - minAngles.z));

        transform.parent.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(clampedLocalJoystickPosition));
    }

    public virtual void PressTrigger()
    {
    }
    public virtual void ReleaseTrigger()
    {
    }
}