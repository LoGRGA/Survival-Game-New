using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject optionsMenu;      // Reference to the Options Menu
    public GameObject leaderboardMenu;  // Reference to the Leaderboard Menu
    public GameObject volumeMenu;       // Reference to the Volume Adjuster Menu

    void Start()
    {
        LockCursor(); // Lock cursor at the start of the game
    }

    // Open the leaderboard and close the options menu
    public void OpenLeaderboard()
    {
        if (optionsMenu != null && leaderboardMenu != null)
        {
            optionsMenu.SetActive(false);
            leaderboardMenu.SetActive(true);
            UnlockCursor();
        }
    }

    // Close the leaderboard and open the options menu
    public void CloseLeaderboard()
    {
        if (optionsMenu != null && leaderboardMenu != null)
        {
            optionsMenu.SetActive(true);
            leaderboardMenu.SetActive(false);
            UnlockCursor();
        }
    }

    // Open the volume adjuster menu and close the options menu
    public void OpenVolumeMenu()
    {
        if (optionsMenu != null && volumeMenu != null)
        {
            optionsMenu.SetActive(false);
            volumeMenu.SetActive(true);
            UnlockCursor();
        }
    }

    // Close the volume adjuster menu and open the options menu
    public void CloseVolumeMenu()
    {
        if (optionsMenu != null && volumeMenu != null)
        {
            optionsMenu.SetActive(true);
            volumeMenu.SetActive(false);
            UnlockCursor();
        }
    }

    // Toggle options menu
    public void ToggleOptionsMenu()
    {
        bool isActive = optionsMenu.activeSelf;
        optionsMenu.SetActive(!isActive);

        if (optionsMenu.activeSelf || leaderboardMenu.activeSelf || volumeMenu.activeSelf)
        {
            UnlockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    // Lock cursor when playing
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Unlock cursor when in UI
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (leaderboardMenu.activeSelf)
            {
                CloseLeaderboard();
            }
            else if (volumeMenu.activeSelf)
            {
                CloseVolumeMenu();
            }
            else
            {
                ToggleOptionsMenu();
            }
        }
    }
}
