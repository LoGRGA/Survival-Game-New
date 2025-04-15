using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggernautAttack2IllusionBehaviour : JuggernautBehaviour
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        disappearDuration = attack2Duration;
        Attack2();
        StartCoroutine(DestroyWithDelay());
    }
}
