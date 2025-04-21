using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class VideoController2 : MonoBehaviour
{
    // Start is called before the first frame update
    public VideoPlayer videoPlayer;
    public Button myButton, myButton2;
    public TMP_Text myText;

    void Start()
    {
        myButton.gameObject.SetActive(false);
        myButton2.gameObject.SetActive(false);
        myText.gameObject.SetActive(false);

        videoPlayer.loopPointReached += onVideoFinished;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onVideoFinished(VideoPlayer vp)
    {
        myButton.gameObject.SetActive(true);
        myButton2.gameObject.SetActive(true);
        myText.gameObject.SetActive(true);
    }
}
