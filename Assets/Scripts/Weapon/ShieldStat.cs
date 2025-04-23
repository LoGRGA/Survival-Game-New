using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldStat : MonoBehaviour
{
    PlayerController controller;

    public int durability;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BlockDamage(int damage)
    {
        if (durability > 0)
        {
            durability -= damage;
            Debug.Log($"Shield absorbed damage. Durability left: {durability}");

            if (durability <= 0)
            {
                Debug.Log("Shield broke!");
                UnequipShield();
                Destroy(gameObject); // Destroy the shield object
            }
        }
    }

    public void UnequipShield()
    {
        controller.InvincibleSwap("false");
    }

}
