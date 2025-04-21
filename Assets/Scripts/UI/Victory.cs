using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Video;

public class Victory : MonoBehaviour
{
    public TMP_Text scoreTexts;
    public VideoPlayer clifford;
    public Button playClifford;

    public VideoPlayer previousVideo;

    // Start is called before the first frame update
    void Start()
    {
        playClifford.onClick.AddListener(ExitGame);

        VideoController2 vc = FindObjectOfType<VideoController2>();
        if (vc != null)
        {
            previousVideo = vc.videoPlayer;
            playClifford = vc.myButton;
            scoreTexts = vc.myText;
        }
        clifford.loopPointReached += OnSecondVideoFinished;
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

    public void ExitGame()
    {
        if (previousVideo != null && previousVideo.gameObject.activeSelf)
        {
            previousVideo.gameObject.SetActive(false);
        }

        if (playClifford != null) playClifford.gameObject.SetActive(false);
        if (scoreTexts != null) scoreTexts.gameObject.SetActive(false);

        clifford.Play();
        Debug.Log("Now playing second video");
    }

    public void OnSecondVideoFinished(VideoPlayer vp)
    {
        Application.Quit();
        Debug.Log("Exiting the Game");
    }
}
