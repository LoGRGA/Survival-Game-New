using UnityEngine;
using TMPro; // For TextMeshPro

public class DoorController : MonoBehaviour
{
    public float openAngle = 90f;                  // Angle to open the door
    public float closeAngle = 0f;                 // Angle to close the door
    public float rotationSpeed = 2f;              // Speed of door rotation
    public bool isOpen = false;                   // Track door state
    public TextMeshProUGUI interactionText;       // Reference to UI text

    private Quaternion targetRotation;            // Target rotation
    private bool isPlayerNearby = false;          // Check if player is near

    void Start()
    {
        targetRotation = Quaternion.Euler(0f, closeAngle, 0f); // Start closed

        // Hide the interaction text at the start
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Rotate the door smoothly to the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Check for input if player is nearby
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            ToggleDoor();
        }
    }

    void ToggleDoor()
    {
        isOpen = !isOpen; // Toggle door state

        // Set the target rotation
        targetRotation = isOpen ? Quaternion.Euler(0f, openAngle, 0f) : Quaternion.Euler(0f, closeAngle, 0f);

        Debug.Log(isOpen ? "Door Opened!" : "Door Closed!");
    }

    // Detect if player is near the door
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(true);
                interactionText.text = "Press F to " + (isOpen ? "close" : "open") + " the door";
            }
            Debug.Log("Player is near the door. Press 'F' to interact.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
            Debug.Log("Player left the door area.");
        }
    }
}
