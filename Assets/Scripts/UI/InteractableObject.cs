using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.F;
    public float interactDistance = 3f;

    [Header("Events")]
    public UnityEvent onInteract;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactDistance && Input.GetKeyDown(interactKey))
        {
            onInteract?.Invoke();  // Trigger any events assigned in the inspector
        }
    }

    // Optional: You can call this manually too
    public void TriggerInteraction()
    {
        onInteract?.Invoke();
    }
}

