using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PudgeHalfHPBehaviour : PudgeBehaviour
{
    protected override void Start()
    {
        maxHealth /= 2;
        base.Start();
    }
}
