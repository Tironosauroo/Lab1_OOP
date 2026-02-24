using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages game settings such as volume, quality, resolution, and fullscreen mode.
/// Saves and loads user preferences using Unity's PlayerPrefs.
/// </summary>
public class Settings : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider volumeSlider;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    /// <summary>List of supported screen resolutions for the current device.</summary>
    private List<Resolution> resolutions = new List<Resolution>();

    /// <summary>
    /// Initializes UI elements with saved preferences or default values upon loading.
    /// </summary>
    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        qualityDropdown.value = PlayerPrefs.GetInt("QualityLevel", 2);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        resolutions = new List<Resolution>(Screen.resolutions);
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Count; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
    }

    /// <summary>
    /// Adjusts the global audio volume based on the slider value and saves it.
    /// </summary>
    public void SetMasterVolume()
    {
        float value = volumeSlider.value;
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Changes the graphical quality level and saves the preference.
    /// </summary>
    /// <param name="index">The index of the selected quality level.</param>
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("QualityLevel", index);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Changes the screen resolution and saves the preference.
    /// </summary>
    /// <param name="index">The index of the selected resolution from the dropdown.</param>
    public void SetResolution(int index)
    {
        if (resolutions != null && index < resolutions.Count)
        {
            Resolution res = resolutions[index];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
            PlayerPrefs.SetInt("ResolutionIndex", index);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// Toggles fullscreen mode and saves the preference.
    /// </summary>
    /// <param name="isFullscreen">True for fullscreen, false for windowed.</param>
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }
}