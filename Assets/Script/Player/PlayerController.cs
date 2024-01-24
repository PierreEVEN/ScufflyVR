using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(CameraController))]
public class PlayerController : MonoBehaviour
{
    private static PlayerController localPlayerController = null;

    public static PlayerController LocalPlayerController => localPlayerController;

    private PlaneActor controlledPlane;
    private CameraController cameraController;
    public PlaneActor ControlledPlane => controlledPlane;
    public GameObject LeftXRController;
    public GameObject RightXRController;
    public GameObject teleportVisual;

    public GameObject MainMenuUI;
    private GameObject mainMenuSpawnedUI;

    private float verticalSpeed = 0;
    private float playerAngle = 0;

    public void SetControlledPlane(GameObject plane)
    {
        if (controlledPlane)
        {
            controlledPlane.EnableIndoor(false);
        }

        transform.parent = null;

        if (!plane)
        {
            controlledPlane.OnDestroyed.RemoveListener(PlaneDestroyed);
            controlledPlane = null;
            return;
        }

        controlledPlane = plane.GetComponent<PlaneActor>();
        controlledPlane.EnableIndoor(true);
        transform.parent = controlledPlane.PilotEyePoint.transform;
        OnCenterCamera(new InputValue());
        controlledPlane.OnDestroyed.AddListener(PlaneDestroyed);
        verticalSpeed = 0;
    }

    public void MoveToLocation(Vector3 location, float angle)
    {
        if (controlledPlane)
            SetControlledPlane(null);

        playerAngle = angle;
        transform.position = location;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }

    void PlaneDestroyed(PlaneActor plane)
    {
        plane.OnDestroyed.RemoveListener(PlaneDestroyed);
        SetControlledPlane(null);
        transform.position -= cameraController.HMD.transform.forward * 25;
        transform.position = new Vector3(transform.position.x, Mathf.Max(transform.position.y, plane.transform.position.y + 10), transform.position.z);
    }

    public void OnCenterCamera(InputValue input)
    {
        if (controlledPlane)
        {
            if (!cameraController)
                cameraController = GetComponent<CameraController>();

            transform.position = controlledPlane.PilotEyePoint.transform.position -
                                 controlledPlane.PilotEyePoint.transform.rotation *
                                 cameraController.HMD.transform.localPosition;
            transform.rotation = controlledPlane.PilotEyePoint.transform.rotation;
        }
    }

    public GameObject GetHmd()
    {
        return cameraController.HMD;
    }

    void Awake()
    {
        localPlayerController = this;
        cameraController = GetComponent<CameraController>();
    }

    private bool wasRightTriggerPressed = false;

    void OnXRTriggerRight(InputValue input)
    {
        if (mainMenuSpawnedUI)
            return;
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
        EnterTargetVehicle();
    }

    private bool wasLeftTriggerPressed = false;

    void OnXRTriggerLeft(InputValue input)
    {
        if (mainMenuSpawnedUI)
            return;
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

        EnterTargetVehicle();
    }


    void EnterTargetVehicle()
    {
        if (mainMenuSpawnedUI)
            return;
        if (!controlledPlane)
        {
            XRRayInteractor rightInteractor = RightXRController.GetComponent<XRRayInteractor>();
            if (rightInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                PlaneActor plane = hit.collider.GetComponent<PlaneActor>();
                if (!plane)
                    plane = hit.collider.GetComponentInParent<PlaneActor>();
                if (plane)
                {
                    SetControlledPlane(plane.gameObject);
                }
            }
        }
    }

    void OnExitVehicle(InputValue input)
    {
        if (controlledPlane)
            SetControlledPlane(null);
    }
    private bool wasRightSecondaryTriggerPressed = false;

    void OnXRSecondaryTriggerRight(InputValue input)
    {
        if (mainMenuSpawnedUI)
            return;
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
        if (mainMenuSpawnedUI)
            return;
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
            mainMenuSpawnedUI = Instantiate(MainMenuUI);
            mainMenuSpawnedUI.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
    }

    void Update()
    {
        if (!transform.parent)
        {
            // Max falling speed of 10m/s
            verticalSpeed = Mathf.Max(verticalSpeed - Time.deltaTime * 4.905f, -10f);
            // Always teleport player to terrain point
            if (Physics.Raycast(transform.position + Vector3.up * 1.5f, -Vector3.up, out RaycastHit result))
            {
                if (result.distance <= 1.5f)
                {
                    verticalSpeed = 0;
                    transform.position = result.point;
                }
            }

            transform.position += Vector3.up * verticalSpeed * Time.deltaTime;
        }


        teleportVisual.SetActive(false);
        if (startTeleport && !controlledPlane)
        {
            XRRayInteractor interactor = LeftXRController.GetComponent<XRRayInteractor>();
            if (Physics.Raycast(interactor.rayEndPoint + Vector3.up * 0.01f, -Vector3.up, out RaycastHit result))
            {
                teleportVisual.SetActive(true);
                teleportVisual.transform.position = result.point;
                teleportVisual.transform.rotation = Quaternion.AngleAxis(playerAngle + 90, Vector3.up); ;
            }
        }
    }

    private bool startTeleport = false;

    public void OnRotateCamera(InputValue input)
    {
        if (!startTeleport && !controlledPlane)
        {
            MoveToLocation(transform.position, playerAngle + Mathf.Round(input.Get<Vector2>().x * 35));
        }
    }

    public void OnTeleport(InputValue input)
    {
        if (input.Get<Vector2>().y > 0.8f && !startTeleport && !controlledPlane)
        {
            startTeleport = true;
        }
        else if (input.Get<Vector2>().magnitude < 0.4f) // release
        {
            if (startTeleport)
            {
                XRRayInteractor interactor = LeftXRController.GetComponent<XRRayInteractor>();
                if (Physics.Raycast(interactor.rayEndPoint + Vector3.up * 0.01f, -Vector3.up, out RaycastHit result))
                {
                    MoveToLocation(result.point, playerAngle);
                }
            }

            startTeleport = false;
        }
    }

    void OnDismissTutorial(InputValue input)
    {
        GetComponentInChildren<TutorialManager>().Dismiss();
    }
}