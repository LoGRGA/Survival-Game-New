using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffChest : MonoBehaviour
{
    public GameObject keyPrefab; // Key for progression
    public GameObject interactionBorder;

    public float baseKeyChance = 5f; // Starts at 5%
    public float keyChanceIncrease = 2f; // Increases each time a chest is opened
    private static float currentKeyChance = 5f;

    private bool isPlayerInRange = false;
    private PlayerController playerController;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            interactionBorder.SetActive(true);
            OpenChest();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerController = other.GetComponent<PlayerController>();
            Debug.Log("Press F to open the chest.");
            interactionBorder.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerController = null;
            interactionBorder.SetActive(false);
        }
    }

    public void OpenChest()
    {
        if (playerController == null) return;

        float roll = Random.Range(0f, 100f);

        if (roll < currentKeyChance) // Key reward
        {
            Instantiate(keyPrefab, transform.position, Quaternion.identity);
            Debug.Log("Key obtained!");
            currentKeyChance = baseKeyChance; // Reset key chance after key is found
        }
        else if (roll < 50f) // 50% chance for a good effect
        {
            ApplyGoodEffect();
        }
        else // Remaining chance for a bad effect
        {
            ApplyBadEffect();
        }

        currentKeyChance += keyChanceIncrease; // Increase key drop chance
        Destroy(gameObject); // Remove the chest after opening
        interactionBorder.SetActive(false);
    }

    private void ApplyGoodEffect()
    {
        int effectType = Random.Range(0, 3); // 0 = health, 1 = speed, 2 = jump
        switch (effectType)
        {
            case 0:
                playerController.Heal(10);
                Debug.Log("Gained +10 Health!");
                break;
            case 1:
                playerController.ChangeSpeed(3);
                Debug.Log("Gained +3 Speed!");
                break;
            case 2:
                playerController.ChangeJump(10);
                Debug.Log("Gained +10 Jump!");
                break;
        }
    }

    private void ApplyBadEffect()
    {
        int effectType = Random.Range(0, 3); // 0 = health, 1 = speed, 2 = jump
        switch (effectType)
        {
            case 0:
                playerController.TakeDamge(10);
                Debug.Log("Lost -10 Health!");
                break;
            case 1:
                playerController.ChangeSpeed(-3);
                Debug.Log("Lost -3 Speed!");
                break;
            case 2:
                playerController.ChangeJump(-3);
                Debug.Log("Lost -3 Jump!");
                break;
        }
    }
}