using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rot : MonoBehaviour
{
    //children game object
    private ParticleSystem rotVFX;
    private RotArea rotArea;

    //
    private bool isRotActive = false;

    //SFX
    private AudioSource audioSource;
    private AudioClip rottingAudioClip; 

    // Start is called before the first frame update
    void Start()
    {
        rotVFX = transform.Find("Rot VFX").GetComponent<ParticleSystem>();
        rotArea = transform.Find("Rot Area").GetComponent<RotArea>();

        audioSource = GetComponent<AudioSource>();
        rottingAudioClip = EnemyBehaviour.LoadAudioClip("Pudge SFX", "Rotting");
    }

    // Update is called once per frame
    void Update()
    {
        if(isRotActive){
            rotVFX.gameObject.SetActive(true);
            rotArea.gameObject.SetActive(true);
        }else{
            rotVFX.gameObject.SetActive(false);
            rotArea.gameObject.SetActive(false);
            StopRottingSFX();
        }
    }

    public void SetIsRotActive(bool value){
        isRotActive = value;
    }

    //SFX
    public void PlayRottingSFX(){
        audioSource.clip = rottingAudioClip;
        audioSource.Play();
    }

    private void StopRottingSFX(){
        audioSource.Stop();
    }
}
