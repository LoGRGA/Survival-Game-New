using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : BuffDebuff
{
    public float damagePerSecond = 5f; // Health lost per second
    private PlayerController playerController;
    private float poisonTimer;

    void Awake()
    {
        effectName = "Poison"; // Set the effect name
    }

    // Logic to apply the effect (start the poison damage over time)
    protected override void ApplyEffectLogic()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            poisonTimer = 0f; // Reset the timer when poison is applied
        }
    }

    // Logic to remove the effect (stop poison damage)
    protected override void RemoveEffectLogic()
    {
        // No action needed here since damage stops when the debuff is removed
    }

    void Update()
    {
        if (isActive && playerController != null)
        {
            poisonTimer += Time.deltaTime;
            if (poisonTimer >= 1f) // Apply damage once every second
            {
                playerController.TakeDamge((int)damagePerSecond);
                poisonTimer = 0f;
            }
        }
    }
}




