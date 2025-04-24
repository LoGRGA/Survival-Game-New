using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    private List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
    private List<LeaderboardDisplay> displays = new List<LeaderboardDisplay>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterDisplay(LeaderboardDisplay display)
    {
        if (!displays.Contains(display))
        {
            displays.Add(display);
            display.UpdateDisplay(entries); // Immediately update with current data
        }
    }

    public void SaveScore(string user, int score, string time)
    {
        entries.Add(new LeaderboardEntry(user, score, time));
        entries.Sort((a, b) => b.score.CompareTo(a.score)); // Descending

        foreach (var display in displays)
        {
            display.UpdateDisplay(entries);
        }
    }
}
