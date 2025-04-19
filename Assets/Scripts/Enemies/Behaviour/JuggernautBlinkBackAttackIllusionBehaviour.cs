using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggernautBlinkBackAttackIllusionBehaviour : JuggernautBehaviour
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        disappearDuration = blinkBackAttackDuration;
        BlinkBackAttack();
        StartCoroutine(DestroyWithDelay());
    }
}
