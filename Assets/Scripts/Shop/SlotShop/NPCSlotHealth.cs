using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCSlotHealth : MonoBehaviour
{
    //References
    private Player player;
    private InventoryController inventory;
    //*References For Health Warning Panel 15/04/2025
    private PlayerController playerC;


    public Image itemImage;
    //item Name
    public TextMeshProUGUI itemName;
    //item Price
    public TextMeshProUGUI itemPrice;
    //item AMount
    public TextMeshProUGUI itemAmount;

    public GameObject itemToBuy;
    public int itemAmountINT;
    public TextMeshProUGUI buyPriceText;

    
   
    // Start is called before the first frame update
    void Start()
    {

        player = FindObjectOfType<Player>();
        inventory = FindObjectOfType<InventoryController>();

        playerC = FindObjectOfType<PlayerController>();

        itemName.text = itemToBuy.GetComponent<Spawn>().itemName;
        //itemImage.sprite = itemToBuy.GetComponent<Image>().sprite;

        

        buyPriceText.text = itemToBuy.GetComponentInChildren<Spawn>().itemPrice + " Gold";
    }

    // Update is called once per frame
    void Update()
    {
        playerC = FindObjectOfType<PlayerController>();
        itemAmount.text = "Amount: " + itemAmountINT.ToString();

        // Apply discount ONLY for HealthPotion if player health <= 50
        if (itemToBuy != null)
        {
            Spawn spawnComponent = itemToBuy.GetComponentInChildren<Spawn>();

            if (spawnComponent.itemName == "HealthPotion")
            {
                if (playerC.currentHealth <= playerC.maxHealth / 2)
                    spawnComponent.itemPrice = 5;
                else
                    spawnComponent.itemPrice = 10;

                buyPriceText.text = spawnComponent.itemPrice + " Gold";
            }
        }
    }

    public void Buy()
    {
        string purchasedItemName = itemToBuy.GetComponentInChildren<Spawn>().itemName;
        int itemPrice = itemToBuy.GetComponentInChildren<Spawn>().itemPrice;

        // Step 1: Handle HealthPotion specific slot (fifth slot, index 4)
        if (purchasedItemName == "HealthPotion")
        {
            int targetIndex = 4;

            if (inventory.isFull[targetIndex] == true &&
                inventory.slots[targetIndex].transform.GetComponent<Slot>().amount < 100 &&
                player.gold >= itemPrice &&
                itemAmountINT > 0)
            {
                if (purchasedItemName == inventory.slots[targetIndex].transform.GetComponentInChildren<Spawn>().itemName)
                {
                    itemAmountINT -= 1;
                    inventory.slots[targetIndex].GetComponent<Slot>().amount += 1;
                    player.gold -= itemPrice;
                    return;
                }
            }
            else if (inventory.isFull[targetIndex] == false &&
                     player.gold >= itemPrice &&
                     itemAmountINT > 0)
            {
                itemAmountINT -= 1;
                player.gold -= itemPrice;
                inventory.slots[targetIndex].GetComponent<Slot>().itemName.text = itemName.text;
                inventory.isFull[targetIndex] = true;
                Instantiate(itemToBuy, inventory.slots[targetIndex].transform, false);
                inventory.slots[targetIndex].GetComponent<Slot>().amount += 1;
                return;
            }
        }


        //General Items
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            if (inventory.isFull[i] == true && inventory.slots[i].transform.GetComponent<Slot>().amount < 100 && player.gold >= itemToBuy.GetComponentInChildren<Spawn>().itemPrice && itemAmountINT > 0)
            {
                if (itemName.text == inventory.slots[i].transform.GetComponentInChildren<Spawn>().itemName)
                {
                    itemAmountINT -= 1;
                    inventory.slots[i].GetComponent<Slot>().amount += 1;
                    player.gold -= itemToBuy.GetComponentInChildren<Spawn>().itemPrice;
                    break;
                }
            }
            else if (inventory.isFull[i] == false && player.gold >= itemToBuy.GetComponentInChildren<Spawn>().itemPrice && itemAmountINT > 0)
            {

                itemAmountINT -= 1;
                player.gold -= itemToBuy.GetComponentInChildren<Spawn>().itemPrice;
                inventory.slots[i].GetComponent<Slot>().itemName.text = itemName.text;
                inventory.isFull[i] = true;
                Instantiate(itemToBuy, inventory.slots[i].transform, false);
                inventory.slots[i].GetComponent<Slot>().amount += 1;
                break;
            }
        }

    }

    public void Sell()
    {

        for (int i = 0; i < inventory.slots.Length; i++)
        {


            if (inventory.slots[i].transform.GetComponent<Slot>().amount != 0)
            {

                if (itemName.text == inventory.slots[i].transform.GetComponentInChildren<Spawn>().itemName)
                {
                    itemAmountINT += 1;
                    inventory.slots[i].GetComponent<Slot>().amount -= 1;
                    player.gold += itemToBuy.GetComponentInChildren<Spawn>().itemPrice / 2;
                    //if we dont have anything in the inventory
                    if (inventory.slots[i].GetComponent<Slot>().amount == 0)
                    {
                        //if there is no item left in the inventory, this will remove the item name text from the slot
                        inventory.slots[i].GetComponent<Slot>().itemName.text = string.Empty;
                        GameObject.Destroy(inventory.slots[i].GetComponentInChildren<Spawn>().gameObject);

                    }

                    break;
                }
            }



        }


    }
}
