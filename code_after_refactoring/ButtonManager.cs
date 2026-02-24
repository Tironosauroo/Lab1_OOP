using UnityEngine;

/// <summary>
/// Manages the UI button interactions and panel transitions in the main menu.
/// </summary>
public class ButtonManager : MonoBehaviour
{
    /// <summary>Reference to the main menu panel.</summary>
    [SerializeField] private GameObject MainPanel;

    /// <summary>Reference to the credits panel.</summary>
    [SerializeField] private GameObject CreditsPanel;

    /// <summary>Reference to the settings panel.</summary>
    [SerializeField] private GameObject SettingsPanel;

    /// <summary>
    /// Exits the application when the exit button is clicked.
    /// </summary>
    public void ExitButton()
    {
        Application.Quit();
    }

    /// <summary>
    /// Hides the main menu and displays the credits panel.
    /// </summary>
    public void ShowCredits()
    {
        MainPanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }

    /// <summary>
    /// Hides the credits panel and returns to the main menu.
    /// </summary>
    public void HideCredits()
    {
        CreditsPanel.SetActive(false);
        MainPanel.SetActive(true);
    }

    /// <summary>
    /// Hides the main menu and displays the settings panel.
    /// </summary>
    public void ShowSettings()
    {
        MainPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    /// <summary>
    /// Hides the settings panel and returns to the main menu.
    /// </summary>
    public void HideSettings()
    {
        SettingsPanel.SetActive(false);
        MainPanel.SetActive(true);
    }

    /// <summary>
    /// Starts the game by loading the first playable scene.
    /// </summary>
    public void PlayGame()
    {
        LevelManager.LoadFirstScene();
    }
}