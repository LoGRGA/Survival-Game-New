using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public LeaderboardManager leaderboardManager;

    private int gameplayNumber = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameComplete(float timeTaken)
    {
        int score = ScoreManager_new.instance.score;
        string formattedTime = FormatTime(timeTaken);
        string user = $"Game {gameplayNumber++}";
        leaderboardManager.SaveScore(user, score, formattedTime);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return $"{minutes:00}:{seconds:00}";
    }
}

