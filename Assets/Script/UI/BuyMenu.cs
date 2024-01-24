
using UnityEngine;

public class BuyMenu : MonoBehaviour
{
    public GameObject AISpawnPoint;
    public GameObject PlayerPlaneSpawnPoint;

    public GameObject AIPlane;
    public GameObject PlayerPlane;
    public GameObject PlayerPlaneColdStart;

    private bool spawnedFirstPlane = false;
    private float AIrespawnDelay = 0;
    private float PlaneRespawnDelay = 0;

    public GameObject coldStartButton;
    public GameObject flightReadyButton;
    public GameObject spawnAIButton;


    void Start()
    {
        Debug.Log(spawnAIButton + " / " + spawnAIButton.GetComponent<UnityEngine.UI.Button>());
        spawnAIButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        flightReadyButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
    }

    void Update()
    {
        AIrespawnDelay -= Time.deltaTime;

        if (AIrespawnDelay < 0 && spawnedFirstPlane)
        {
            spawnAIButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }

        PlaneRespawnDelay -= Time.unscaledDeltaTime;

        if (PlaneRespawnDelay < 0 && spawnedFirstPlane)
        {
            coldStartButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
            flightReadyButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
    }

    public void BuyPlaneColdStart()
    {
        spawnedFirstPlane = true;
        GameObject plane = Instantiate(PlayerPlaneColdStart);
        plane.transform.position = PlayerPlaneSpawnPoint.transform.position;
        plane.transform.rotation = PlayerPlaneSpawnPoint.transform.rotation;
        coldStartButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        flightReadyButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        PlaneRespawnDelay = 20;
    }

    public void BuyPlane()
    {
        GameObject plane = Instantiate(PlayerPlane);
        plane.transform.position = PlayerPlaneSpawnPoint.transform.position;
        plane.transform.rotation = PlayerPlaneSpawnPoint.transform.rotation;
        coldStartButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        flightReadyButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        PlaneRespawnDelay = 20;
    }

    public void HireAI()
    {
        GameObject plane = Instantiate(AIPlane);
        plane.AddComponent<PlaneAIController>();
        plane.transform.position = AISpawnPoint.transform.position;
        plane.transform.rotation = AISpawnPoint.transform.rotation;
        spawnAIButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        AIrespawnDelay = 4;
    }
}
