using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The base class of a player
/// </summary>
public class PlayerManager : MonoBehaviour
{
    private static PlayerManager localPlayer;
    public static PlayerManager LocalPlayer { get { return localPlayer; } }

    [HideInInspector]
    public PlaneActor controlledPlane;

    /// <summary>
    /// Enable and disable player inputs
    /// </summary>
    [HideInInspector]
    public bool disableInputs = false;

    private void OnEnable()
    {
        localPlayer = this;
    }

    private void OnDisable()
    {
        localPlayer = null;
    }

    [HideInInspector]
    public UnityEvent<PlaneActor> OnPossessPlane = new UnityEvent<PlaneActor>();
    
    void Start()
    {
        disableInputs = GameplayManager.Singleton.Menu;
        if (disableInputs)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    /*
    [ServerRpc]
    public void RequestPlaneServerRpc()
    {
        AirportActor foundAirport = AirportActor.GetClosestAirport(PlaneTeam.Blue, new Vector3(0, 0, 0));
        if (!foundAirport)
            return;

        foreach (var spawnpoint in foundAirport.GatherSpawnpoints())
        {
            if (spawnpoint.useForAI)
                continue;
            GameObject plane = spawnpoint.SpawnPlane(false, OwnerClientId);
            if (!plane) return;
            NetworkObject planeNet = plane.GetComponent<NetworkObject>();
            OnPlaneSpawnedClientRpc(planeNet.NetworkObjectId);
            return;
        }
        Debug.LogError("failed to spawn plane");
    }

    [ClientRpc]
    public void OnPlaneSpawnedClientRpc(ulong planeId)
    {
        NetworkObject viewPlaneNet = GetNetworkObject(planeId);
        if (!viewPlaneNet)
        {
            Debug.LogError("plane spawned but cannot be found on client side : " + planeId);
            return;
        }
        controlledPlane = viewPlaneNet.GetComponent<PlaneActor>();
        OnPossessPlane.Invoke(controlledPlane);
    }
    */

    /// <summary>
    /// Make the player possess and control the given plane
    /// </summary>
    /// <param name="plane"></param>
    public void PossessPlane(PlaneActor plane)
    {
        controlledPlane = plane;
        OnPossessPlane.Invoke(controlledPlane);
    }
}
