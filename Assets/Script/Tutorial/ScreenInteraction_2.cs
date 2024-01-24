
using UnityEngine;

public class ScreenInteraction_2 : TutorialElement
{
    void Start()
    {
        SetTarget(TemporaryGamemode.Gamemode.buyMenuPoint.transform.parent.GetComponentInChildren<Canvas>().gameObject);
    }

    void Update()
    {
        if (PlaneActor.PlaneList.Count > 0)
        {
            Next();
        }
    }
}