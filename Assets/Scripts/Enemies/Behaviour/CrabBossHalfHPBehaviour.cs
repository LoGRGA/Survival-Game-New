using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBossHalfHPBehaviour : CrabBossBehaviour
{
    protected override void Start()
    {
        maxHealth /= 2;
        base.Start();
    }
}
