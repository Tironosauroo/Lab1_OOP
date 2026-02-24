using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;
using System.Collections;
using System.Reflection;

/// <summary>
/// PlayMode tests for the PlayerMovement component.
/// </summary>
public class PlayerMovementTest
{
    private GameObject player;
    private PlayerMovement movement;
    private CharacterController controller;
    private GameObject cameraObj;
    private GameObject inputManagerObj;

    /// <summary>
    /// Sets up the test environment before each test runs.
    /// Creates a clean InputManager and Player object.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        // 1. FORCE CLEAN THE SINGLETON. 
        // Prevents ghost references between test runs which cause NullReferenceExceptions.
        typeof(InputManager).GetProperty("Instance", BindingFlags.Public | BindingFlags.Static)?.SetValue(null, null);

        // 2. Create InputManager for the test scene
        inputManagerObj = new GameObject("InputManager");
        inputManagerObj.AddComponent<InputManager>();

        // 3. Create the player object
        player = new GameObject("Player");
        controller = player.AddComponent<CharacterController>();

        // 4. Create and tag the camera (prevents Camera.main null errors if PlayerMovement uses it)
        cameraObj = new GameObject("Camera");
        cameraObj.tag = "MainCamera";
        cameraObj.AddComponent<Camera>();
        cameraObj.transform.SetParent(player.transform);

        // 5. Add PlayerMovement (Awake() is automatically called by Unity right here!)
        movement = player.AddComponent<PlayerMovement>();
        movement.cameraTransform = cameraObj.transform;

        // Call Start manually. Removed SendMessage("Awake") and ("OnEnable") to prevent double execution.
        player.SendMessage("Start");
    }

    /// <summary>
    /// Cleans up the scene after each test finishes to avoid memory leaks.
    /// </summary>
    [TearDown]
    public void Teardown()
    {
        // Clean up the game objects
        Object.DestroyImmediate(player);
        if (inputManagerObj != null)
        {
            Object.DestroyImmediate(inputManagerObj);
        }

        // Clean up the singleton instance again just to be safe
        typeof(InputManager).GetProperty("Instance", BindingFlags.Public | BindingFlags.Static)?.SetValue(null, null);
    }

    /// <summary>
    /// Tests if the player moves forward when forward input is simulated.
    /// </summary>
    [UnityTest]
    public IEnumerator Player_Moves_Forward_When_Input_Given()
    {
        // Arrange
        Vector3 startPos = player.transform.position;

        // Simulate forward input using Reflection to access private fields
        movement.GetType().GetField("moveInput", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.SetValue(movement, new Vector2(0, 1));

        // Act
        for (int i = 0; i < 10; i++)
        {
            player.SendMessage("Update");
            yield return null;
        }

        // Assert
        Assert.Greater(player.transform.position.z, startPos.z, "Player should move forward on the Z axis.");
    }

    /// <summary>
    /// Tests if the player's camera drops down when crouching and returns to normal when standing up.
    /// </summary>
    [UnityTest]
    public IEnumerator Player_Crouches_And_Returns_To_Normal()
    {
        // Arrange
        float normalY = movement.cameraTransform.localPosition.y;

        // Act - Start Crouch
        movement.GetType().GetMethod("StartCrouch", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.Invoke(movement, null);

        yield return null;
        float crouchedY = movement.cameraTransform.localPosition.y;

        // Assert 1 - Check if camera is lowered
        Assert.Less(crouchedY, normalY, "Camera local Y position should be lower when crouched.");

        // Act - Stop Crouch
        movement.GetType().GetMethod("StopCrouch", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.Invoke(movement, null);

        yield return null;
        float resetY = movement.cameraTransform.localPosition.y;

        // Assert 2 - Check if camera returned to original height
        Assert.AreEqual(normalY, resetY, 0.01f, "Camera local Y position should return to normal height.");
    }
}