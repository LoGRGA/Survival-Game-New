using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUIHandler : MonoBehaviour
{
    public GameObject speedBuffIcon;
    public GameObject jumpBuffIcon;

    private PlayerController playerController;
    private FPSInput FPSInput;

    public float baseSpeed = 6.0f;
    public float threshold = 0.0f;

    public float baseJumpForce = 15.0f;
    public float jumpThreshold = 0.0f;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (speedBuffIcon != null)
            speedBuffIcon.SetActive(false);

        if (jumpBuffIcon != null)
            jumpBuffIcon.SetActive(false);
    }

    void Update()
    {
        if (playerController != null)
        {
            bool speedBuffActive = FPSInput.speed > baseSpeed + threshold;
            bool jumpBuffActive = FPSInput.jumpSpeed > playerController.baseJumpSpeed + jumpThreshold;

            if (speedBuffIcon != null)
                speedBuffIcon.SetActive(speedBuffActive);

            if (jumpBuffIcon != null)
                jumpBuffIcon.SetActive(jumpBuffActive);
        }
    }
}


