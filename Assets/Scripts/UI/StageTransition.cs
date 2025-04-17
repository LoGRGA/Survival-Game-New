using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageTransition : MonoBehaviour
{
    public GameObject stageToHide;       // Text to hide (e.g., Stage 1)
    public GameObject stageToShow;       // Text to show (e.g., Stage 2)
    public float interactionDistance = 3f; // Distance for interaction
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (stageToHide != null) stageToHide.SetActive(false);
                if (stageToShow != null) stageToShow.SetActive(true);
            }
        }
    }
}

