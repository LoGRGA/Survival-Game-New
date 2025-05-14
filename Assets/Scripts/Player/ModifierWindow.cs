using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierWindow : MonoBehaviour
{
    public GameObject canvas;

    private PlayerController playerController;
    private MouseLook mouseLook;
    public MouseLook mouseLook2;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        mouseLook = GetComponent<MouseLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M)){
            canvas.SetActive(!canvas.activeSelf);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            playerController.enabled = false;
            mouseLook.enabled = false;
            mouseLook2.enabled = false;

            if(!canvas.activeSelf){
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                playerController.enabled = true;
                mouseLook.enabled = true;
                mouseLook2.enabled = true;
            }    
        }
    }
}
