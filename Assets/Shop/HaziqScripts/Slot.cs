using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour
{

    private InventoryController inventory;
    public int i;
    public TextMeshProUGUI amountText;
    public int amount;

    public TextMeshProUGUI itemName;


    // Start is called before the first frame update
    void Start()
    {
        //find object in controller
        inventory = FindObjectOfType<InventoryController>();
    }

    // Update is called once per frame
    void Update()
    {


        amountText.text = amount.ToString();

        //if we have zero amount or one amount then there should not be any text
        //now the texture will always be there if there is more than one
        if(amount > 1)
        {
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = true;
        }
        else
        {
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().enabled = false;
        }

        if(transform.childCount == 2)
        {
            //if the amount of object like text or button in a slot is two, it will return as false for isFUll
            inventory.isFull[i] = false;
        }


    }

    public void DropItem()
    {
         if(amount > 1) // if it is stacked
        {
            amount -= 1;
            transform.GetComponentInChildren<Spawn>().SpawnDroppedItem();

        }
        else
        {
            amount -= 1;
            GameObject.Destroy(transform.GetComponentInChildren<Spawn>().gameObject);
            transform.GetComponentInChildren<Spawn>().SpawnDroppedItem();
        }
    }
}
