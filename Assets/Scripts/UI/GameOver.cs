using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public TMP_Text scoreTexts;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(int score)
    {
        gameObject.SetActive(true);
        scoreTexts.text = "SCORE: " + score.ToString();
        Debug.Log("Your Game SCORE");
    }


    public void RetryGame()
    {
        SceneManager.LoadScene(""); // Until figure out where or how to restart at
        Debug.Log("Restarting the Game");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exiting the Game");
    }
}
