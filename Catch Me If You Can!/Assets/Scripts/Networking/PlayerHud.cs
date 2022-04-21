using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerHud : NetworkBehaviour
{
    private NetworkVariable<FixedString64Bytes> m_PlayerName = new NetworkVariable<FixedString64Bytes>();
    bool isNameSet = false;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            m_PlayerName.Value = "Player " + OwnerClientId;
        }
    }

    public void SetNameText()
    {
        if (gameObject)
        {
            TMP_Text playerNameText = gameObject.GetComponentInChildren<TMP_Text>();
            playerNameText.text = m_PlayerName.Value.ToString();
            isNameSet = true;
        }
    } 

    private void Update()
    {
        if (!isNameSet && !string.IsNullOrEmpty(m_PlayerName.Value.ToString()))
        {
            SetNameText();
        }
    }
}
