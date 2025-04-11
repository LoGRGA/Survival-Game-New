using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //public float interactionDistance = 3f;
    //public KeyCode interactKey = KeyCode.E;
    //private GameObject currentNPC;
    public bool isOpen = false;                   // whether shop is open or not/state
    private bool isPlayerNearby = false;          // Check if player is near

    void Update()
    {
        // Check for input if player is nearby
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleShop();
        }
    }

    void ToggleShop()
    {
        isOpen = !isOpen; // Toggle shop state

        

        Debug.Log(isOpen ? "Door Opened!" : "Door Closed!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
        //    currentNPC = other.gameObject;
            Debug.Log("Press E to interact");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
      //      currentNPC = null;
        }
    }



}
