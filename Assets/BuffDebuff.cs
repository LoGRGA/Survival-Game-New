using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BuffDebuff : MonoBehaviour
{
    public string effectName; // The name of the effect (e.g., "Speed", "Poison")
    public bool isActive = false; // Flag to check if the effect is active

    // Method to apply the effect
    protected abstract void ApplyEffectLogic();

    // Method to remove the effect
    protected abstract void RemoveEffectLogic();

    // Call this method to activate the buff or debuff
    public void Activate()
    {
        if (!isActive)
        {
            isActive = true;
            ApplyEffectLogic(); // Apply the effect logic
        }
    }

    // Call this method to deactivate the buff or debuff
    public void Deactivate()
    {
        if (isActive)
        {
            isActive = false;
            RemoveEffectLogic(); // Remove the effect logic
        }
    }
}

