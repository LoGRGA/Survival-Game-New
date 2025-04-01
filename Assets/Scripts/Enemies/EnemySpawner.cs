using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawners;

    [SerializeField] private GameObject zombie;
    private bool spawn = true;

    [SerializeField] protected int maxZombie;
    private int zombieCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            zombieCount++;
            SpawnZombie();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (zombieCount >= maxZombie)
            spawn = false;
        else if (zombieCount < maxZombie && spawn == true)
        {
            spawn = false;
            StartCoroutine(IncreaseCount());
        }
    }

    IEnumerator IncreaseCount()
    {
        yield return new WaitForSeconds(2f);
        zombieCount++;
        SpawnZombie();
        spawn = true;
    }

    public void ReduceCount()
    {
        zombieCount--;
        spawn = true;
    }

    private void SpawnZombie()
    {
        int randomInt = Random.Range(0, spawners.Length);
        Transform randomSpawner = spawners[randomInt];
        Instantiate(zombie, randomSpawner.position, randomSpawner.rotation);
    }
}
