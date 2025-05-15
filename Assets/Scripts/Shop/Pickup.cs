using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    //reference to the inventory
    private InventoryController inventory;
    //public GameObject weap;
    private Weapons weapons;
    public GameObject itemButton;

    //name of the item in the inventory
    public string itemName;
    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<InventoryController>();
        weapons = FindObjectOfType<Weapons>();
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
            // Check if the item is a health potion
            /*
            if (itemName == "HealthPotion") // Make sure the name matches
            {
                int targetIndex = 4; // Slot index 4 is the fifth slot

                if (inventory.isFull[targetIndex] == true &&
                    inventory.slots[targetIndex].transform.GetComponent<Slot>().amount < 6)
                {
                    if (itemName == inventory.slots[targetIndex].transform.GetComponentInChildren<Spawn>().itemName)
                    {
                        Destroy(gameObject);
                        inventory.slots[targetIndex].GetComponent<Slot>().amount += 1;
                        return;
                    }
                }
                else if (inventory.isFull[targetIndex] == false)
                {
                    inventory.isFull[targetIndex] = true;
                    Instantiate(itemButton, inventory.slots[targetIndex].transform, false);
                    inventory.slots[targetIndex].GetComponent<Slot>().amount += 1;
                    Destroy(gameObject);
                    return;
                }
            }  */

            // Step 1: Create a mapping from item names to preferred slots
            Dictionary<string, int> itemSlotMap = new Dictionary<string, int>()
        {
            { "HealthPotion", 4 },  // slot 5 Health
            { "JumpPotion", 5 },    // slot 6 Jump
            { "SpeedPotion", 6 },   // slot 7 Speed
            { "KeyItem", 8 },        //slot 9 key
            { "PickupBuffChest", 9 }
        };

            // Step 2: Check if itemName has a mapped slot
            if (itemSlotMap.ContainsKey(itemName))
            {
                int targetIndex = itemSlotMap[itemName];

                if (inventory.isFull[targetIndex] == true &&
                    inventory.slots[targetIndex].transform.GetComponent<Slot>().amount < 6)
                {
                    if (itemName == inventory.slots[targetIndex].transform.GetComponentInChildren<Spawn>().itemName)
                    {
                        Destroy(gameObject);
                        inventory.slots[targetIndex].GetComponent<Slot>().amount += 1;
                        return;
                    }
                }
                else if (inventory.isFull[targetIndex] == false)
                {
                    inventory.isFull[targetIndex] = true;
                    Instantiate(itemButton, inventory.slots[targetIndex].transform, false);
                    inventory.slots[targetIndex].GetComponent<Slot>().amount += 1;
                    Destroy(gameObject);
                    return;
                }
            }

            //Normal Behaviour For other items
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                //To Remove Weapon from Full Inventory before Pickup Up
                if (this.gameObject.tag == "Weapon")
                {
                    weapons.DropWeapon();
                }

                //stack items
                //check if inventory is full  //max is 6 in a slot
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
                    Debug.LogWarning(itemName);
                    WeapReplace();
                    inventory.isFull[i] = true;
                    Instantiate(itemButton, inventory.slots[i].transform, false);
                    inventory.slots[i].GetComponent<Slot>().amount += 1;
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }

    private void WeapReplace()
    {
        if (itemName == "PocketKnife" || itemName == "Dagger" || itemName == "Sword" || itemName == "Axe" || itemName == "ScytheHealth" ||
            itemName == "Hammer" || itemName == "Polearm" || itemName == "RunesAxe" || itemName == "LightningSword" || itemName == "Shuriken")
            weapons.ReplaceWeap(itemName);
        else
            return;  
    }
}
