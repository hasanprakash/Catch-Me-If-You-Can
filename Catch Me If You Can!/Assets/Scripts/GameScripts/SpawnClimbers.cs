using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using HasanPrakash.Singlestons;
using TMPro;
using System.Threading.Tasks;

public class SpawnClimbers : NetworkingSingleton<SpawnClimbers>
{
    [SerializeField] GameObject climber;
    [SerializeField] GameObject parent;
    [SerializeField] Vector3 center;
    bool hasServerStarted = false;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            hasServerStarted = true;
        };
    }

    public void ShuffleClimbers()
    {
        if (hasServerStarted)
        {
            GameObject networkClimber = Instantiate(climber, center, Quaternion.Euler(-90, 0, 0), parent.transform);
            networkClimber.GetComponent<NetworkObject>().Spawn();
        }
        else
        {
            Debug.Log("Server not started yet");
        }
    }
}
