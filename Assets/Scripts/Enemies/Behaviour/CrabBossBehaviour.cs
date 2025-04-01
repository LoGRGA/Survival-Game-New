using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CrabBossBehaviour : EnemyBehaviour
{
    //add in attack2 animation state
    protected enum ExtendedAnimationState { Attack2, Attack3, Summon }

    //special attack variable
    protected bool isAttack = true;
    protected bool isAttack2 = false;
    protected bool isAttack3 = false;

    protected float attack2Range = 4f;
    protected float attack2Distance = 4f;
    protected float attack2Duration = 1f * 1.2f;
    protected float attack2WindUpDuration = 0.5f * 1.2f; //from frame 0 to 15 
    protected int attack2Damage = 5;

    protected float attack3Range = 5f;
    protected float attack3Distance = 5f;
    protected float attack3Duration = 1f * 1.5f;
    protected float attack3WindUpDuration = 0.5f * 1.5f; //from frame 0 to 15 
    protected int attack3Damage = 10;

    //Coroutine variable
    protected Coroutine attack2TimerCoroutine;
    protected Coroutine attack2LogicCoroutine;
    protected Coroutine attack3TimerCoroutine;
    protected Coroutine attack3LogicCoroutine;

    protected void Awake() {
        maxHealth = 150f; // ------------------------------------------------------------------needs to change -------------------------------------------------------------
        //Attack: 30 fps
        //Hit: 60 fps
    }
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hipsTransform = transform.Find("Armature/Tooth_B_Target_L").transform;

        //initiate enemy attributes
        currentHealth = maxHealth;
        hitable = true;
        hitCoolDown = 5f;
        isHitting = false;
        hitDuration = 0.38f * 3f;
        disappearDuration = 4f;

        attackRange = 4f;
        attackDistance = 4f;
        attackWindUpDuration = 0.5f * 1.2f; // from frme 0 to 15
        dealDamageDuration = 0.1f;

        attackDuration = 1f * 1.2f;
        attackDamage = 5;
        attackRaycastHeight = -1f;

        //fov
        detectionRadius = 20f; fov.radius = detectionRadius;
        detectionAngle = 145f; fov.angle = detectionAngle;
        fovRaycastHeight = 0f;

        speed = 3.5f; agent.speed = speed;
        rotationSpeed = 10f;

        //roar
        roarDuration = 3f / 2f;

        //SFX
        roarAudioClip = LoadAudioClip("CrabBoss SFX", "CrabBoss Roar");
        attackAudioClip = LoadAudioClip("CrabBoss SFX", "CrabBoss Attack");
        hitAudioClip = LoadAudioClip("CrabBoss SFX", "CrabBoss Hit");
        dieAudioClip = LoadAudioClip("CrabBoss SFX", "CrabBoss Die");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        //death check
        if(currentHealth <= 0 && !isDying){
            Die();
        }
        //attack reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= attackRange && isAttack){
            Attack();
        }
        //attack2 reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= attack2Range && isAttack2){
            Attack2();
        }
        //attack3 reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= attack3Range && isAttack3){
            Attack3();
        }
        //Chases reaction check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer){
            Chase();
        }
        //patrolling if nothing happened 
        else if(alive && !isAttacking && !isRoaring && !isHitting && !fov.canSeePlayer){
            Patrol();
            agent.enabled = false;
            isRoared = false;
        }

        //disable the movement when roaring
        if(isDying || isAttacking || isRoaring || isHitting){
            agent.enabled = false;
        }

        //face to player while attacking
        if(isAttacking || isHitting || isRoaring || isFaceToPlayer){
            FaceToPlayer();
        }
    }

    //override the Die() funcction to include stop attack2
    protected override void Die()
    {
        base.Die();

        StopAttackLogic(attack2LogicCoroutine);

        StopAttackLogic(attack3LogicCoroutine);
    }

    //override the Hit() funcction to include stop attack2
    protected override void Hit()
    {
        base.Hit();
        StopAttack(attack2TimerCoroutine);
        StopAttackLogic(attack2LogicCoroutine);

        StopAttack(attack3TimerCoroutine);
        StopAttackLogic(attack3LogicCoroutine);

    }

    //override the Attack() function
    protected override void Attack(){
        base.Attack();
        isAttack = false;
        isAttack2 = true;
    }

    //add in new attack2 function
    protected void Attack2(){
        SetAnimationActive(ExtendedAnimationState.Attack2);
        attack2TimerCoroutine = StartCoroutine(AttackTimer(attack2Duration));
        attack2LogicCoroutine = StartCoroutine(Attack2Logic());
        isAttack2 = false;
        isAttack3 = true;
    }

    //deal damage to player with time checking (Attack 2)
    protected virtual IEnumerator Attack2Logic(){
        if(alive && isAttacking){
            yield return new WaitForSeconds(attack2WindUpDuration);
            PlaySFX(attackAudioClip);
            isDealingDamage = true;
            StartCoroutine(DealDamageTimer(dealDamageDuration));
            StartCoroutine(DealDamage(attack2Damage, attack2Distance));
        }
    }

    //add in new attack3 function
    protected void Attack3(){
        SetAnimationActive(ExtendedAnimationState.Attack3);
        attack3TimerCoroutine = StartCoroutine(AttackTimer(attack3Duration));
        attack3LogicCoroutine = StartCoroutine(Attack3Logic());
        isAttack3 = false;
        isAttack = true;
    }

    //deal damage to player with time checking (Attack 3)
    protected virtual IEnumerator Attack3Logic(){
        if(alive && isAttacking){
            yield return new WaitForSeconds(attack3WindUpDuration);
            PlaySFX(attackAudioClip);
            isDealingDamage = true;
            StartCoroutine(DealDamageTimer(dealDamageDuration));
            StartCoroutine(DealDamage(attack3Damage, attack3Distance));
        }
    }

    //put the attack2 attack3 state inside the dictionary
    protected override Dictionary<object, string> GetExtendedAnimationParameterNames()
    {
        return new Dictionary<object, string>
        {
            { ExtendedAnimationState.Attack2, "Attack2" },
            { ExtendedAnimationState.Attack3, "Attack3" },
            { ExtendedAnimationState.Summon, "Summon" }
        };
    }

}
