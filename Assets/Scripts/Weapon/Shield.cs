using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    public int selectedshield = 0;
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

        if (Input.GetKeyDown(KeyCode.Q))
            selectedshield = 0;

        if (Input.GetKeyDown(KeyCode.Alpha9))
            selectedshield = 1;

        if (Input.GetKeyDown(KeyCode.Alpha0))
            GetComponentInChildren<ShieldStat>().gameObject.SetActive(false);

        if (previousSelectedShield != selectedshield)
            SwapShield();
    }

    public void SwapShield()
    {
        int i = 0;

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
