using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private PlayerController player;
    private InventoryController inventory;

    public int i;
    public TextMeshProUGUI amountText;
    public int amount;

    public TextMeshProUGUI itemName;


    // Start is called before the first frame update
    void Start()
    {
        //find object in player controller
        inventory = FindObjectOfType<InventoryController>();

        //find obeject in player controller
        player = FindObjectOfType<PlayerController>();
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

    public void UseItem()
    {
        if (amount > 0)
        {
            player.Heal(10);                        // Reduce HP by 10
            //player.ChangeJump(10);  //Jump HP by 10
            amount--;

            if (amount <= 0)
            {
                // Optionally destroy the item visuals or GameObject
                Destroy(transform.GetChild(1).gameObject); // Assuming 1 = item prefab
            }
        }
    }

    public void UseItemJump()
    {
        if (amount > 0)
        {
            player.ChangeJump(10);                        // Reduce HP by 10
            //player.ChangeJump(10);  //Jump HP by 10
            amount--;

            if (amount <= 0)
            {
                // Optionally destroy the item visuals or GameObject
                Destroy(transform.GetChild(1).gameObject); // Assuming 1 = item prefab
            }
        }
    }

    public void UseItemSpeed()
    {
        if (amount > 0)
        {
            player.ChangeSpeed(10);                        // Reduce HP by 10
            //player.ChangeJump(10);  //Jump HP by 10
            amount--;

            if (amount <= 0)
            {
                // Optionally destroy the item visuals or GameObject
                Destroy(transform.GetChild(1).gameObject); // Assuming 1 = item prefab
            }
        }
    }

    public InventoryController GetInventory()
    {
        return inventory;
    }
}
