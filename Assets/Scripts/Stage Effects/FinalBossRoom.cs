using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossRoom : MonoBehaviour
{
    [System.Serializable]
    public class ThresholdAction
    {
        [Range(0, 100)] public float healthPercent;
        public GameObject targetObjectToDisable;
        public GameObject objectWithScriptToEnable;
        public string scriptToEnableName; // Input the script name as string
        [HideInInspector] public bool triggered = false;
    }

    public JuggernautBehaviour pudgeEnemy;
    public List<ThresholdAction> actions;

    private float maxHealth;

    void Start()
    {
        if (pudgeEnemy == null)
        {
            Debug.LogError("Pudge enemy not assigned!");
            enabled = false;
            return;
        }

        maxHealth = pudgeEnemy.MaxHealth;
    }

    void Update()
    {
        float currentHealth = pudgeEnemy.CurrentHealth;
        float healthPercent = (currentHealth / maxHealth) * 100f;

        foreach (var action in actions)
        {
            if (!action.triggered && healthPercent <= action.healthPercent)
            {
                TriggerThreshold(action);
                action.triggered = true;
            }
        }
    }

    void TriggerThreshold(ThresholdAction action)
    {
        // Disable visuals and collider
        if (action.targetObjectToDisable != null)
        {
            MeshRenderer meshRenderer = action.targetObjectToDisable.GetComponent<MeshRenderer>();
            if (meshRenderer) meshRenderer.enabled = false;

            MeshCollider meshCollider = action.targetObjectToDisable.GetComponent<MeshCollider>();
            if (meshCollider) meshCollider.enabled = false;

            action.targetObjectToDisable.SetActive(false); // Fully disable the object
        }

        // Move object downward by 15 units and enable script
        if (action.objectWithScriptToEnable != null)
        {
            Vector3 position = action.objectWithScriptToEnable.transform.position;
            position.y -= 15f;
            action.objectWithScriptToEnable.transform.position = position;

            // Enable the specified script
            var script = action.objectWithScriptToEnable.GetComponent(action.scriptToEnableName) as MonoBehaviour;
            if (script != null)
            {
                script.enabled = true;
                Debug.Log($"Enabled script: {action.scriptToEnableName} on {action.objectWithScriptToEnable.name}");
            }
            else
            {
                Debug.LogWarning($"Script '{action.scriptToEnableName}' not found on {action.objectWithScriptToEnable.name}");
            }
        }

        Debug.Log($"Triggered threshold at {action.healthPercent}% HP.");
    }
}
