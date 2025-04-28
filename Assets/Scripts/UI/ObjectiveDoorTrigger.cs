using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ObjectiveDoorTrigger : MonoBehaviour
{
    public GameObject player; // Drag the player object here
    public float interactionRange = 3f; // Distance to interact
    public GameObject objective1UI; // Drag your Objective 1 UI
    public GameObject objective2UI; // Drag your Objective 2 UI
    public float delayBeforeSwitch = 3f; // Delay before switching UI

    private bool playerInRange = false;

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        playerInRange = distance <= interactionRange;

        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(SwitchObjectives());
        }
    }

    private IEnumerator SwitchObjectives()
    {
        yield return new WaitForSeconds(delayBeforeSwitch);

        if (objective1UI != null)
            objective1UI.SetActive(false);

        if (objective2UI != null)
            objective2UI.SetActive(true);

        Debug.Log("Objectives switched after interacting with the door!");
    }
}

