
using UnityEngine;

public class BuyMenu : MonoBehaviour
{
    public GameObject AISpawnPoint;
    public GameObject PlayerPlaneSpawnPoint;

    public GameObject AIPlane;
    public GameObject PlayerPlane;
    public void BuyPlane()
    {
        GameObject plane = Instantiate(PlayerPlane);
        plane.transform.position = PlayerPlaneSpawnPoint.transform.position;
        plane.transform.rotation = PlayerPlaneSpawnPoint.transform.rotation;
    }

    public void HireAI()
    {
        GameObject plane = Instantiate(AIPlane);
        plane.AddComponent<PlaneAIController>();
        plane.transform.position = AISpawnPoint.transform.position;
        plane.transform.rotation = AISpawnPoint.transform.rotation;
    }
}
