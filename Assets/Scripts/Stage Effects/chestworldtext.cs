using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chestworldtext : MonoBehaviour
{
    public Canvas chestCanvas;
    public float showDistance = 5f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (chestCanvas != null)
            chestCanvas.enabled = false; // hide by default
    }

    void Update()
    {
        if (player == null || chestCanvas == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        chestCanvas.enabled = distance <= showDistance;

        // Optional: Always face the player
        chestCanvas.transform.LookAt(player);
        chestCanvas.transform.Rotate(0, 180, 0); // Flip to face player properly
    }
}