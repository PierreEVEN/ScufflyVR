using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaneController))]
public class CameraController : MonoBehaviour
{
    private PlayerController player;
    public GameObject HMD;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.ControlledPlane)
        {
            //player.ControlledPlane.pilotEyePoint;
        }

    }

    private bool IsInsideCockpit = false;
}
