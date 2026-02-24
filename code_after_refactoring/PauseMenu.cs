using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the in-game pause menu, including pausing time, toggling UI panels, and handling scene transitions.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [Header("UI Elements")]
    /// <summary>Reference to the main player HUD (Heads-Up Display).</summary>
    public GameObject hud;

    /// <summary>Reference to the container holding the main pause menu buttons.</summary>
    public GameObject buttonsGroup;

    /// <summary>Reference to the settings panel within the pause menu.</summary>
    public GameObject settingsMenu;

    /// <summary>Reference to the root pause menu UI panel.</summary>
    public GameObject menu;

    /// <summary>Tracks whether the game is currently paused.</summary>
    private bool isPaused = false;

    /// <summary>
    /// Initializes input subscriptions via the centralized InputManager.
    /// Listens for the "Menu" action (usually Esc or Start button).
    /// </summary>
    void Start()
    {
        InputManager.Instance.Controls.Player.Menu.performed += ctx => Menu();
    }

    /// <summary>
    /// Sets the initial state of the menu panels upon loading the scene.
    /// Ensures the game starts unpaused and the menu is hidden.
    /// </summary>
    void Awake()
    {
        isPaused = false;
        menu.SetActive(false);
        if (settingsMenu != null) settingsMenu.SetActive(false);
    }

    /// <summary>
    /// Toggles the game's pause state.
    /// </summary>
    void Menu()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    /// <summary>
    /// Pauses the game by setting the time scale to zero, showing the cursor, and enabling the menu UI.
    /// </summary>
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (hud != null) hud.SetActive(false);
        menu.SetActive(true);
        if (buttonsGroup != null) buttonsGroup.SetActive(true);
        if (settingsMenu != null) settingsMenu.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Resumes the game by restoring the time scale, hiding the cursor, and disabling the menu UI.
    /// </summary>
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (hud != null) hud.SetActive(true);
        menu.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Hides the main pause buttons and displays the settings panel.
    /// </summary>
    public void OpenSettings()
    {
        if (buttonsGroup != null) buttonsGroup.SetActive(false);
        if (settingsMenu != null) settingsMenu.SetActive(true);
    }

    /// <summary>
    /// Hides the settings panel and returns to the main pause buttons.
    /// </summary>
    public void BackFromSettings()
    {
        if (buttonsGroup != null) buttonsGroup.SetActive(true);
        if (settingsMenu != null) settingsMenu.SetActive(false);
    }

    /// <summary>
    /// Unpauses the game, cleans up persistent UI/Player objects, and loads the main menu scene (Scene 0).
    /// </summary>
    public void BackToMenu()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (hud != null) Destroy(hud);
        Destroy(gameObject); // Destroy the pause menu/player object to prevent duplicates in the main menu

        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
