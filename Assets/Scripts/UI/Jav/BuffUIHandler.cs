using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUIHandler : MonoBehaviour
{
    public GameObject speedBuffIcon;
    private PlayerController playerController;
    public float baseSpeed = 5f; //default speed is
    public float threshold = 0.1f;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (speedBuffIcon != null)
            speedBuffIcon.SetActive(false);
    }

    void Update()
    {
        if (playerController != null)
        {
            bool speedBuffActive = playerController.speed > baseSpeed + threshold;

            if (speedBuffIcon != null)
                speedBuffIcon.SetActive(speedBuffActive);
        }
    }
}

