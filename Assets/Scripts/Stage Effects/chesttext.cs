using UnityEngine;

public class TreasureChestMessage : MonoBehaviour
{
    public GameObject interactionBorder; // Drag the UI Image (interactionBorder) here

    private bool isPlayerNearby = false;

    void Start()
    {
        if (interactionBorder != null)
        {
            interactionBorder.SetActive(false); // Hide UI at start
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            interactionBorder?.SetActive(true); // Show UI when near
           
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactionBorder?.SetActive(false); // Hide UI when far
        }
    }
}
