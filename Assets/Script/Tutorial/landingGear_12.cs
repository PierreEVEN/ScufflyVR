using UnityEngine;

public class LandingGear_12 : TutorialElement
{
    void Start()
    {
        if (PlayerController.LocalPlayerController.ControlledPlane)
        {
            foreach (FlipFlopSwitch component in PlayerController.LocalPlayerController.ControlledPlane
                         .GetComponentsInChildren<FlipFlopSwitch>())
            {
                if (component.modifiedProperty == ESwitchTarget.Gear)
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
            if (!PlayerController.LocalPlayerController.ControlledPlane.RetractGear)
                Next();
        }
    }
}