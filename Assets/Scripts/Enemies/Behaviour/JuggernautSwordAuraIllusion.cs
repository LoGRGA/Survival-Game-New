using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggernautSwordAuraIllusion : JuggernautBehaviour
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        disappearDuration = swordAuraDuration;
        SwordAura(false);
        StartCoroutine(DestroyWithDelay());
    }
}
