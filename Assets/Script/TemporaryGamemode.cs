using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TemporaryGamemode : MonoBehaviour
{
    public GameObject plane;
    public GameObject player;

    private int nbFrames = 0;

    // Start is called before the first frame update
    void Start()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        PlaneController planeController = player.GetComponent<PlaneController>();
        planeController.EnableInputs = true;
        playerController.SetControlledPlane(plane);
    }

    // Update is called once per frame
    void Update()
    {
        // Fix wrong camera origin
        if (nbFrames++ == 60)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.OnCenterCamera(new InputValue());
        }
    }
}
