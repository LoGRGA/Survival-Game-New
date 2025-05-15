using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUIHandler : MonoBehaviour
{
    public GameObject speedBuffIcon;
    public GameObject jumpBuffIcon;

    private PlayerController playerController;
    private FPSInput fpsInput;

    public float baseSpeed = 6f;
    public float threshold = 0.1f;

    public float baseJumpForce = 15f;
    public float jumpThreshold = 0.1f;

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
            bool speedBuffActive = playerController.speed > baseSpeed + threshold;
            bool jumpBuffActive = fpsInput.jumpSpeed > fpsInput.baseJumpSpeed + jumpThreshold;

            if (speedBuffIcon != null)
                speedBuffIcon.SetActive(speedBuffActive);

            if (jumpBuffIcon != null)
                jumpBuffIcon.SetActive(jumpBuffActive);
        }
    }
}


