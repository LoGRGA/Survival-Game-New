using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateInteraction : MonoBehaviour
{
    public float riseAmount = 5f;              // How high the object rises
    public float moveSpeed = 2f;               // How fast the object moves
    private bool isPlayerInRange = false;      // Is the player in range?
    private bool isRaised = false;             // Is the object raised?
    private Vector3 initialPosition;           // Starting position
    private Vector3 targetPosition;            // Target position

    //HAZIQ
    public InventoryController inventory;
    //HAZIQ

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition;

        //HAZIQ
        inventory = FindObjectOfType<InventoryController>();
        //HAZIQ

    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            // Check if the key is in slot 9 (index 8)
            if (HasKeyItem())
            {
                TogglePosition();
            }
            else
            {
                Debug.Log("Gate is locked. You need a key.");
            }
        }

        // Smoothly move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Press F to raise/lower the object.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void TogglePosition()
    {
        if (isRaised)
        {
            targetPosition = initialPosition;                // Move back to the original position
            Debug.Log("Object lowering...");
        }
        else
        {
            targetPosition = initialPosition + Vector3.up * riseAmount;  // Move up
            Debug.Log("Object rising...");
        }

        isRaised = !isRaised;  // Toggle the state
    }

    //HAZIQ
    private bool HasKeyItem()
    {
        if (inventory == null || inventory.slots.Length < 11)
            return false;

        Transform slot = inventory.slots[8].transform;

        if (slot.childCount > 0)
        {
            Spawn spawnScript = slot.GetComponentInChildren<Spawn>();
            if (spawnScript != null && spawnScript.itemName == "KeyItem")
            {
                return true;
            }
        }

        return false;
    }

    //HAZIQ
}