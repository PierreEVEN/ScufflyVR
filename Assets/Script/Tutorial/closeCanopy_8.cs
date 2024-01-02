using System;
using UnityEngine;

public class CloseCanopy_8 : TutorialElement
{
    void Start()
    {
        if (PlayerController.LocalPlayerController.ControlledPlane)
        {
            SetTarget(PlayerController.LocalPlayerController.ControlledPlane
                .GetComponentInChildren<CanopyAudioAttenuation>().gameObject);
        }
    }

    void Update()
    {
        if (PlayerController.LocalPlayerController.ControlledPlane)
        {
            Debug.Log("Canopy : " + PlayerController.LocalPlayerController.ControlledPlane.OpenCanopy);
            // Wait for APU full start
            if (!PlayerController.LocalPlayerController.ControlledPlane.OpenCanopy)
                Next();
        }
    }
}