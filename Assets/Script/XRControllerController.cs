using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class XRControllerController : MonoBehaviour
{
    private SwitchBase targetSwitch = null;
    private JoystickBase targetJoystick = null;
    private List<SwitchBase> switchsInTargetArea = new();


    private float handLength = 0.5f;
    private float scanRadius = 0.1f;

    private bool isGrabbingSwitch = false;
    private bool isGrabbingJoystick = false;

    void OnDrawGizmos()
    {
        if (targetSwitch)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawSphere(targetSwitch.transform.position, 0.02f);
        }
    }

    Vector3 GetPointOnLine(Vector3 p, Vector3 a, Vector3 b)
    {
        return a + Vector3.Project(p - a, b - a);
    }

    void Update()
    {
        // Don't update switch until we released current one
        if (isGrabbingSwitch && targetSwitch)
        {
            // Update selected switch finger transform
            targetSwitch.SetControllerTransforms(transform.position, transform.rotation);
            return;
        }

        if (isGrabbingJoystick && targetJoystick)
        {
            // Update selected joystick finger transform
            targetJoystick.SetControllerTransforms(transform.position, transform.rotation);
            return;
        }

        // UnHover last frame's switches
        foreach (SwitchBase sw in switchsInTargetArea)
            if (!sw.IsDestroyed())
                sw.StopOver();
        switchsInTargetArea.Clear();

        // Retrieve all switch and joysticks in hand direction
        List<RaycastHit> switchHits = new();
        List<RaycastHit> joystickHits = new();
        foreach (RaycastHit hit in Physics.SphereCastAll(transform.position, scanRadius, transform.forward, handLength))
        {
            if (hit.collider.GetComponent<SwitchBase>())
                switchHits.Add(hit);
            if (hit.collider.GetComponent<JoystickBase>())
                joystickHits.Add(hit);
        }

        // Release and unTarget joystick and switches if we are not targeting it anymore
        if (switchHits.Count != 0)
        {
            // Get closest switch from hand direction
            RaycastHit closestSwPoint = switchHits[0];
            float closestSwDistance = float.PositiveInfinity;
            foreach (RaycastHit raycastHit in switchHits)
            {
                float distance = Vector3.Distance(raycastHit.collider.transform.position, GetPointOnLine(
                    raycastHit.collider.transform.position, transform.position,
                    transform.position + transform.forward * handLength));
                if (distance < closestSwDistance)
                {
                    closestSwDistance = distance;
                    closestSwPoint = raycastHit;
                }
            }

            targetSwitch = closestSwPoint.collider.GetComponent<SwitchBase>();

            // Get all switch around closest switch and mark them as hovered
            Vector3 hitPoint = GetPointOnLine(closestSwPoint.point, transform.position,
                transform.position + transform.forward * handLength);
            foreach (RaycastHit raycastHit in switchHits)
            {
                if (Vector3.Distance(hitPoint, closestSwPoint.point) < scanRadius)
                {
                    SwitchBase sw = raycastHit.collider.GetComponent<SwitchBase>();
                    if (sw)
                    {
                        sw.StartOver();
                        switchsInTargetArea.Add(sw);
                    }
                }
            }
        }
        else
        {
            if (targetSwitch && isGrabbingSwitch)
                targetSwitch.Release();
            isGrabbingSwitch = false;
            targetSwitch = null;
        }

        if (joystickHits.Count != 0)
        {
            // Get closest joystick from hand direction
            RaycastHit closestJsPoint = joystickHits[0];
            float closestJsDistance = float.PositiveInfinity;
            foreach (RaycastHit raycastHit in joystickHits)
            {
                float distance = Vector3.Distance(raycastHit.collider.transform.position, GetPointOnLine(
                    raycastHit.collider.transform.position, transform.position,
                    transform.position + transform.forward * handLength));
                if (distance < closestJsDistance)
                {
                    closestJsDistance = distance;
                    closestJsPoint = raycastHit;
                }
            }

            targetJoystick = closestJsPoint.collider.GetComponent<JoystickBase>();
        }
        else
        {
            if (targetJoystick && isGrabbingJoystick)
                targetJoystick.Release();
            isGrabbingJoystick = false;
            targetJoystick = null;
        }
    }

    public void GrabJoystick()
    {
        // Don't grab joystick if we are currently using a switch
        if (!targetJoystick || isGrabbingSwitch)
            return;

        targetJoystick.Grab();
        isGrabbingJoystick = true;
    }

    public void GrabInteraction()
    {
        // Don't grab a switch if we are currently using a joystick
        if (isGrabbingJoystick && targetJoystick)
        {
            // But send trigger action to joystick instead
            targetJoystick.PressTrigger();
            return;
        }

        if (targetSwitch)
            targetSwitch.Press(transform.position, transform.rotation);
        isGrabbingSwitch = true;
    }

    public void ReleaseJoystick()
    {
        if (targetSwitch && isGrabbingJoystick)
            targetJoystick.Release();
        isGrabbingJoystick = false;
    }

    public void ReleaseInteraction()
    {
        if (targetSwitch && isGrabbingSwitch)
            targetSwitch.Release();
        isGrabbingSwitch = false;
    }
}