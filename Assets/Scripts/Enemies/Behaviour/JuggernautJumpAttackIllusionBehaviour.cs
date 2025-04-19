using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggernautJumpAttackIllusionBehaviour : JuggernautBehaviour
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        disappearDuration = jumpAttackDuration;
        JumpAttack(false);
        StartCoroutine(DestroyWithDelay());
    }
}
