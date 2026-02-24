using UnityEngine;

/// <summary>
/// A data class that describes an item in the inventory.
/// </summary>
[System.Serializable]
public class InventoryItem
{
    /// <summary>Subject name.</summary>
    public string itemName;

    /// <summary>Item icon to display in the UI.</summary>
    public Sprite itemSprite;

    /// <summary>Prefab 3D model of an object to display in the hand.</summary>
    public GameObject prefab;

    /// <summary>
    /// Constructor for creating a new record about an item.
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="sprite">Sprite (icon).</param>
    /// <param name="prefabObject">3D prefab.</param>
    public InventoryItem(string name, Sprite sprite, GameObject prefabObject)
    {
        itemName = name;
        itemSprite = sprite;
        prefab = prefabObject;
    }
}