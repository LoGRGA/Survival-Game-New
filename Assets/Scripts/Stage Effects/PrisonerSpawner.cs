using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisionerSpawner : MonoBehaviour
{
    public GameObject prisonerPrefab;
    public Transform[] spawnPoints;

    private int prisonerAmount = 4;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < prisonerAmount; i++)
        {
            SpawnPrisoner();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void SpawnPrisoner(){
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(prisonerPrefab, spawnPoint.position, Quaternion.identity, spawnPoint.parent);
    }
}
