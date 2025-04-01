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

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition;
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            TogglePosition();
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
}