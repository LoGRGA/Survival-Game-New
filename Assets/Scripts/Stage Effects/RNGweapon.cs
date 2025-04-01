using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNGweapon : MonoBehaviour
{
    public GameObject[] weaponPrefabs; // Array of weapon prefabs to choose from
    private bool isPlayerInRange = false;

    private void Update()
    {
        // Check if player is in range and presses 'F' to open the box
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            GiveRandomWeapon();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Press F to open the weapon box.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    public void GiveRandomWeapon()
    {
        if (weaponPrefabs.Length == 0)
        {
            Debug.LogWarning("No weapons assigned to the weapon box!");
            return;
        }

        // Pick a random weapon
        int randomIndex = Random.Range(0, weaponPrefabs.Length);
        GameObject weapon = Instantiate(weaponPrefabs[randomIndex], transform.position, Quaternion.identity);

        Debug.Log($"You received: {weapon.name}");

        // Destroy the box after giving the weapon
        Destroy(gameObject);
    }
}