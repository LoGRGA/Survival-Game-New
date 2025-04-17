using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectiveManager : MonoBehaviour
{
    public GameObject objective1UI;
    public GameObject objective2UI;

    public Image keycardTick;
    public Image safehouseTick;
    public Image boss1Tick;
    public Image boss2Tick;

    private bool keycardCollected = false;
    private bool safehouseReached = false;
    private int bossesKilled = 0;

    void Start()
    {
        objective1UI.SetActive(true);
        objective2UI.SetActive(false);
    }

    public void CollectKeycard()
    {
        keycardCollected = true;
        keycardTick.enabled = true;
    }

    public void ReachSafehouse()
    {
        if (keycardCollected)
        {
            safehouseReached = true;
            safehouseTick.enabled = true;
            StartCoroutine(CompleteObjective1());
        }
    }

    IEnumerator CompleteObjective1()
    {
        yield return new WaitForSeconds(5f);
        objective1UI.SetActive(false);
        objective2UI.SetActive(true);
    }

    public void BossKilled()
    {
        bossesKilled++;
        if (bossesKilled == 1)
            boss1Tick.enabled = true;
        else if (bossesKilled == 2)
            boss2Tick.enabled = true;
    }
}

