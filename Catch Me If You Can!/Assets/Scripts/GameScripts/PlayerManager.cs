using UnityEngine;
using HasanPrakash.Singlestons;
using Unity.Netcode;

public class PlayerManager : NetworkingSingleton<PlayerManager>
{
    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();

    public int PlayersInGame
    {
        get
        {
            return playersInGame.Value;
        }
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) => {
            if (IsServer)
            {
                Debug.Log($"Client {id} connected");
                playersInGame.Value++;
            }
        };
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsServer)
            {
                Debug.Log($"Client {id} disconnected");
                playersInGame.Value--;
            }
        };
    }
}
