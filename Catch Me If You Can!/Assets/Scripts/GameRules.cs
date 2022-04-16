using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour
{
    Spawner spawner;
    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= -30)
        {
            spawner.instantiatePlayer(transform);
            Destroy(gameObject);
            
        }
    }
}
