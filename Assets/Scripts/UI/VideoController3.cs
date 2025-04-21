using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class VideoController3 : MonoBehaviour
{
    // Start is called before the first frame update
    public VideoPlayer videoPlayer;
    public Button myButton;
    public TMP_Text myText;

    void Start()
    {
        myButton.gameObject.SetActive(false);
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
        myText.gameObject.SetActive(true);
    }
}
