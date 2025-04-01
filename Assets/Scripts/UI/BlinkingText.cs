using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject StartB;
    public float repeatTime = 1f;
    void Start()
    {
        InvokeRepeating("ChangeofStatesObject",1f,repeatTime);
    }

    public void ChangeofStatesObject()
    {
        StartB.SetActive(!StartB.activeInHierarchy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
