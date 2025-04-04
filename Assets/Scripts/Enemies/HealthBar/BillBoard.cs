using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Transform cam;
    //public Transform cam;

    void Start()
    {
       cam = GameObject.FindGameObjectWithTag("Player").transform.Find("Body/Main Camera");
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}


//learn from https://www.youtube.com/watch?v=3JjBJfoWDCM
