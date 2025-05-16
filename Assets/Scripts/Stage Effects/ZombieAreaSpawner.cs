using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAreaSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;
    public GameObject weaponBoxPrefab;
    public GameObject itemBoxPrefab;
    public Transform[] spawnPoints; // Assign 8 spawn points in the inspector
    //public float spawnInterval = 10f;
    //public int zombiesPerSpawn = 2;

    private float zombieNoKeyAmount = 5;
    private float zombieHasKeyAmount = 5;
    private float weaponBoxAmount = 5;
    private float itemBoxAmount = 10;

    void Start()
    {
        for (int i = 0; i < zombieNoKeyAmount; i++){
            SpawnZombie();
        }

        for (int i = 0; i < zombieHasKeyAmount; i++){
            SpawnZombieWithKey();
        }

        for (int i = 0; i < weaponBoxAmount; i++){
            SpawnWeaponBox();
        }

        for (int i = 0; i < itemBoxAmount; i++){
            SpawnItemBox();
        }


        
    }

/*
    IEnumerator SpawnZombies()
    {
        while (true)
        {
            for (int i = 0; i < zombiesPerSpawn; i++)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
*/

    void SpawnZombie(){
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity, spawnPoint.parent);
    }

    void SpawnZombieWithKey(){
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject zombie;
        zombie = Instantiate(zombiePrefab, spawnPoint.position, Quaternion.identity, spawnPoint.parent);
        zombie.GetComponent<ZombieBehaviour>().hasKey = true;
    }

    void SpawnWeaponBox(){
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(weaponBoxPrefab, spawnPoint.position, Quaternion.identity, spawnPoint.parent);
    }

    void SpawnItemBox(){
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(itemBoxPrefab, spawnPoint.position, Quaternion.identity, spawnPoint.parent);
    }

}