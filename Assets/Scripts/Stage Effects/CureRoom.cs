using System.Collections.Generic;
using UnityEngine;

public class CureRoom : MonoBehaviour
{
    [System.Serializable]
    public class InteractionAction
    {
        public GameObject objectToDisable;
        public List<GameObject> objectsWithScriptToEnable; // Now a list
        public string scriptToEnableName; // The script name to enable
    }

    public List<InteractionAction> actions;
    public float interactionDistance = 3f;

    private GameObject player;
    private bool isPlayerInRange = false;
    private bool interactionDone = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player == null) return;

        isPlayerInRange = Vector3.Distance(transform.position, player.transform.position) <= interactionDistance;

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F) && !interactionDone)
        {
            PerformInteraction();
            interactionDone = true;
        }
    }

    void PerformInteraction()
    {
        foreach (var action in actions)
        {
            // Disable target object
            if (action.objectToDisable != null)
            {
                action.objectToDisable.SetActive(false);
                Debug.Log("Disabled: " + action.objectToDisable.name);
            }

            // Enable scripts on multiple objects
            foreach (GameObject target in action.objectsWithScriptToEnable)
            {
                if (target != null && !string.IsNullOrEmpty(action.scriptToEnableName))
                {
                    var script = target.GetComponent(action.scriptToEnableName) as MonoBehaviour;
                    if (script != null)
                    {
                        script.enabled = true;
                        Debug.Log($"Enabled script: {action.scriptToEnableName} on {target.name}");
                    }
                    else
                    {
                        Debug.LogWarning($"Script '{action.scriptToEnableName}' not found on {target.name}");
                    }
                }
            }
        }

        Debug.Log("Room interaction completed.");
    }
}
