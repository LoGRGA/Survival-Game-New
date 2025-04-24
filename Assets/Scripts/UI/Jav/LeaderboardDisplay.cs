using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardDisplay : MonoBehaviour
{
    public List<TextMeshProUGUI> userTexts;
    public List<TextMeshProUGUI> scoreTexts;
    public List<TextMeshProUGUI> timeTexts;

    private void Start()
    {
        if (LeaderboardManager.Instance != null)
        {
            LeaderboardManager.Instance.RegisterDisplay(this);
        }
    }

    public void UpdateDisplay(List<LeaderboardEntry> entries)
    {
        for (int i = 0; i < 5; i++)
        {
            if (i < entries.Count)
            {
                userTexts[i].text = entries[i].user;
                scoreTexts[i].text = entries[i].score.ToString();
                timeTexts[i].text = entries[i].time;
            }
            else
            {
                userTexts[i].text = "-";
                scoreTexts[i].text = "-";
                timeTexts[i].text = "-";
            }
        }
    }
}


