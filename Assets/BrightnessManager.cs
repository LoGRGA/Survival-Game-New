using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    public Slider brightnessSlider; // Reference to the Brightness Slider
    public Image brightnessOverlay; // Reference to the Black Overlay Image

    void Start()
    {
        // Load the saved brightness setting (default to 0.5 if not set)
        float savedBrightness = PlayerPrefs.GetFloat("GameBrightness", 0.5f);

        if (brightnessSlider != null)
        {
            brightnessSlider.value = savedBrightness;
            brightnessSlider.onValueChanged.AddListener(delegate { AdjustBrightness(); });
        }

        ApplyBrightness(savedBrightness);
    }

    public void AdjustBrightness()
    {
        if (brightnessSlider == null || brightnessOverlay == null) return;

        float brightness = brightnessSlider.value;
        PlayerPrefs.SetFloat("GameBrightness", brightness);
        ApplyBrightness(brightness);
    }

    private void ApplyBrightness(float brightness)
    {
        if (brightnessOverlay == null) return;

        // Now, 1.0 = Fully Bright (transparent overlay), 0.0 = Fully Dark (black overlay)
        Color overlayColor = brightnessOverlay.color;
        overlayColor.a = Mathf.Clamp(brightness, 0f, 1f); // 0 = Darkest, 1 = Fully Bright
        brightnessOverlay.color = overlayColor;
    }
}


