using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOpen : MonoBehaviour
{
    //check whether the player is close to the npc and create the trigger area
    public bool playerInRange;

    public bool isTalkingWithPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            playerInRange = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            playerInRange = false;

        }
    }

    internal void StartConversation()
    {
        isTalkingWithPlayer = true;

        print("Conversation started");
    }
}
