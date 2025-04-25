using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggernautFlySwordIllusionBehaviour : JuggernautBehaviour
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        disappearDuration = flySwordDuration;
        FlySword(false);
        StartCoroutine(DestroyWithDelay());
    }
}
