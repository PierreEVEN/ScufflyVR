using UnityEngine;

/// <summary>
/// Wip multiplayer manager
/// //@TODO MULTIPLAYER !!!
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager _singleton;
    [HideInInspector]
    public static GameManager Singleton
    {
        get
        {
            if (!_singleton) _singleton = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            return _singleton;
        }
    }

    private void Start()
    {
        Destroy(this);
    }

    private void OnGUI()
    {
    }

    public void GoToMenu()
    {

    }
}
