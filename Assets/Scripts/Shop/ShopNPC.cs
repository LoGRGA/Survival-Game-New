using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopNPC : MonoBehaviour
{
    public bool playerInRange;
    public bool isTalkingWithPlayer;

    //Open a panel that is going to allow us to buy,sell or leave the conversation
    public GameObject shopMenuUI;


    //Pause Game
    public bool isPaused;

    //MouseLook 
    public MouseLook MouseLook;

    //check if shop ui is open or not
    public bool isOpen;

    //Button For Buy,Sell and Exit;
    public Button btnBuy;
    public Button btnSell;
    public Button exitBTN;

    //Back page Button
    public Button backpageBuy;
    public Button backpageSell;


    //Buy and Sell Section
    public GameObject buyPanelUI;
    public GameObject sellPanelUI;

    public void Start()
    {
        shopMenuUI.SetActive(false);
        //buyBTN.onClick.AddListener(BuyMode);
        //sellBTN.onClick.AddListener(SellMode);
        //exitBTN.onClick.AddListener(StopTalking);

        // buyBTN.onClick.AddListener();
        // buyBTN.onClick.AddListener();
    }

    void Update()
    {
        // Check if the player presses the E key and an NPC is nearby
        if (Input.GetKeyDown(KeyCode.E) && playerInRange == true)
        {
            // Open the shop UI
            DisplayDialogUI();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
            isOpen = true;
            MouseLook.canLook = false;
        }
        
    }

    public void BuyMode()
    {
     //sellPanelUI.SetActive(false);
    buyPanelUI.SetActive(true);

       HideDialogUI();
    }
    public void SellMode()
     {
     //buyPanelUI.SetActive(false);
     sellPanelUI.SetActive(true);

        HideDialogUI();
    }

    public void BackPageBuy()
    {
        buyPanelUI.SetActive(false);
        sellPanelUI.SetActive(false);
        DisplayDialogUI();
    }

    public void BackPageSell()
    {
        buyPanelUI.SetActive(false);
        sellPanelUI.SetActive(false);
        DisplayDialogUI();
    }

    //A way to switch the between the buy or sell mode
    //public void DialogMode()
    //{
    // buyPanelUI.SetActive(false);
    // sellPanelUI.SetActive(false);

    //DisplayDialogUI();
    //}

    //stop and starting the interaction
    public void Talk()
    {
        isTalkingWithPlayer = true;
        DisplayDialogUI();

    } 

    public void StopTalking()
    {
        isTalkingWithPlayer = false;
        HideDialogUI();

    } 

    private void DisplayDialogUI()
    {
        shopMenuUI.SetActive(true);
    }

    private void HideDialogUI()
    {
        shopMenuUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        isOpen = false;
        MouseLook.canLook = true;
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
