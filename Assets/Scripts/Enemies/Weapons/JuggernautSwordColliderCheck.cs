using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggernautSwordColliderCheck : MonoBehaviour
{
    private LayerMask playerLayer;
    private LayerMask obstacleLayer;

    private GameObject playerObject;
    private PlayerController playerController;

    private GameObject rootObject;

    public GameObject swordHitSFX;
    

    // Start is called before the first frame update
    void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
        obstacleLayer = LayerMask.GetMask("Obstacle");

        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerController = playerObject.GetComponent<PlayerController>();

        rootObject = transform.root.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other){
        //if collide with player
        if (((1 << other.gameObject.layer) & playerLayer) != 0){
            playerController.TakeDamge(10);
            InstantiateSwordHitSFX();
            Destroy(rootObject);
        }

        //if collide with obstcle
        else if(((1 << other.gameObject.layer) & obstacleLayer) != 0){
            InstantiateSwordHitSFX();
            Destroy(rootObject);
        }    
        
    }

    private void InstantiateSwordHitSFX(){
        Instantiate(swordHitSFX, transform.position, transform.rotation);
    }
}
