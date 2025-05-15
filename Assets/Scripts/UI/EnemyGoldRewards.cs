using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoldReward : MonoBehaviour
{
    public int goldReward = 10;              // Amount of gold this enemy gives
    public Player player;                    // Reference to Player script (assign in Inspector)
    private bool hasGivenGold = false;       // Prevent double reward

    public void GiveGoldToPlayer()
    {
        if (hasGivenGold || player == null) return;

        hasGivenGold = true;
        player.gold += goldReward;
    }
}



