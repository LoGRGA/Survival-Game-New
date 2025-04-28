using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameObject tickImage; // Assign your green tick UI image here
    public KeyCode interactKey = KeyCode.F; // Key to press
    public Transform player; // Drag your player object here

    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            Interact();
        }
    }

    private void Interact()
    {
        if (tickImage != null)
        {
            tickImage.SetActive(true);
        }
        // Optional: Disable door, play animation, etc.
        Debug.Log("Door interacted and tick set active!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            playerInRange = false;
        }
    }
}



