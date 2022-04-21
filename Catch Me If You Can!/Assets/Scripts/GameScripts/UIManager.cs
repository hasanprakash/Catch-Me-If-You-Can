using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform playerName;
    [SerializeField] private Text playersCountText;
    [SerializeField] private Button startServer;
    [SerializeField] private Button startClient;
    //TMP_Text playerNameText;
    void Start()
    {
        //playerNameText = playerName.GetComponent<TMP_Text>();
        startServer.onClick.AddListener(StartMyServer);
        startClient.onClick.AddListener(StartMyClient);
    }

    void Update()
    {
        playersCountText.text = "Players Count: " + PlayerManager.Instance.PlayersInGame;
    }


    void StartMyServer()
    {
        if (NetworkManager.Singleton.StartServer())
        {
            Debug.Log("Server started");
            //playerNameText.text = "Server";
            startServer.interactable = false;
            startClient.interactable = false;
        }
        else
        {
            Debug.Log("Server failed to start");
        }
    }

    void StartMyClient()
    {
        if(NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Client started");
            //playerNameText.text = "Client";
            startServer.interactable = false;
            startClient.interactable = false;
        }
        else
        {
            Debug.Log("Client failed to start");
        }
    }
}
