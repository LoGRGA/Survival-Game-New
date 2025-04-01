using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class PlaytimeTracker : MonoBehaviour
{
    public TMP_Text playtimeText; // Reference to the TMP text element
    private float playtime = 1f;

    void Update()
    {
        playtime += Time.deltaTime; // Increment playtime
        UpdatePlaytimeUI();
    }

    void UpdatePlaytimeUI()
    {
        if (playtimeText != null)
        {
            int minutes = Mathf.FloorToInt(playtime / 60);
            int seconds = Mathf.FloorToInt(playtime % 60);
            playtimeText.text = $"{minutes:D2}:{seconds:D2}"; // Only display time in MM:SS format
        }
    }
}



