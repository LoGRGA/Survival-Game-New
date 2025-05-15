using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToMainMenu : MonoBehaviour
{
    public string mainMenuSceneName = "Proto menu (Jav)"; 

    public void QuitToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}

