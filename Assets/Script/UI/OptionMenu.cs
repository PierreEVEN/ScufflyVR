using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenu : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0;
    }

    void OnDestroy()
    {
        Time.timeScale = 1;
    }

    public void Continue()
    {
        Destroy(gameObject);
    }
    public void GoToStart()
    {
        GameObject buyMenuPoint = GameObject.FindGameObjectWithTag("BuySpawnPoint");
        PlayerController.LocalPlayerController.MoveToLocation(buyMenuPoint.transform.position,
            Vector3.SignedAngle(buyMenuPoint.transform.forward, new Vector3(1, 0, 0), Vector3.up));
        Destroy(gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }
}