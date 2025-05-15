using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer;      // Drag your MasterMixer here
    public Slider volumeSlider;        // Drag your UI slider here

    void Start()
    {
        // Load saved volume or set to max (0 dB)
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 0);
        volumeSlider.value = savedVolume;
        audioMixer.SetFloat("MasterVolume", savedVolume);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume); // Save user setting
    }
}





