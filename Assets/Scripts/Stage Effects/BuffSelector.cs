using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemboxSelectors : MonoBehaviour
{
    public GameObject itemUIPanel;         // UI Panel to display item options
    public TMP_Text itemListText;          // Text to display item choices
    public float interactionDistance = 3f; // Distance to interact
    private bool isPlayerNearby = false;
    private bool isBoxOpen = false;

    private PlayerController player;

    void Update()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.GetComponent<PlayerController>();
            }
        }

        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);
        isPlayerNearby = distance <= interactionDistance;

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            OpenItemBox();
        }

        if (isBoxOpen)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) { ApplyBuff(0); }
            else if (Input.GetKeyDown(KeyCode.Alpha2)) { ApplyBuff(1); }
            else if (Input.GetKeyDown(KeyCode.Alpha3)) { ApplyBuff(2); }
        }
    }

    private void OpenItemBox()
    {
        if (!isBoxOpen)
        {
            isBoxOpen = true;
            ShowItemUI();
            Debug.Log("Buff Selector Opened! Press 1, 2, or 3 to choose.");
        }
    }

    private void ApplyBuff(int index)
    {
        switch (index)
        {
            case 0:
                player.Heal(50);
                Debug.Log("Applied Health Buff: +50 HP");
                break;
            case 1:
                player.ChangeSpeed(3);
                Debug.Log("Applied Speed Buff: +3 Speed");
                break;
            case 2:
                player.ChangeJump(5);
                Debug.Log("Applied Jump Buff: +5 Jump");
                break;
            default:
                Debug.Log("Invalid Buff Index");
                return;
        }

        HideItemUI();
        Destroy(gameObject);
    }

    private void ShowItemUI()
    {
        if (itemUIPanel != null && itemListText != null)
        {
            itemUIPanel.SetActive(true);
            itemListText.text = "Select a Buff:\n";
            itemListText.text += "1. Health Buff (+50 HP)\n";
            itemListText.text += "2. Speed Buff (+3 Speed)\n";
            itemListText.text += "3. Jump Buff (+5 Jump)";
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
