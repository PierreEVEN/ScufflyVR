using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TemporaryGamemode : MonoBehaviour
{
    public GameObject player;

    private GameObject buyMenuPoint;

    // Start is called before the first frame update
    void Start()
    {
        buyMenuPoint = GameObject.FindGameObjectWithTag("BuySpawnPoint");
        GoToBuyPoint();
    }

    public void GoToBuyPoint()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.MoveToLocation(buyMenuPoint.transform.position,
            Vector3.SignedAngle(buyMenuPoint.transform.forward, new Vector3(1, 0, 0), Vector3.up));
    }
}