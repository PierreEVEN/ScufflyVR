
public class enterPlane_3 : TutorialElement
{

    void Start()
    {
        if (PlaneActor.PlaneList.Count > 0)
            SetTarget(PlaneActor.PlaneList[0].gameObject);
    }


    void Update()
    {
        if (PlayerController.LocalPlayerController.ControlledPlane)
            Next();
    }
}