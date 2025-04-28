using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKillTracker : MonoBehaviour
{
    public GameObject tickImage; // Assign your green tick UI Image here
    private bool isDead = false;

    private void Update()
    {
        // Optional: If the boss has "health", you could monitor it here
        if (!isDead && CheckBossDeath())
        {
            HandleBossDeath();
        }
    }

    private bool CheckBossDeath()
    {
        // You can replace this with your own boss health system
        // For now, we assume the boss is "dead" if this GameObject is deactivated or destroyed.
        // (You can replace this with a health check if needed.)

        // Example placeholder: 
        return false;
    }

    public void HandleBossDeath()
    {
        if (tickImage != null)
        {
            tickImage.SetActive(true);
        }
        Debug.Log("Boss defeated! Tick image activated.");
        isDead = true;
    }
}

