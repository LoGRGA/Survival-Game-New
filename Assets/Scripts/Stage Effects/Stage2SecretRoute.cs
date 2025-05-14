using System.Collections;
using UnityEngine;

public class KeyUnlockTrigger : MonoBehaviour
{
    public GameObject objectToEnable;               // Assign in Inspector
    public InventoryController inventory;           // Auto-find or assign manually

    private bool isPlayerInRange = false;

    private void Start()
    {
        if (inventory == null)
        {
            inventory = FindObjectOfType<InventoryController>();
        }

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(false); // Ensure the second object is hidden at start
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (HasKeyItem())
            {
                if (objectToEnable != null)
                {
                    objectToEnable.SetActive(true);   // Enable the replacement
                }

                gameObject.SetActive(false);          // Disable this object
                Debug.Log("Object swapped using key!");
            }
            else
            {
                Debug.Log("You need the key item to activate this.");
            }
        }
    }

    private bool HasKeyItem()
    {
        if (inventory == null || inventory.slots.Length < 9)
            return false;

        Transform slot = inventory.slots[8].transform; // Slot 9 (index 8)

        if (slot.childCount > 0)
        {
            Spawn spawnScript = slot.GetComponentInChildren<Spawn>();
            return spawnScript != null && spawnScript.itemName == "KeyItem";
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Press F to unlock.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
