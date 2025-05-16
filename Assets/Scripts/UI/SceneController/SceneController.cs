using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject victoryBanner, defeatBanner, timeoutBanner;
    private Victory victory;
    private GameOver gameOver;
    private string score;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        string outcome = PlayerPrefs.GetString("GameOutcome", "none"); // Default: none
        score = PlayerPrefs.GetString("Score", "0");

        victoryBanner.SetActive(outcome == "victory");
        defeatBanner.SetActive(outcome == "defeat");
        timeoutBanner.SetActive(outcome == "timeout");

        if (victoryBanner.activeSelf)
            victory = victoryBanner.GetComponent<Victory>();
        else if (defeatBanner.activeSelf)
            gameOver = defeatBanner.GetComponent<GameOver>();
        else if (timeoutBanner.activeSelf)
            gameOver = timeoutBanner.GetComponent<GameOver>();

        ScoreUpdate();

    }

    void ScoreUpdate()
    {
        if (victory)
        {
            victory.scoring = score;
            victory.SetScore();
        }
        else
        {
            gameOver.scoring = score;
            gameOver.SetScore();
        }
    }
}
