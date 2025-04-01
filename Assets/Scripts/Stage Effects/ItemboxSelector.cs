using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ItemboxSelector : MonoBehaviour
{
    public GameObject[] itemPrefabs;                // Array to hold item prefabs
    public GameObject itemUIPanel;                 // UI Panel to display item options
    public TMP_Text itemListText;   // Use TextMeshPro for item list
    public float interactionDistance = 3f;         // Distance to interact with the item box
    private bool isPlayerNearby = false;
    private bool isBoxOpen = false;

    private void Update()
    {
        // Check player proximity
        if (Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position) <= interactionDistance)
        {
            isPlayerNearby = true;

            if (Input.GetKeyDown(KeyCode.F))
            {
                OpenItemBox();
            }
        }
        else
        {
            isPlayerNearby = false;
        }

        // Handle item selection when the box is open
        if (isBoxOpen)
        {
            for (int i = 0; i < itemPrefabs.Length; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    SelectItem(i);
                }
            }
        }
    }

    private void OpenItemBox()
    {
        if (!isBoxOpen)
        {
            isBoxOpen = true;
            ShowItemUI();
            Debug.Log("Item Box Opened! Choose an item by pressing 1, 2, 3, or 4.");
        }
    }

    private void SelectItem(int index)
    {
        if (index >= 0 && index < itemPrefabs.Length)
        {
            // Spawn the selected item
            Instantiate(itemPrefabs[index], transform.position + Vector3.up * 1f, Quaternion.identity);
            Debug.Log($"Spawned Item: {itemPrefabs[index].name}");

            // Hide the UI and destroy the item box
            HideItemUI();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Invalid selection.");
        }
    }

    private void ShowItemUI()
    {
        if (itemUIPanel != null && itemListText != null)
        {
            itemUIPanel.SetActive(true);
            itemListText.text = "Select an item:\n";

            for (int i = 0; i < itemPrefabs.Length; i++)
            {
                itemListText.text += $"{i + 1}. {itemPrefabs[i].name}\n";
            }
        }
    }

    private void HideItemUI()
    {
        if (itemUIPanel != null)
        {
            itemUIPanel.SetActive(false);
        }
    }
}