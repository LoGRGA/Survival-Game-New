using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCube : MonoBehaviour
{
    public BuffDebuff buffEffect; // Reference to the specific buff or debuff

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buffEffect.Activate(); // Activate the buff when the player touches the cube
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buffEffect.Deactivate(); // Deactivate the buff when the player leaves the cube
        }
    }
}



