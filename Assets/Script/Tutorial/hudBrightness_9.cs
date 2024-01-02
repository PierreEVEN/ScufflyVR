using UnityEngine;

public class HudBrightness_9 : TutorialElement
{
    void Start()
    {
        if (PlayerController.LocalPlayerController.ControlledPlane)
        {
            foreach (RotatingSwitch component in PlayerController.LocalPlayerController.ControlledPlane
                         .GetComponentsInChildren<RotatingSwitch>())
            {
                if (component.modifiedProperty == RotatingSwitchStates.HudLevel)
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
            if (PlayerController.LocalPlayerController.ControlledPlane.HudLightLevel > 0.5f)
                Next();
        }
    }
}