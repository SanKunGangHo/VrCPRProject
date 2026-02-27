using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            NetworkObject runner = Runner.Spawn(PlayerPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            runner.AssignInputAuthority(player);
        }
    }
}
