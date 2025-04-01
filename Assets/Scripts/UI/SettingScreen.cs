using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingScreen : MonoBehaviour
{
    public AudioMixer theMixer;
    public TMP_Text masterLabel;
    public Slider masterSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMasterVol()
    {
        masterLabel.text = Mathf.RoundToInt (masterSlider.value + 80).ToString();
        theMixer.SetFloat("MasterVol",masterSlider.value);
    }


}

