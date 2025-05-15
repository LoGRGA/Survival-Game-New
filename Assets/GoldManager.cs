using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : MonoBehaviour
{
    public static GoldManager instance;

    public Player player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGold(int amount)
    {
        // Optional: apply multipliers, buffs, or bonuses here
        int finalAmount = amount;

        if (player != null)
        {
            player.ReceiveGold(finalAmount);
        }
    }
}

