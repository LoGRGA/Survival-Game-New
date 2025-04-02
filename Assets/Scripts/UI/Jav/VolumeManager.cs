using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        // Load saved volume settings
        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 1f);
        AudioListener.volume = savedVolume;
        volumeSlider.value = savedVolume;
    }

    public void AdjustVolume()
    {
        float volume = volumeSlider.value;
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("GameVolume", volume); // Save volume setting
    }
}

