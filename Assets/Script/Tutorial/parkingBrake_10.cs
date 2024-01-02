using UnityEngine;

public class ParkingBrake_10 : TutorialElement
{
    void Start()
    {
        if (PlayerController.LocalPlayerController.ControlledPlane)
        {
            foreach (FlipFlopSwitch component in PlayerController.LocalPlayerController.ControlledPlane
                         .GetComponentsInChildren<FlipFlopSwitch>())
            {
                if (component.modifiedProperty == ESwitchTarget.Brakes)
                {
                    SetTarget(component.gameObject);
                    break;
                }
            }
        }
    }

    void Update()
    {
        if (PlayerController.LocalPlayerController.ControlledPlane)
        {
            // Wait for APU full start
            if (!PlayerController.LocalPlayerController.ControlledPlane.ParkingBrakes)
                Next();
        }
    }
}