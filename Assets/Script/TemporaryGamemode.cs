using UnityEngine;

public class TemporaryGamemode : MonoBehaviour
{
    public GameObject player;
    public GameObject buyMenuPoint;

    private static TemporaryGamemode gamemode;
    public static TemporaryGamemode Gamemode => gamemode;

    void Awake()
    {
        gamemode = this;
    }

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