using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonerBehaviour : EnemyBehaviour
{
    //add in attack2 animation state
    protected enum ExtendedAnimationState { Attack2 }

    //special attack variable
    protected bool isAttack = true;
    protected bool isAttack2 = false;
    protected float attack2Range = 2.5f;
    protected float attack2Distance = 2.5f;
    protected float attack2Duration = 3.46f;
    protected float attack2WindUpDuration = 0.8f; //from frame 0 to 48
    protected float attack2WindUpDuration2 = 0.75f; //from frame 48 to 93
    protected int attack2Damage = 5;

    //Coroutine variable
    protected Coroutine attack2TimerCoroutine;
    protected Coroutine attack2LogicCoroutine;

    //SFX

    protected void Awake() {
        maxHealth = 150f; // ------------------------------------------------------------------needs to change -------------------------------------------------------------
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hipsTransform = transform.Find("mixamorig:Hips").transform;

        //initiate enemy attributes
        currentHealth = maxHealth;
        hitable = true;
        hitCoolDown = 5f;
        isHitting = false;
        hitDuration = 1.13f;
        disappearDuration = 4.36f;

        attackRange = 2.5f;
        attackDistance = 2.5f;
        attackWindUpDuration = 0.9f;
        dealDamageDuration = 0.15f;

        attackDuration = 2f;
        attackDamage = 5;
        attackRaycastHeight = 0.5f;

        //fov
        detectionRadius = 20f; fov.radius = detectionRadius;
        detectionAngle = 145f; fov.angle = detectionAngle;
        fovRaycastHeight = 0.8f;

        speed = 3.5f; agent.speed = speed;
        rotationSpeed = 10f;

        //roar
        roarDuration = 1.55f;

        //SFX
        roarAudioClip = LoadAudioClip("Prisoner SFX", "Prisoner Roar");
        attackAudioClip = LoadAudioClip("Prisoner SFX", "Prisoner Attack");
        hitAudioClip = LoadAudioClip("Prisoner SFX", "Prisoner Hit");
        dieAudioClip = LoadAudioClip("Prisoner SFX", "Prisoner Die");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //death check
        if(currentHealth <= 0 && !isDying){
            Die();
            // Javier Addition: Give gold to player
            if (GoldManager.instance != null)
            {
                GoldManager.instance.AddGold(10);
            }
            // Javier Addition: ADDING SCORE WHEN ZOMBIE DIES
            if (ScoreManager_new.instance != null)
            {
                ScoreManager_new.instance.AddScore(10); // Adjust points as needed
            }
        }
       
        //attack reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= attackRange && isAttack){
            Attack();
        }
        //attack2 reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= attack2Range && isAttack2){
            Attack2();
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

    //protected override void FixedUpdate() {
    //    base.FixedUpdate();
    //}

    //override the Die() funcction to include stop attack2
    protected override void Die()
    {
        base.Die();
        StopAttackLogic(attack2LogicCoroutine);
    }

    //override the Hit() funcction to include stop attack2
    protected override void Hit()
    {
        base.Hit();
        StopAttack(attack2TimerCoroutine);
        StopAttackLogic(attack2LogicCoroutine);
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
        isAttack = true;
        isAttack2 = false;
    }

    //deal damage to player with time checking (Attack 2)
    protected virtual IEnumerator Attack2Logic(){
        if(alive && isAttacking){
            //deal damage first time
            yield return new WaitForSeconds(attack2WindUpDuration);
            PlaySFX(attackAudioClip);
            isDealingDamage = true;
            StartCoroutine(DealDamageTimer(dealDamageDuration));
            StartCoroutine(DealDamage(attack2Damage, attack2Distance));

            //deal damage second time
            yield return new WaitForSeconds(attack2WindUpDuration2);
            PlaySFX(attackAudioClip);
            isDealingDamage = true;
            StartCoroutine(DealDamageTimer(dealDamageDuration));
            StartCoroutine(DealDamage(attack2Damage, attack2Distance));
        }
    }

    //put the attack2 state inside the dictionary
    protected override Dictionary<object, string> GetExtendedAnimationParameterNames()
    {
        return new Dictionary<object, string>
        {
            { ExtendedAnimationState.Attack2, "Attack2" }
        };
    }
}
