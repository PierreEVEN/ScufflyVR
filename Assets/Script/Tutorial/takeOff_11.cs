using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class TaleOff_11 : TutorialElement
{
    void Start()
    {
        SetTarget(null);
    }

    void Update()
    {
        if (PlayerController.LocalPlayerController.ControlledPlane)
        {
            if (PlayerController.LocalPlayerController.ControlledPlane.transform.position.y > 200)
                Next();
        }
    }
}