using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInventory : MonoBehaviour
{

    //reference to inventory system
    public GameObject inventoryScreenUI;

    //Pause Game
    public GameObject pauseGame;
    public bool isPaused;

    //MouseLook 
    public MouseLook MouseLook;
    

    //check if inventory screen is open or not
    public bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        pauseGame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            isOpen = true;
            MouseLook.canLook = false;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
            isOpen = false;
            MouseLook.canLook = true;
            
        }
    }

    public void PauseGame()
    {
        pauseGame.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseGame.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;

    }


}
