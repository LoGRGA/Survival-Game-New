using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject leaderboardEntryPrefab;  // Prefab for each leaderboard entry
    public Transform leaderboardContent;      // Parent to hold entries (inside Scroll View)
    public GameObject leaderboardPanel;       // Panel to show/hide leaderboard

    private List<PlayerScore> playerScores = new List<PlayerScore>();

    void Start()
    {
        LoadLeaderboard();
    }

    // Function to Load and Display the Leaderboard
    public void LoadLeaderboard()
    {
        // Clear old entries
        foreach (Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }

        // Load scores from PlayerPrefs
        playerScores = LoadScoresFromPrefs();

        // Sort scores in descending order
        playerScores.Sort((a, b) => b.score.CompareTo(a.score));

        // Display sorted scores
        for (int i = 0; i < playerScores.Count; i++)
        {
            GameObject entry = Instantiate(leaderboardEntryPrefab, leaderboardContent);
            Text[] texts = entry.GetComponentsInChildren<Text>();
            texts[0].text = (i + 1).ToString();   // Rank
            texts[1].text = playerScores[i].playerName;  // Name
            texts[2].text = playerScores[i].score.ToString();  // Score
        }
    }

    // Save new score to PlayerPrefs
    public void SaveNewScore(string playerName, int score)
    {
        playerScores.Add(new PlayerScore(playerName, score));
        PlayerPrefs.SetString("Leaderboard", JsonUtility.ToJson(new ScoreData(playerScores)));
        PlayerPrefs.Save();
    }

    // Load scores from PlayerPrefs
    private List<PlayerScore> LoadScoresFromPrefs()
    {
        if (PlayerPrefs.HasKey("Leaderboard"))
        {
            ScoreData data = JsonUtility.FromJson<ScoreData>(PlayerPrefs.GetString("Leaderboard"));
            return data.scores;
        }
        return new List<PlayerScore>();
    }

    // Function to Show/Hide Leaderboard
    public void ToggleLeaderboard()
    {
        bool isActive = leaderboardPanel.activeSelf;
        leaderboardPanel.SetActive(!isActive);
        if (!isActive)
        {
            LoadLeaderboard();
        }
    }
}

// Helper Classes for Score Storage
[System.Serializable]
public class PlayerScore
{
    public string playerName;
    public int score;

    public PlayerScore(string name, int score)
    {
        this.playerName = name;
        this.score = score;
    }
}

[System.Serializable]
public class ScoreData
{
    public List<PlayerScore> scores;

    public ScoreData(List<PlayerScore> scores)
    {
        this.scores = scores;
    }
}

