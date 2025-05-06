using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotArea : MonoBehaviour
{
    private GameObject playerObject;
    private PlayerController playerController;

    private LayerMask playerLayer;

    //private Collider rotArea;

    //damage related
    private bool isAbleDealDamage = false;
    private float dealDamageTimer = 0f;
    private float dealDamageInterval = 1f;
    private int dealDamageAmount = 5;


    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerController = playerObject.GetComponent<PlayerController>();

        playerLayer = LayerMask.GetMask("Player");

        //rotArea = transform.Find("Rot Area").GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        dealDamageTimer += Time.deltaTime;

        if(isAbleDealDamage && dealDamageTimer >= dealDamageInterval){
            playerController.TakeDamge(dealDamageAmount);
            dealDamageTimer = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0){
            isAbleDealDamage = true;
            Debug.Log("isAbleDealDamage in trigger = " + isAbleDealDamage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0){
            isAbleDealDamage = false;
            dealDamageTimer = 0f;
        }
    }
}
