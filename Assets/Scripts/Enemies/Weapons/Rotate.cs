using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private float rotationSpeed = 1500f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateY();
    }

    private void RotateY(){
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
