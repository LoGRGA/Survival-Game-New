using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    public PlayerController controller;

    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 25;

    public GameObject hitEffect;
    public AudioClip swordSwing;
    public AudioClip hitSound;

    protected virtual void Start()
    {
        controller = GetComponentInParent<PlayerController>();
    }

    // onEnable is called upon being active
    protected virtual void OnEnable()
    {
        controller.attackDelay = attackDelay;
        controller.attackSpeed = attackSpeed;
        controller.attackDamage = attackDamage;

        controller.hitEffect = hitEffect;
        controller.swordSwing = swordSwing;
        controller.hitSound = hitSound;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
