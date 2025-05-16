using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioLinkSettings : MonoBehaviour
{
    public static AudioLinkSettings Instance;
    public AudioMixerGroup masterGroup;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}