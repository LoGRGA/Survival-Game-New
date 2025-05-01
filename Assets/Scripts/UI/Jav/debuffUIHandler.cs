using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuffUIHandler : MonoBehaviour
{
    public PlayerController PlayerController; // Reference to your player controller

    public Image poisonIcon;
    public Image burnIcon;
    public Image bleedIcon;

    void Update()
    {
        if (PlayerController == null) return;

        poisonIcon.gameObject.SetActive(PlayerController.isPoisoned);
        burnIcon.gameObject.SetActive(PlayerController.isBurned);
        bleedIcon.gameObject.SetActive(PlayerController.isBleeding);
    }
}
