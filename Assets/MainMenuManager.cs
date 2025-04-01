using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject settingsMenu;      // Reference to the Settings Canvas
    public GameObject leaderboardMenu;   // Reference to the Leaderboard Canvas

    // Start the game (Load the main gameplay scene)
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); // Change "GameScene" to your actual scene name
    }

    // Open the Settings menu
    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
    }

    // Close the Settings menu
    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
    }

    // Open the Leaderboard menu
    public void OpenLeaderboard()
    {
        leaderboardMenu.SetActive(true);
    }

    // Close the Leaderboard menu
    public void CloseLeaderboard()
    {
        leaderboardMenu.SetActive(false);
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit(); // This will quit the game (only works in a built version)
        Debug.Log("Game Quit!"); // Just for testing in Unity editor
    }
}

