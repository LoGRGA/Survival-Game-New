using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    public ScoreManager_new scoreManager;

    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 60)
        {
            remainingTime -= Time.deltaTime;
        }
        else if(remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            timerText.color = Color.yellow;
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
            timerText.color = Color.red;
            Timeout();
        }


        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }
    void Timeout()
    {
        PlayerPrefs.SetString("GameOutcome", "timeout");
        if (scoreManager != null)
            PlayerPrefs.SetString("Score", scoreManager.score.ToString());
        PlayerPrefs.Save();
        SceneManager.LoadScene("Chin Ann UI");
    }
}


