using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggernautAttack3IllusionBehaviour : JuggernautBehaviour
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        disappearDuration = attack3Duration;
        Attack3();
        StartCoroutine(DestroyWithDelay());
    }
}
