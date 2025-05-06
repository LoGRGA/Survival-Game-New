using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAura : MonoBehaviour
{
    private LayerMask playerLayer;

    private GameObject playerObject;
    private PlayerController playerController;
    private Transform playerTransform;
    private Vector3 targetPosition;
    private Vector3 moveDirection;

    private float moveSpeed = 30f;

    public GameObject swordAuraHitSFX;

    //SFX
    private AudioSource audioSource;
    private AudioClip swordAuraAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        SetTargetPosition(playerTransform.position);
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerController = playerObject.GetComponent<PlayerController>();

        playerLayer = LayerMask.GetMask("Player");
        StartCoroutine(DestroyWithDelay());

        Vector3 initialDirection = targetPosition - transform.position;
        moveDirection = new Vector3(initialDirection.x, 0, initialDirection.z).normalized;

        Vector3 currentEulerAngles = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(90, currentEulerAngles.y+90, currentEulerAngles.z);

        //SFX
        audioSource = GetComponent<AudioSource>();
        swordAuraAudioClip = EnemyBehaviour.LoadAudioClip("Juggernaut SFX", "Juggernaut Sword Aura");
        PlayRotationSFX();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardPlayer();
    }

    void OnTriggerEnter(Collider other)
    {
        //if collide with player
        if (((1 << other.gameObject.layer) & playerLayer) != 0){
            playerController.TakeDamge(10);
            InstantiateSwordAuraHitSFX();
            Destroy(gameObject);
        }
    }

    private void MoveTowardPlayer(){
        
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    private IEnumerator DestroyWithDelay(){
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void InstantiateSwordAuraHitSFX(){
        Instantiate(swordAuraHitSFX, transform.position, transform.rotation);
    }

    private void SetTargetPosition(Vector3 newPosition)
    {
        targetPosition = newPosition;
    }

    //SFX
    private void PlayRotationSFX(){
        audioSource.clip = swordAuraAudioClip;
        audioSource.Play();
    }
}
