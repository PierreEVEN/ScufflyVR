using UnityEngine;

public class Apu_6 : TutorialElement
{
    void Start()
    {
        if (PlayerController.LocalPlayerController.ControlledPlane)
        {
            foreach (FlipFlopSwitch component in PlayerController.LocalPlayerController.ControlledPlane
                         .GetComponentsInChildren<FlipFlopSwitch>())
            {
                if (component.modifiedProperty == ESwitchTarget.APU)
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
            if (PlayerController.LocalPlayerController.ControlledPlane.EnableAPU && PlayerController.LocalPlayerController.ControlledPlane.GetCurrentPower() > 55)
                Next();
        }
    }
}