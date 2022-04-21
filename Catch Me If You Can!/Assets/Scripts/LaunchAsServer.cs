using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LaunchAsServer : MonoBehaviour
{
    private void Start()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-launch-as-client")
            {
                NetworkManager.Singleton.StartClient();
            }
            else if (args[i] == "-launch-as-server")
            {
                NetworkManager.Singleton.StartServer();
            }
        }
    }
}
