using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrintDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(DestroyWithDelay());
    }

    //destroy the enemy after die animation is done
    protected IEnumerator DestroyWithDelay(){
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
