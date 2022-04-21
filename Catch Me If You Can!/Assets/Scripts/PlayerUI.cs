using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class PlayerUI : NetworkBehaviour
{
    [SerializeField] Text groundStatusText;
    [SerializeField] Text velocityText;
    [SerializeField] Text horizontalPowerText;

    private void Start()
    {
        if (!(IsClient && IsOwner))
        {
            groundStatusText.enabled = false;
            velocityText.enabled = false;
            horizontalPowerText.enabled = false;
        }
    }
}
