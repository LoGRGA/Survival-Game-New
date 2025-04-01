using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour
{
    public float baseSpeed = 6.0f;  // Default movement speed
    public float speed;  // Actual speed that can be modified
    public float baseJumpSpeed = 15.0f;  // Default jump height
    public float jumpSpeed;  // Actual jump speed that can be modified
    public float gravity = -9.8f;  // Gravity setting
    public float terminalVelocity = -20f;  // Max falling speed

    private float vertSpeed;
    protected Vector3 movement;
    protected CharacterController charController;
    protected Animator[] animator;
    protected AudioSource audioSource;

    protected virtual void Start()
    {
        charController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Set initial speed and jump speed based on base values
        speed = baseSpeed;
        jumpSpeed = baseJumpSpeed;
    }

    protected virtual void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);

        if (Input.GetButtonDown("Jump") && charController.isGrounded)
        {
            vertSpeed = jumpSpeed;
        }
        else if (!charController.isGrounded)
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }
        }

        movement.y = vertSpeed;
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);
        charController.Move(movement);
    }

    // ?? Apply speed buffs and debuffs
    public void ChangeSpeed(float amount)
    {
        speed += amount;
        speed = Mathf.Clamp(speed, 3f, 15f); // Set a reasonable limit
        Debug.Log("Speed changed to: " + speed);
    }

    // ?? Apply jump buffs and debuffs
    public void ChangeJump(float amount)
    {
        jumpSpeed += amount;
        jumpSpeed = Mathf.Clamp(jumpSpeed, 5f, 30f); // Set a reasonable limit
        Debug.Log("Jump height changed to: " + jumpSpeed);
    }
}
