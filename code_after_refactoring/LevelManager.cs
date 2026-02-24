using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Enumeration of available scenes in the game.
/// </summary>
public enum SceneList
{
    Menu,
    Tutorial
}

/// <summary>
/// Manages scene transitions and loading within the game.
/// </summary>
public class LevelManager : MonoBehaviour
{
    /// <summary>The target spawn position for the player in the new scene.</summary>
    public GameObject spawnPosition;

    /// <summary>The next scene to be loaded.</summary>
    public SceneList nextScene;

    /// <summary>
    /// Instantly loads the first playable scene (Tutorial).
    /// </summary>
    public static void LoadFirstScene()
    {
        SceneManager.LoadScene(((int)SceneList.Tutorial));
    }

    /// <summary>
    /// Initiates an asynchronous scene transition.
    /// </summary>
    /// <param name="nextScene">The scene to transition to.</param>
    /// <param name="spawnPosition">The position to spawn the player at.</param>
    public void SwitchScene(SceneList nextScene, GameObject spawnPosition)
    {
        StartCoroutine(SwitchSceneAsync());
    }

    /// <summary>
    /// Coroutine handling the asynchronous scene loading process.
    /// </summary>
    /// <returns>IEnumerator for the coroutine.</returns>
    IEnumerator SwitchSceneAsync()
    {
        // Add loading logic here (e.g., loading screens, async load operations)
        yield return null;
    }
}