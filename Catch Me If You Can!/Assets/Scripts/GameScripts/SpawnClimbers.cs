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
    [SerializeField] float disBwLevels = 8f;
    [SerializeField] float maxHeight = 10f;
    [SerializeField] int totalLevels = 4;
    [SerializeField] float minHeightOfClimber = 1f;
    bool hasServerStarted = false;
    float heightChangeForLtoL;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            hasServerStarted = true;
        };
        heightChangeForLtoL = (center.y - minHeightOfClimber) / totalLevels;
    }

    public void ShuffleClimbers()
    {
        if (hasServerStarted)
        {
            GameObject networkClimber = Instantiate(climber, center, Quaternion.Euler(-90, 0, 0), parent.transform);
            networkClimber.GetComponent<NetworkObject>().Spawn();
            SpawnLevel(1, 5);
            SpawnLevel(2, 12);
            SpawnLevel(3, 18);
            SpawnLevel(4, 24);
        }
        else
        {
            Debug.Log("Server not started yet");
        }
    }

    void SpawnLevel(int levelNum, int noOfClimbers)
    {
        float distance = levelNum * disBwLevels;
        Vector3 point = Vector3.zero;
        point.y = -1 * levelNum * heightChangeForLtoL;
        float yValue = point.y;

        float toRadians = 0.01745f;
        for(int i=0;i<noOfClimbers;i++)
        {
            // randomizing climber position in y direction
            float randomMagnitude = Random.Range(-0.5f, 0.5f);
            point.y += randomMagnitude;

            float angle = ((float)i * 360) / noOfClimbers;
            point.x = distance * Mathf.Cos(angle * toRadians);
            point.z = distance * Mathf.Sin(angle * toRadians);
            GameObject networkClimber = Instantiate(climber, point + center, Quaternion.Euler(-90, 0, 0), parent.transform);
            networkClimber.GetComponent<NetworkObject>().Spawn();

            point.y = yValue;
            // back to previous value
        }
    }
}
