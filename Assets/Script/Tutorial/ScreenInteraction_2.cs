
public class ScreenInteraction_2 : TutorialElement
{
    void Start()
    {
        SetTarget(TemporaryGamemode.Gamemode.buyMenuPoint);
    }

    void Update()
    {
        if (PlaneActor.PlaneList.Count > 0)
        {
            Next();
        }
    }
}