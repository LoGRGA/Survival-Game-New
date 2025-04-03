using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private List<ScoreEntry> scores = new List<ScoreEntry>();
    private int playCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int points, string time)
    {
        playCount++;
        string userName = "Play " + playCount;

        ScoreEntry newEntry = new ScoreEntry(userName, points, time);
        scores.Add(newEntry);
        scores = scores.OrderByDescending(s => s.points).ToList(); // Sort by highest score

        SaveScores();
    }

    public List<ScoreEntry> GetTopScores(int maxEntries = 5)
    {
        return scores.Take(maxEntries).ToList();
    }

    private void SaveScores()
    {
        PlayerPrefs.SetInt("PlayCount", playCount);
        PlayerPrefs.SetString("Leaderboard", JsonUtility.ToJson(new ScoreList(scores)));
        PlayerPrefs.Save();
    }

    private void LoadScores()
    {
        playCount = PlayerPrefs.GetInt("PlayCount", 0);
        string savedData = PlayerPrefs.GetString("Leaderboard", "{}");
        scores = JsonUtility.FromJson<ScoreList>(savedData).entries;
    }

    public static implicit operator ScoreManager(ScoreManager_new v)
    {
        throw new NotImplementedException();
    }
}

[System.Serializable]
public class ScoreEntry
{
    public string userName;
    public int points;
    public string time;

    public ScoreEntry(string userName, int points, string time)
    {
        this.userName = userName;
        this.points = points;
        this.time = time;
    }
}

[System.Serializable]
public class ScoreList
{
    public List<ScoreEntry> entries = new List<ScoreEntry>();

    public ScoreList(List<ScoreEntry> entries)
    {
        this.entries = entries;
    }
}

