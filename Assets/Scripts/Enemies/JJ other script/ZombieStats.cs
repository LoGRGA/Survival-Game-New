/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieStats : Shootable
{
    [SerializeField] ZombieController controller;
    [SerializeField] private int damage;
    public float attackSpeed;
    private Animator anim;
    private int random;
    private CapsuleCollider collider;

    private GameObject spawnerObject;
    private EnemySpawner spawner;

    [SerializeField] EnemyHealthBar healthBar;

    public void Awake()
    {
        healthBar = GetComponentInChildren<EnemyHealthBar>();
    }

    public bool checkDead()
    {
        return isDead;
    }

    // Damaging function
    public void DealDamage(Shootable damageStats)
    {
        damageStats.SetHealth(damage);
    }

    public override void Dead()
    {
        controller.canAttack = false;
        base.Dead();
        gameObject.tag = "Untagged";
        collider.enabled = false;

        spawner.ReduceCount();
        random = Random.Range(0, 10);
        if (random <= 8)
            anim.SetTrigger("Backward");
        else
            anim.SetTrigger("Forward");

        // Handle object destruction or deactivation
        Destroy(gameObject, 2f);
    }

    public override void SetHealth(int damage)
    {
        base.SetHealth(damage);
        healthBar.UpdateHealthBar(healthAfterDamage, maxHealth);
    }

    public override void InitVariables()
    {
        // Health Init
        maxHealth = 10;
        SetHealthTo(maxHealth);
        isDead = false;

        // Damage Init
        damage = 5;
        attackSpeed = 1.5f;

        collider = GetComponent<CapsuleCollider>();
        anim = GetComponentInChildren<Animator>();
        spawnerObject = GameObject.FindGameObjectWithTag("Spawner");
        spawner = spawnerObject.GetComponent<EnemySpawner>();

    }
}*/
