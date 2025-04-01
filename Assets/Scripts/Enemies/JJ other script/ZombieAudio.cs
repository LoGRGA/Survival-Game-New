using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ZombieAudio : MonoBehaviour
{
    public AudioClip zombieFall;
    public AudioClip[] zombieAngry, zombieAttack; // Assign in the inspector
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Fall()
    {
        audioSource.clip = zombieFall;
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void Angry()
    {
        int n = Random.Range(1, zombieAngry.Length);
        audioSource.clip = zombieAngry[n];
        audioSource.PlayOneShot(audioSource.clip);

        zombieAngry[n] = zombieAngry[0];
        zombieAngry[0] = audioSource.clip;
    }

    public void AttackAudio()
    {
        int n = Random.Range(1, zombieAttack.Length);
        audioSource.clip = zombieAttack[n];
        audioSource.PlayOneShot(audioSource.clip);

        zombieAttack[n] = zombieAttack[0];
        zombieAttack[0] = audioSource.clip;
    }
}
