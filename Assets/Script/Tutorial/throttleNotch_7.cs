using UnityEngine;

public class ThrottleNotch_7 : TutorialElement
{
    void Start()
    {
        if (PlayerController.LocalPlayerController.ControlledPlane)
        {
            SetTarget(PlayerController.LocalPlayerController.ControlledPlane
                .GetComponentInChildren<ThrustJoystick>().gameObject);
        }
    }

    void Update()
    {
        if (PlayerController.LocalPlayerController.ControlledPlane)
        {
            // Wait for APU full start
            if (PlayerController.LocalPlayerController.ControlledPlane.ThrottleNotch)
                Next();
        }
    }
}