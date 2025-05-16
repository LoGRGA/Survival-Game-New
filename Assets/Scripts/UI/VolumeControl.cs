using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
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
        volume = volumeSlider.value;
        audioMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume); // Save user setting
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            float currentVolume;
            if (audioMixer.GetFloat("MasterVolume", out currentVolume))
            {
                Debug.Log("MasterVolume set to: " + currentVolume);
            }
            else
            {
                Debug.LogError("MasterVolume parameter not found!");
            }

        }
    }
}





