using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    //reference to the inventory
    private InventoryController inventory;
    public GameObject itemButton;

    //name of the item in the inventory
    public string itemName;
    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<InventoryController>();
    }

    //public void PickupItem()
    //{
    //    for (int i = 0; i < inventory.slots.Length; i++)
    //    {

    //        //stack items
    //        //check if inventory is full  //max is 2 in a slot
    //        if (inventory.isFull[i] == true && inventory.slots[i].transform.GetComponent<Slot>().amount < 2)
    //        {
    //            //if the item that I pick up is the same the one in the inventory, it will stack
    //            if (itemName == inventory.slots[i].transform.GetComponentInChildren<Spawn>().itemName) //stack
    //            {
    //                Destroy(gameObject);
    //                inventory.slots[i].GetComponent<Slot>().amount += 1;
    //                break;
    //            }
    //        }
    //        else if (inventory.isFull[i] == false)
    //        {
    //            //if not stackable
    //            inventory.isFull[i] = true;
    //            Instantiate(itemButton, inventory.slots[i].transform, false);
    //            inventory.slots[i].GetComponent<Slot>().amount += 1;
    //            Destroy(gameObject);
    //            break;
    //        }


    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < inventory.slots.Length; i++)
            {

                //stack items
                //check if inventory is full  //max is 2 in a slot
                if (inventory.isFull[i] == true && inventory.slots[i].transform.GetComponent<Slot>().amount < 6)
                {
                    //if the item that I pick up is the same the one in the inventory, it will stack
                    if (itemName == inventory.slots[i].transform.GetComponentInChildren<Spawn>().itemName) //stack
                    {
                        Destroy(gameObject);
                        inventory.slots[i].GetComponent<Slot>().amount += 1;
                        break;
                    }
                }
                else if (inventory.isFull[i] == false)
                {
                    //if not stackable
                    inventory.isFull[i] = true;
                    Instantiate(itemButton, inventory.slots[i].transform, false);
                    inventory.slots[i].GetComponent<Slot>().amount += 1;
                    Destroy(gameObject);
                    break;
                }


            }
        }
    }
}
