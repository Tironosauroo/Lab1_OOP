using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;

/// <summary>
/// Component responsible for player interaction with items in the scene.
/// Uses a centralized InputManager for input handling.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    /// <summary>UI element (hint/cursor) that appears when the player looks at an interactable item.</summary>
    [Header("HUD for Pickable Items")]
    [SerializeField] private GameObject hud;

    /// <summary>The current item within interaction range (trigger).</summary>
    private GameObject currentPickable;

    /// <summary>Reference to the player's inventory manager.</summary>
    private InventoryManager inventory;

    /// <summary>
    /// Initializes references and subscribes to input events via the Singleton InputManager.
    /// Changed to Start to avoid NullReferenceException (Race Condition).
    /// </summary>
    private void Start() // ВИПРАВЛЕНО: Awake замінено на Start
    {
        // Subscribe to events via our new InputManager
        InputManager.Instance.Controls.Player.Interact.started += ctx => Interact();

        inventory = GetComponent<InventoryManager>();
        InputManager.Instance.Controls.Player.NextItemQueue.started += ctx => inventory?.NextItem();
    }

    /// <summary>
    /// Disables the interaction HUD hint when the object is enabled.
    /// </summary>
    private void OnEnable()
    {
        if (hud != null)
            hud.SetActive(false);
    }

    /// <summary>
    /// Detects when a pickable item enters the player's interaction zone.
    /// </summary>
    /// <param name="other">The collider of the object that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            currentPickable = other.gameObject;
            if (hud != null)
                hud.SetActive(true);
        }
    }

    /// <summary>
    /// Detects when a pickable item exits the player's interaction zone.
    /// </summary>
    /// <param name="other">The collider of the object that exited the trigger.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable") && other.gameObject == currentPickable)
        {
            currentPickable = null;
            if (hud != null)
                hud.SetActive(false);
        }
    }

    /// <summary>
    /// Item pickup logic: creates a clone, sends it to the inventory, and disables the original in the scene.
    /// </summary>
    private void Interact()
    {
        // Починаємо вимірювання
        Profiler.BeginSample("My_Interact_Performance");

        if (currentPickable != null)
        {
            Sprite itemSprite = currentPickable.GetComponent<PickableItem>()?.itemSprite;

            if (inventory != null && itemSprite != null)
            {
                // Create a clone of the active item
                GameObject clone = Instantiate(currentPickable);
                clone.SetActive(true);
                clone.transform.SetParent(null); // Detach from scene hierarchy
                clone.transform.position = Vector3.zero;
                clone.transform.rotation = Quaternion.identity;
                clone.tag = "Untagged"; // Remove tag to prevent infinite picking

                // Add the copy to the queue via the new InventoryManager
                inventory.AddItem(new InventoryItem(currentPickable.name, itemSprite, clone));
            }

            currentPickable.SetActive(false);
            currentPickable = null;

            if (hud != null)
                hud.SetActive(false);
        }

        // ВИПРАВЛЕНО: EndSample має бути в самому кінці методу, поза всіма IF-ами!
        Profiler.EndSample();
    }
}
