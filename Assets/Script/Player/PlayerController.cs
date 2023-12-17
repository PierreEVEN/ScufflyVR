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
            transform.parent = plane.transform;
            OnCenterCamera(new InputValue());
        }
    }

    public void OnCenterCamera(InputValue input)
    {
        if (controlledPlane)
        {
            transform.position = controlledPlane.PilotEyePoint.transform.position - controlledPlane.PilotEyePoint.transform.rotation * cameraController.HMD.transform.localPosition;
            transform.rotation = controlledPlane.PilotEyePoint.transform.rotation;

            Debug.Log("Center camera : " + cameraController.HMD.transform.localPosition);

        }
    }

    void Start()
    {
        cameraController = GetComponent<CameraController>();
    }

    void Update()
    {
    }
}