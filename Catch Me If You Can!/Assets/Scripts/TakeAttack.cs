using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeAttack : MonoBehaviour
{
    [SerializeField] GameObject mainCharacter;
    Spawner spawner;
    Manager manager;
    void Start()
    {
        manager = mainCharacter.GetComponent<Manager>();
        spawner = FindObjectOfType<Spawner>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag == "HeroWeapon" || collision.collider.gameObject.tag == "Weapon")
        {
            //Vector3 direction = collision.contacts[0].point - collision.gameObject.transform.position;
            manager.EnableRagDoll();
            StartCoroutine(SpawnPlayer(5));
        }
    }

    IEnumerator SpawnPlayer(float time)
    {
        yield return new WaitForSeconds(time);
        spawner.instantiatePlayer(mainCharacter.transform);
        //Destroy(mainCharacter);
    }
}
