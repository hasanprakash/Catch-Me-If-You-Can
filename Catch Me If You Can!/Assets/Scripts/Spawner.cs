using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnSurfaces;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform players;
    void Start()
    {
        foreach(var surface in spawnSurfaces)
        {
            Instantiate(playerPrefab, surface.position, Quaternion.identity, players);
        }
    }

    void Update()
    {
        
    }

    public void instantiatePlayer(Transform player)
    {
        int index = Random.Range(0, spawnSurfaces.Length);
        Instantiate(player, spawnSurfaces[index].position, Quaternion.identity, players);
    }
}
