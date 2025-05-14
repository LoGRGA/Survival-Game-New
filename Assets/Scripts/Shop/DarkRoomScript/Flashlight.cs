using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{

    [SerializeField] GameObject Flight;
    private bool FlashlightActive = false;

    // Start is called before the first frame update
    void Start()
    {
        Flight.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if(FlashlightActive == false){
                Flight.gameObject.SetActive(true);
                FlashlightActive = true;
            }
            else
            {
                Flight.gameObject.SetActive(false);
                FlashlightActive = false;
            }
        }
    }
}
