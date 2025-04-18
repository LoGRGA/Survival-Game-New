using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTransition : MonoBehaviour
{
    public GameObject stageToHide;
    public GameObject stageToShow;
    public float interactionDistance = 3f;
    public Transform player; // Assign this manually in the Inspector

    void Update()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (stageToHide != null) stageToHide.SetActive(false);
                if (stageToShow != null) stageToShow.SetActive(true);
            }
        }
    }
}



