using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggernautFlySword : MonoBehaviour
{
    private Transform playerTransform;
    private float rotationSpeed = 10f;
    private float moveSpeed = 10f;

    //SFX
    private AudioSource audioSource;
    private AudioClip rotationAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        //SFX
        audioSource = GetComponent<AudioSource>();
        rotationAudioClip = EnemyBehaviour.LoadAudioClip("Juggernaut SFX", "Juggernaut Sword Rotation");
        PlayRotationSFX();

    }

    // Update is called once per frame
    void Update()
    {
        TrackPlayer();
        
    }

    private void TrackPlayer(){
        if (playerTransform != null)
        {
            Vector3 vector = new Vector3(0, 1.2f, 0);
            Vector3 playerPosition = playerTransform.position + vector;

            Vector3 directionToPlayer = playerPosition - transform.position;
            
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            // move forward
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    //SFX
    private void PlayRotationSFX(){
        audioSource.clip = rotationAudioClip;
        audioSource.Play();
    }
}


