using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Inventory display component in the user interface (HUD).
/// </summary>
public class InventoryUI : MonoBehaviour
{
    /// <summary>UI image for the active item.</summary>
    [Header("HUD Slots")]
    [SerializeField] private Image mainSlotImage;

    /// <summary>UI image for the next item in the queue.</summary>
    [SerializeField] private Image subSlotImage;

    /// <summary>Link to inventory manager for event subscriptions.</summary>
    [SerializeField] private InventoryManager inventoryManager;

    /// <summary>Subscribes to inventory events when the script is activated.</summary>
    private void OnEnable()
    {
        if (inventoryManager != null)
            inventoryManager.OnInventoryChanged += UpdateHUD;
    }

    /// <summary>Unsubscribes from inventory events when the script is deactivated.</summary>
    private void OnDisable()
    {
        if (inventoryManager != null)
            inventoryManager.OnInventoryChanged -= UpdateHUD;
    }

    /// <summary>
    /// Updates the sprites on the screen based on the contents of the queue.
    /// </summary>
    /// <param name="items">The current array of items from the queue.</param>
    private void UpdateHUD(InventoryItem[] items)
    {
       // UpdateHand remains unchanged
        if (items.Length > 0)
        {
            mainSlotImage.sprite = items[0].itemSprite;
            mainSlotImage.enabled = true;
        }
        else
        {
            mainSlotImage.sprite = null;
            mainSlotImage.enabled = false;
        }

        if (items.Length > 1)
        {
            subSlotImage.sprite = items[1].itemSprite;
            subSlotImage.enabled = true;
        }
        else
        {
            subSlotImage.sprite = null;
            subSlotImage.enabled = false;
        }
    }
}