using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlownessBuff : BuffDebuff
{
    public float slowDownAmount = 1.5f;  // Slowness effect multiplier
    private PlayerController playerController;

    void Awake()
    {
        effectName = "SlownessBuff";  // Set the effect name
    }

    // Logic to apply the effect (reduce player speed)
    protected override void ApplyEffectLogic()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.speed *= slowDownAmount; // Decrease the player's speed
        }
    }

    // Logic to remove the effect (restore original speed)
    protected override void RemoveEffectLogic()
    {
        if (playerController != null)
        {
            playerController.speed /= slowDownAmount;  // Restore the original speed
        }
    }
}




