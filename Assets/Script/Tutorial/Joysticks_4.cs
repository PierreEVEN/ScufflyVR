using UnityEngine;

public class Josyticks_4 : TutorialElement
{
    void Start()
    {
        if (PlayerController.LocalPlayerController.ControlledPlane)
            SetTarget(PlayerController.LocalPlayerController.ControlledPlane.gameObject.GetComponentInChildren<ControlSurfaceJoystick>(true).gameObject);
    }

    void Update()
    {
        if (PlayerController.LocalPlayerController.ControlledPlane)
        {
            if (
                Mathf.Abs(PlayerController.LocalPlayerController.ControlledPlane.PitchInput) > 0.5f ||
                Mathf.Abs(PlayerController.LocalPlayerController.ControlledPlane.RollInput) > 0.5f)
                Next();
        }
    }
}