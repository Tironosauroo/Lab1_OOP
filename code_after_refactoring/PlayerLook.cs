using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles the player's camera rotation (mouse look) based on input.
/// </summary>
public class PlayerLook : MonoBehaviour
{
    /// <summary>Sensitivity multiplier for mouse movement.</summary>
    public float mouseSensitivity = 100f;

    /// <summary>Reference to the camera's transform to apply vertical rotation.</summary>
    public Transform cameraTransform;

    /// <summary>Accumulated rotation around the X-axis (vertical look).</summary>
    private float xRotation = 0f;

    /// <summary>Current input vector from the mouse/controller.</summary>
    private Vector2 lookInput;

    /// <summary>
    /// Locks the cursor to the center of the screen and subscribes to look input events 
    /// from the centralized InputManager safely after initialization.
    /// </summary>
    void Start()
    {
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;

        // Subscribe to input events safely
        InputManager.Instance.Controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        InputManager.Instance.Controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    /// <summary>
    /// Applies rotation to the camera and player body based on input.
    /// Executed in LateUpdate to ensure camera follows player physics smoothly.
    /// </summary>
    void LateUpdate()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        // Clamp vertical rotation to prevent the camera from flipping over
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}