using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class Movements_1 : TutorialElement
{
	private Vector3 startPosition;
    private Quaternion startRotation;

    private float time = 0;

    void OnDestroy()
    {
        if (PlayerController.LocalPlayerController.LeftXRController)
        {
            List<GameObject> children = new();
            PlayerController.LocalPlayerController.LeftXRController.GetChildGameObjects(children);

            PlayerController.LocalPlayerController.LeftXRController.layer = 0;
            foreach (GameObject child in children)
                child.layer = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (time < 2)
        {
            if (time + Time.deltaTime >= 2)
            {
                startPosition = PlayerController.LocalPlayerController.transform.position;
                startRotation = PlayerController.LocalPlayerController.transform.rotation;

                if (PlayerController.LocalPlayerController.LeftXRController)
                {
                    List<GameObject> children = new();
                    PlayerController.LocalPlayerController.LeftXRController.GetChildGameObjects(children);

                    PlayerController.LocalPlayerController.LeftXRController.layer = 3;
                    foreach (GameObject child in children)
                        child.layer = 3;
                }
            }

            time += Time.deltaTime;
        }
        else
        {
            if (PlayerController.LocalPlayerController)
                SetTarget(PlayerController.LocalPlayerController.LeftXRController);

            if (startPosition != PlayerController.LocalPlayerController.transform.position && startRotation != PlayerController.LocalPlayerController.transform.rotation)
                Next();
        }
    }
}