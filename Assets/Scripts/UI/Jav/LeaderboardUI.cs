using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardUI : MonoBehaviour
{
    public TMP_Text[] rankTexts;
    public TMP_Text[] userTexts;
    public TMP_Text[] pointsTexts;
    public TMP_Text[] timeTexts;

    void OnEnable()
    {
        UpdateLeaderboard();
    }

    public void UpdateLeaderboard()
    {
        List<ScoreEntry> topScores = ScoreManager.Instance.GetTopScores(rankTexts.Length);

        for (int i = 0; i < rankTexts.Length; i++)
        {
            if (i < topScores.Count)
            {
                rankTexts[i].text = (i + 1).ToString("00");
                userTexts[i].text = topScores[i].userName;
                pointsTexts[i].text = topScores[i].points.ToString();
                timeTexts[i].text = topScores[i].time;
            }
            else
            {
                rankTexts[i].text = "--";
                userTexts[i].text = "---";
                pointsTexts[i].text = "---";
                timeTexts[i].text = "--:--";
            }
        }
    }
}


