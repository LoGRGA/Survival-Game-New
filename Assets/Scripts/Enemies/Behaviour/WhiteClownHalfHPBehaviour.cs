using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteClownHalfHPBehaviour : WhiteClownBehaviour
{
    protected override void Start()
    {
        maxHealth /= 2;
        base.Start();
    }
}
