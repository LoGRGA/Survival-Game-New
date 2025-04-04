using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings_Manager : MonoBehaviour
{
    public OptionsMenu optionsMenu;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            optionsMenu.gameObject.SetActive(!optionsMenu.gameObject.activeSelf);
        }
    }
}
