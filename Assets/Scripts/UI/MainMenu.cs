using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public string firstLevel;
    public GameObject settingScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(firstLevel);
        Debug.Log("Game Starting");
    }

    public void OpenSetting()
    {
        settingScreen.SetActive(true);
        Debug.Log("Opening Game Setting");
    }

    public void CloseSetting()
    {
        settingScreen.SetActive(false);
        Debug.Log("Close Game Setting");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exiting the Game");
    }
}
