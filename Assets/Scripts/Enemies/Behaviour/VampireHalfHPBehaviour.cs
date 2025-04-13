using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireHalfHPBehaviour : VampireBehaviour
{
    protected override void Start()
    {
        maxHealth /= 2;
        base.Start();
    }
}
