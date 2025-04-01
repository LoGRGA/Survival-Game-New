using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuff : BuffDebuff
{
    public int damageIncrease = 10; // Damage increase for the buff
    private PlayerController playerController;

    void Awake()
    {
        effectName = "Damage"; // Set the effect name
    }

    // Logic to apply the effect (increase player's damage)
    protected override void ApplyEffectLogic()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.attackDamage += damageIncrease;  // Assuming attackDamage is publicly accessible
        }
    }

    // Logic to remove the effect (restore original damage)
    protected override void RemoveEffectLogic()
    {
        if (playerController != null)
        {
            playerController.attackDamage -= damageIncrease;  // Restore original attack damage
        }
    }
}



