using System;
using UnityEngine;

/// <summary>
/// The main controller of the inventory logic. Manages the queue and notifies other components of changes.
/// </summary>
public class InventoryManager : MonoBehaviour, IInventoryQueue<InventoryItem>
{
    /// <summary>An instance of a queue for storing inventory items.</summary>
    private InventoryQueue<InventoryItem> itemQueue = new InventoryQueue<InventoryItem>();

    /// <summary>
    /// An event that is triggered after each change in inventory status.
    /// </summary>
    public event Action<InventoryItem[]> OnInventoryChanged;

    /// <summary>The current number of items in inventory.</summary>
    public int Count => itemQueue.Count;

    /// <summary>Adds an item to the queue and raises an update event.</summary>
    /// <param name="item">Item to add.</param>
    public void Enqueue(InventoryItem item)
    {
        itemQueue.Enqueue(item);
        NotifyInventoryChanged();
    }

    /// <summary>Removes an item from the front of the queue and raises an update event.</summary>
    /// <returns>Inventory item removed.</returns>
    public InventoryItem Dequeue()
    {
        InventoryItem item = itemQueue.Dequeue();
        NotifyInventoryChanged();
        return item;
    }

    /// <summary>Returns an array of all items in the inventory.</summary>
    /// <returns>An array of objects of type InventoryItem.</returns>
    public InventoryItem[] IQtoArray()
    {
        return itemQueue.IQtoArray();
    }

    /// <summary>A wrapper for Enqueue, convenient for calling from other scripts (e.g. PlayerInteraction).</summary>
    /// <param name="newItem">An item picked up by the player.</param>
    public void AddItem(InventoryItem newItem)
    {
        Enqueue(newItem);
    }

    /// <summary>
    /// Takes the active item (the beginning of the queue) and moves it to the end of the queue.
    /// </summary>
    public void NextItem()
    {
        if (Count > 1)
        {
            InventoryItem first = Dequeue();
            Enqueue(first);
        }
    }

    /// <summary>
    /// Helper method for safely calling the OnInventoryChanged event.
    /// </summary>
    private void NotifyInventoryChanged()
    {
        OnInventoryChanged?.Invoke(IQtoArray());
    }

    /// <summary>Unity lifecycle method. Updates the state of subscribers when the game starts.</summary>
    private void Start()
    {
        NotifyInventoryChanged();
    }
}