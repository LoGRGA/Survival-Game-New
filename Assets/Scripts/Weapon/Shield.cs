using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    public int selectedshield = 3;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedShield = selectedshield;

        if (Input.GetKeyDown(KeyCode.J))
            selectedshield = 0;

        if (Input.GetKeyDown(KeyCode.K))
            selectedshield = 1;

        if (Input.GetKeyDown(KeyCode.L))
            selectedshield = 2;

        if (Input.GetKeyDown(KeyCode.P))
            GetComponentInChildren<ShieldStat>().gameObject.SetActive(false);

        if (previousSelectedShield != selectedshield)
            SwapShield();
    }

    public void SwapShield()
    {
        int i = 0;
        playerController.InvincibleSwap("true");

        foreach (Transform shield in transform)
        {
            if (i == selectedshield)
                shield.gameObject.SetActive(true);
            else
                shield.gameObject.SetActive(false);
            i++;
        }
    }


}
