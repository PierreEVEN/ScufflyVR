using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CameraController))]
public class PlayerController : MonoBehaviour
{
    private PlaneActor controlledPlane;
    private CameraController cameraController;
    public PlaneActor ControlledPlane => controlledPlane;
    public GameObject LeftXRController;
    public GameObject RightXRController;

    public void SetControlledPlane(GameObject plane)
    {
        if (controlledPlane)
        {
            controlledPlane.EnableIndoor(false);
        }

        transform.parent = null;
        controlledPlane = plane.GetComponent<PlaneActor>();

        if (controlledPlane)
        {
            controlledPlane.EnableIndoor(true);
            transform.parent = controlledPlane.PilotEyePoint.transform;
            OnCenterCamera(new InputValue());
        }
    }

    public void OnCenterCamera(InputValue input)
    {
        if (controlledPlane)
        {
            if (!cameraController)
                cameraController = GetComponent<CameraController>();

            transform.position = controlledPlane.PilotEyePoint.transform.position - controlledPlane.PilotEyePoint.transform.rotation * cameraController.HMD.transform.localPosition;
            transform.rotation = controlledPlane.PilotEyePoint.transform.rotation;
        }
    }

    void Start()
    {
        cameraController = GetComponent<CameraController>();
    }

    void Update()
    {
    }

    private bool wasRightTriggerPressed = false;
    void OnXRTriggerRight(InputValue input)
    {
        if (input.isPressed)
        {
            if (!wasRightTriggerPressed)
            {
                RightXRController.GetComponent<XRControllerController>().GrabInteraction();
                wasRightTriggerPressed = true;
            }
        }
        else
        {
            RightXRController.GetComponent<XRControllerController>().ReleaseInteraction();
            wasRightTriggerPressed = false;
        }
    }

    private bool wasLeftTriggerPressed = false;
    void OnXRTriggerLeft(InputValue input)
    {
        if (input.isPressed)
        {
            if (!wasLeftTriggerPressed)
            {
                LeftXRController.GetComponent<XRControllerController>().GrabInteraction();
                wasLeftTriggerPressed = true;
            }
        }
        else
        {
            LeftXRController.GetComponent<XRControllerController>().ReleaseInteraction();
            wasLeftTriggerPressed = false;
        }
    }

    private bool wasRightSecondaryTriggerPressed = false;
    void OnXRSecondaryTriggerRight(InputValue input)
    {
        if (input.isPressed)
        {
            if (!wasRightSecondaryTriggerPressed)
            {
                RightXRController.GetComponent<XRControllerController>().GrabJoystick();
                wasRightSecondaryTriggerPressed = true;
            }
        }
        else
        {
            RightXRController.GetComponent<XRControllerController>().ReleaseJoystick();
            wasRightSecondaryTriggerPressed = false;
        }
    }

    private bool wasLeftSecondaryTriggerPressed = false;
    void OnXRSecondaryTriggerLeft(InputValue input)
    {
        if (input.isPressed)
        {
            if (!wasLeftSecondaryTriggerPressed)
            {
                LeftXRController.GetComponent<XRControllerController>().GrabJoystick();
                wasLeftSecondaryTriggerPressed = true;
            }
        }
        else
        {
            LeftXRController.GetComponent<XRControllerController>().ReleaseJoystick();
            wasLeftSecondaryTriggerPressed = false;
        }
    }
}