using UnityEngine;

public class MainPower_5 : TutorialElement
{
    void Start()
    {
        if (PlayerController.LocalPlayerController.ControlledPlane)
        {
            foreach (FlipFlopSwitch component in PlayerController.LocalPlayerController.ControlledPlane
                         .GetComponentsInChildren<FlipFlopSwitch>())
            {
                if (component.modifiedProperty == ESwitchTarget.MainPower)
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
            if (PlayerController.LocalPlayerController.ControlledPlane.MainPower)
                Next();
        }
    }
}