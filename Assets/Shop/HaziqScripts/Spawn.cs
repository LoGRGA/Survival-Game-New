using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class Spawn : MonoBehaviour
{
    public GameObject itemPrefab;
    private Transform player;

    public int itemPrice;

    public string itemName;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void SpawnDroppedItem()
    {
        //drop the item to spawn or overworld
        Vector3 playerposition = new Vector3(player.position.x, player.position.y, player.position.z + 4);
        Instantiate(itemPrefab, playerposition, Quaternion.identity);
    }
}
