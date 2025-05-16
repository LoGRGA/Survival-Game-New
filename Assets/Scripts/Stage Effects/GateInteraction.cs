using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateInteraction : MonoBehaviour
{
    public float riseAmount = 5f;
    public float moveSpeed = 2f;
    private bool isPlayerInRange = false;
    private bool isRaised = false;
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    public InventoryController inventory;

    [Header("Optional - Gimmick Script to Disable")]
    public FinalBossRoom bossRoomScript; // Assign this in the Inspector

    private void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition;

        inventory = FindObjectOfType<InventoryController>();

        // If key is already held, disable boss gimmick script
        if (HasKeyItem() && bossRoomScript != null)
        {
            bossRoomScript.enabled = false;
            Debug.Log("Player has key. FinalBossRoom script disabled at Start.");
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (HasKeyItem())
            {
                TogglePosition();

                if (bossRoomScript != null && bossRoomScript.enabled)
                {
                    //bossRoomScript.enabled = false;
                    //Debug.Log("FinalBossRoom script disabled after opening gate with key.");
                }
            }
            else
            {
                Debug.Log("Gate is locked. You need a key.");
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Press F to raise/lower the object.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void TogglePosition()
    {
        if (isRaised)
        {
            targetPosition = initialPosition;
            Debug.Log("Object lowering...");
        }
        else
        {
            targetPosition = initialPosition + Vector3.up * riseAmount;
            Debug.Log("Object rising...");
        }

        isRaised = !isRaised;
    }

    private bool HasKeyItem()
    {
        if (inventory == null || inventory.slots.Length < 11)
            return false;

        Transform slot = inventory.slots[8].transform;

        if (slot.childCount > 0)
        {
            Spawn spawnScript = slot.GetComponentInChildren<Spawn>();
            if (spawnScript != null && spawnScript.itemName == "KeyItem")
            {
                return true;
            }
        }

        return false;
    }
}
