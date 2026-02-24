using UnityEngine;

/// <summary>
/// A component for visualizing the current item in the hand of the 3D player model.
/// </summary>
public class PlayerEquipment : MonoBehaviour
{
    /// <summary>The anchor point (Transform) in the player model's hand.</summary>
    [Header("Hand Object")]
    [SerializeField] private Transform hand;

    /// <summary>Link to inventory manager for event subscriptions.</summary>
    [SerializeField] private InventoryManager inventoryManager;

    /// <summary>A reference to a 3D object of the item created in the world.</summary>
    private GameObject currentItemInHand;

    /// <summary>Subscribes to inventory events when the script is activated.</summary>
    private void OnEnable()
    {
        if (inventoryManager != null)
            inventoryManager.OnInventoryChanged += UpdateHand;
    }

    /// <summary>Unsubscribes from inventory events when the script is deactivated.</summary>
    private void OnDisable()
    {
        if (inventoryManager != null)
            inventoryManager.OnInventoryChanged -= UpdateHand;
    }

    /// <summary>
    /// Removes the old model from the hand and creates a new one (Instantiate) based on the active item.
    /// </summary>
    /// <param name="items">The current array of inventory items.</param>
    private void UpdateHand(InventoryItem[] items)
    {
        // UpdateHand remains unchanged
        if (currentItemInHand != null)
            Destroy(currentItemInHand);

        if (items.Length > 0 && items[0].prefab != null)
        {
            currentItemInHand = Instantiate(items[0].prefab, hand);
            currentItemInHand.transform.localPosition = Vector3.zero;

            PickableItem pickable = items[0].prefab.GetComponent<PickableItem>();
            if (pickable != null)
                currentItemInHand.transform.localRotation = Quaternion.Euler(pickable.handRotation);
            else
                currentItemInHand.transform.localRotation = Quaternion.identity;
        }
    }
}