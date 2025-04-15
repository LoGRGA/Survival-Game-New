using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggernautAttackIllusionBehaviour : JuggernautBehaviour
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        disappearDuration = attackDuration;
        Attack();
        StartCoroutine(DestroyWithDelay());
    }
}
