using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggernautSwordHitSFX : MonoBehaviour
{
    //SFX
    private AudioSource audioSource;
    private AudioClip swordHitAudioClip;
    
    // Start is called before the first frame update
    void Start()
    {
        //SFX
        audioSource = GetComponent<AudioSource>();
        swordHitAudioClip = EnemyBehaviour.LoadAudioClip("Juggernaut SFX", "Juggernaut Sword Rotation Hit");

        PlaySwordHitSFX();
        StartCoroutine(DestroyWithDelay());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator DestroyWithDelay(){
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    //SFX
    private void PlaySwordHitSFX(){
        audioSource.clip = swordHitAudioClip;
        audioSource.Play();
    }
}
