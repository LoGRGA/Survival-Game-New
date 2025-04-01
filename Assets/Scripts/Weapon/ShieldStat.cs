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
}
