using System.Collections;
using UnityEngine;

public class AOE_Damage : MonoBehaviour
{
    public float damagePerSecond = 1f; // Damage applied per second
    private bool playerInside = false; // Track if player is inside the area
    private PlayerController playerController; // Reference to the player's script

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure only the player takes damage
        {
            playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerInside = true;
                StartCoroutine(ApplyDamage());
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Stop damage when the player leaves
        {
            playerInside = false;
        }
    }

    IEnumerator ApplyDamage()
    {
        while (playerInside)
        {
            if (playerController != null)
            {
                playerController.TakeDamge((int)damagePerSecond);
            }
            yield return new WaitForSeconds(1f); // Wait 1 second before applying damage again
        }
    }
}
