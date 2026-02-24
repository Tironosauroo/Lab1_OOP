using UnityEngine;

/// <summary>
/// Ensures that specific game objects persist across scene loads.
/// Implements a basic array-based Singleton pattern for multiple persistent objects.
/// </summary>
public class DontDestroy : MonoBehaviour
{
    /// <summary>Static array tracking persistent objects to prevent duplication.</summary>
    private static GameObject[] persistentObjects = new GameObject[3];

    /// <summary>The designated index for this specific object in the persistent array.</summary>
    public int objectIndex;

    /// <summary>
    /// Checks if an object at the given index already exists.
    /// If not, it marks this object as persistent. Otherwise, it destroys the duplicate.
    /// </summary>
    void Awake()
    {
        if (persistentObjects[objectIndex] == null)
        {
            persistentObjects[objectIndex] = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if (persistentObjects[objectIndex] != gameObject)
        {
            Destroy(gameObject);
        }
    }
}