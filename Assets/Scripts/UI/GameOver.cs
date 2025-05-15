using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public TMP_Text scoreTexts;
    public TMP_Text scores;
    public string scoring;
    // Start is called before the first frame update
    void Start()
    {
        scores.SetText(scoring);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(int score)
    {
        gameObject.SetActive(true);
        scoreTexts.text = "SCORE: ";
        //Debug.LogWarning(PlayerPrefs.GetString("Score", "none"));
        scores.text = PlayerPrefs.GetString("Score", "none");
        //Debug.Log("Your Game SCORE");
    }


    public void RetryGame()
    {
        SceneManager.LoadScene("GameScene"); // Until figure out where or how to restart at
        Debug.Log("Restarting the Game");
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Proto Menu (Jav)");
        Debug.Log("Exiting the Game");
    }
}
