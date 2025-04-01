using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : WeaponStats
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void SpecialAttack()
    {
        Throw();
    }

    void Throw()
    {

    }
}
