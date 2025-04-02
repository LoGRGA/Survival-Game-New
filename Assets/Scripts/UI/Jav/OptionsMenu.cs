using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsMenu; // The main options menu
    public GameObject volumeAdjusterCanvas; // The volume adjuster menu
    public Slider volumeSlider; // The volume slider

    private void Start()
    {
        // Load saved volume level if available
        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 1f);
        AudioListener.volume = savedVolume;
        volumeSlider.value = savedVolume;

        // Hide volume menu initially
        volumeAdjusterCanvas.SetActive(false);
    }

    private void Update()
    {
        // Press ESC to return to the options menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (volumeAdjusterCanvas.activeSelf)
            {
                CloseVolumeMenu();
            }
        }
    }

    public void OpenVolumeMenu()
    {
        optionsMenu.SetActive(false);
        volumeAdjusterCanvas.SetActive(true);
    }

    public void CloseVolumeMenu()
    {
        optionsMenu.SetActive(true);
        volumeAdjusterCanvas.SetActive(false);
    }

    public void AdjustVolume()
    {
        float volume = volumeSlider.value;
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("GameVolume", volume); // Save volume setting
    }
}

