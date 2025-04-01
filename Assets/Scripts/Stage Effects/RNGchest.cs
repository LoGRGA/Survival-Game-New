using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNGchest : MonoBehaviour
{
    public GameObject[] goodItems; // Health potion, speed boost, etc.
    public GameObject[] badEffects; // Debuffs like slow, damage
    public GameObject keyPrefab;

    public float baseKeyChance = 5f; // Starts at 5%
    public float keyChanceIncrease = 2f; // Increases each time a chest is opened

    private static float currentKeyChance = 5f;

    private bool isPlayerInRange = false;

    private void Update()
    {
        // Check if player is in range and presses 'F' to open the chest
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            OpenChest();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Press F to open the chest.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    public void OpenChest()
    {
        float roll = Random.Range(0f, 100f);

        if (roll < currentKeyChance)
        {
            Instantiate(keyPrefab, transform.position, Quaternion.identity);
            Debug.Log("Key obtained!");
            currentKeyChance = baseKeyChance; // Reset chance after key is found
        }
        else if (roll < 50f) // 50% chance for a good item
        {
            Instantiate(goodItems[Random.Range(0, goodItems.Length)], transform.position, Quaternion.identity);
            Debug.Log("You got a good item!");
        }
        else // Remaining chance is for a bad effect
        {
            Instantiate(badEffects[Random.Range(0, badEffects.Length)], transform.position, Quaternion.identity);
            Debug.Log("Bad effect applied!");
        }

        currentKeyChance += keyChanceIncrease; // Increase key drop chance
        Destroy(gameObject); // Remove the chest after opening
    }
}