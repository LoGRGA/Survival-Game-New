using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatHook : MonoBehaviour
{
    private LayerMask playerLayer;
    private LayerMask obstacleLayer;

    private bool isHookable = false;
    private bool isHookPlayer = false;

    private GameObject playerObject;
    private PlayerController playerController;

    private bool isAbleDealDamage = false;

    private PudgeBehaviour enemy;

    //SFX
    protected AudioSource audioSource;
    protected AudioClip hookTouchAudioClip;

    void Start(){
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerController = playerObject.GetComponent<PlayerController>();

        enemy = GetComponentInParent<PudgeBehaviour>();

        playerLayer = LayerMask.GetMask("Player");
        obstacleLayer = LayerMask.GetMask("Obstacle");

        //SFX
        audioSource = GetComponent<AudioSource>();
        hookTouchAudioClip = EnemyBehaviour.LoadAudioClip("Pudge SFX", "Hook Touch");
    }

    // Update is called once per frame
    void LateUpdate()
    {   
        if(isHookable && isHookPlayer){
            playerController.ChangePlayerPosition(transform.position);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isHookable){
            // if collide with player
            if (((1 << other.gameObject.layer) & playerLayer) != 0)
            {
                isHookPlayer = true;
                enemy.SetIsRetractingHook(true);

                //deal 10 damage to player while hooked enemy
                DealDamageToPlayer(other);
                
            }
            // if collide with obstcle
            else if (((1 << other.gameObject.layer) & obstacleLayer) != 0)
            {
                enemy.SetIsRetractingHook(true);

                //PlayHookTouchSFX();
            }
        }
    }

    //deal damage to player
    private void DealDamageToPlayer(Collider other){
        if(isAbleDealDamage){
            playerController.TakeDamge(10);

            //play SFX when touch Player
            PlayHookTouchSFX();
            
            isAbleDealDamage = false;
        }
    }

    public void SetIsHookable(bool value){
        isHookable = value;
    }

    public void SetIsHookPlayer(bool value){
        isHookPlayer = value;
    }

    public void SetIsAbleDealDamage(bool value){
        isAbleDealDamage = value;
    }

    //SFX
    private void PlayHookTouchSFX(){
        audioSource.clip = hookTouchAudioClip;
        audioSource.Play();
    }
}
