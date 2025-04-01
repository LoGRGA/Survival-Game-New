using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTrigger : MonoBehaviour
{
    public BuffDebuff buffEffect; // Reference to the BuffDebuff attached to the cube

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Activate the buff or debuff when the player enters the trigger
            buffEffect.Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Deactivate the buff or debuff when the player exits the trigger
            buffEffect.Deactivate();
        }
    }
}


