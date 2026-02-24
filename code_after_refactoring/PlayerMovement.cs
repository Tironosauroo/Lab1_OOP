using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player character movement, including walking, jumping, crouching, and gravity.
/// Uses the Unity CharacterController and the centralized InputManager.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 6f;
    public float crouchSpeed = 3f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float crouchHeight = 0.8f;

    /// <summary>Original Y position of the camera.</summary>
    private float normalCameraY;

    /// <summary>Flag indicating if the player is currently crouching.</summary>
    private bool isCrouching = false;

    private CharacterController controller;

    /// <summary>Current velocity vector, primarily used for gravity calculations.</summary>
    private Vector3 velocity;

    /// <summary>Current movement input vector (WASD/Left Stick).</summary>
    private Vector2 moveInput;

    /// <summary>
    /// Retrieves necessary components.
    /// Internal initializations should be done here to avoid race conditions.
    /// </summary>
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Initializes input subscriptions via the centralized InputManager and stores initial states.
    /// Subscriptions are placed here to ensure InputManager is fully loaded.
    /// </summary>
    void Start()
    {
        normalCameraY = cameraTransform.localPosition.y;

        // Subscribe to events via our new InputManager safely in Start()
        InputManager.Instance.Controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        InputManager.Instance.Controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        InputManager.Instance.Controls.Player.Jump.performed += ctx => Jump();
        InputManager.Instance.Controls.Player.Crouch.performed += ctx => StartCrouch();
        InputManager.Instance.Controls.Player.Crouch.canceled += ctx => StopCrouch();
    }

    /// <summary>
    /// Calculates and applies horizontal movement and vertical gravity every frame.
    /// </summary>
    void Update()
    {
        // Horizontal movement
        float currentSpeed = isCrouching ? crouchSpeed : speed;
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Gravity implementation
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f; // Small downward force to keep player grounded

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// Applies an upward velocity to the player if they are currently grounded.
    /// </summary>
    void Jump()
    {
        if (controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    /// <summary>
    /// Lowers the camera position to simulate crouching and reduces movement speed.
    /// </summary>
    void StartCrouch()
    {
        isCrouching = true;
        Vector3 pos = cameraTransform.localPosition;
        pos.y = normalCameraY - crouchHeight;
        cameraTransform.localPosition = pos;
    }

    /// <summary>
    /// Restores the camera position to simulate standing up and restores normal speed.
    /// </summary>
    void StopCrouch()
    {
        isCrouching = false;
        Vector3 pos = cameraTransform.localPosition;
        pos.y = normalCameraY;
        cameraTransform.localPosition = pos;
    }
}