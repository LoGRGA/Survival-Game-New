using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : BuffDebuff
{
    public float speedIncrease = 2f; // Speed increase for the buff
    private PlayerController playerController;

    void Awake()
    {
        effectName = "Speed"; // Set the effect name
    }

    // Logic to apply the effect when the buff is triggered
    protected override void ApplyEffectLogic()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            // Increase the player's speed (access from PlayerController)
            playerController.speed += speedIncrease;
        }
    }

    // Logic to remove the effect when the buff expires or is removed
    protected override void RemoveEffectLogic()
    {
        if (playerController != null)
        {
            // Revert the player's speed to the original value
            playerController.speed -= speedIncrease;
        }
    }
}




