using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene transition

public class CureCollect : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.F;
    private bool playerInRange = false;
    private float timeTaken;
    private float startTime;

    // Public reference for player and scene name (drag-and-drop in inspector)
    public GameObject player;
    public string sceneToLoad; // Name of the scene to load
    public ScoreManager_new scoreManager;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            timeTaken = Time.time - startTime;
            GameManager.Instance.GameComplete(timeTaken); // Save leaderboard stats

            // Change the player's tag (optional)
            if (player != null)
            {
                player.tag = "Finished"; // You can change this to any tag you prefer
            }

            // Optionally deactivate the cure object or destroy it
            gameObject.SetActive(false);

            // Load the next scene (make sure to set it in the inspector)
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                //Cursor.lockState = CursorLockMode.None;
                //Cursor.visible = true;
                PlayerPrefs.SetString("GameOutcome", "victory");
                if(scoreManager != null)
                    PlayerPrefs.SetString("Score", scoreManager.score.ToString());
                PlayerPrefs.Save();
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}


