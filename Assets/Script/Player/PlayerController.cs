using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

[RequireComponent(typeof(CameraController))]
public class PlayerController : MonoBehaviour
{
    private PlaneActor controlledPlane;
    private CameraController cameraController;
    public PlaneActor ControlledPlane => controlledPlane;
    public GameObject LeftXRController;
    public GameObject RightXRController;

    public GameObject MainMenuUI;
    private GameObject mainMenuSpawnedUI;

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

    void OnPause(InputValue input)
    {
        if (mainMenuSpawnedUI)
        {
            Destroy(mainMenuSpawnedUI);
        }
        else
        {
            float widthAtOneMeter = 1;

            float resolution = 100f;
            float spawnMaxDistance = 1;

            float spawnDistance = spawnMaxDistance;
            float spawnSize = spawnDistance * widthAtOneMeter;

            // Progressive box cast with increasing size
            for (int i = 0; i < resolution; ++i)
            {
                float stepDepth = spawnMaxDistance / resolution;
                float startDistance = stepDepth * i;
                float width = startDistance * widthAtOneMeter;

                if (Physics.BoxCast(cameraController.HMD.transform.position + cameraController.HMD.transform.forward * startDistance, new Vector3(width / 2, width / 2, 0.01f),
                        cameraController.HMD.transform.forward, out RaycastHit hit, cameraController.HMD.transform.rotation, stepDepth * 2))
                {
                    spawnDistance = startDistance + hit.distance;
                    spawnSize = width;

                    break;
                }
            }

            mainMenuSpawnedUI = Instantiate(MainMenuUI);
            mainMenuSpawnedUI.transform.parent = transform;
            mainMenuSpawnedUI.transform.position = cameraController.HMD.transform.position + cameraController.HMD.transform.forward * spawnDistance;
            mainMenuSpawnedUI.transform.rotation = Quaternion.LookRotation(cameraController.HMD.transform.forward, transform.up);
            mainMenuSpawnedUI.transform.localScale = new Vector3(spawnSize, spawnSize, spawnSize);
        }
    }

    void Update()
    {
        if (mainMenuSpawnedUI)
        {
        }
    }

}