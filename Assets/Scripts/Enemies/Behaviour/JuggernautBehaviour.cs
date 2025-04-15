using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggernautBehaviour : EnemyBehaviour
{
    //add in attack2 animation state
    protected enum ExtendedAnimationState { Attack2, Attack3}

    //Special Attack vaaraible
    protected bool isAttack = true;
    protected bool isAttack2 = false;
    protected bool isAttack3 = false;

    protected float attackWindUpDuration2 = 0.4f; //from 9 to 21, 30fps

    protected float attack2Range = 9f;
    protected float attack2Distance = 4f;
    protected float attack2Duration = 2.46f; //24fps
    protected float attack2WindUpDuration = 0.5f; //from 0 to 12, 24fps
    protected int attack2Damage = 5;
    protected float attack2MoveDistance = 5f;

    protected float attack3Range = 14f;
    protected float attack3Distance = 4f;
    protected float attack3Duration = 2.375f; //24fps
    protected float attack3WindUpDuration = 0.625f; //from 0 to 15, 24fps
    protected int attack3Damage = 10;
    protected float attack3MoveDistance = 10f;


    //illusion gameobjects
    public GameObject attackIllusion;
    public GameObject attack2Illusion;
    public GameObject attack3Illusion;

    //Coroutine variable
    protected Coroutine generateIllusionCoroutine;
    protected Coroutine attack2TimerCoroutine;
    protected Coroutine attack2LogicCoroutine;
    protected Coroutine attack3TimerCoroutine;
    protected Coroutine attack3LogicCoroutine;

    //SFX
    protected AudioSource[] audioSources;
    protected AudioSource audioSource2;
    protected AudioClip attack2AudioClip;
    protected AudioClip attack3AudioClip;


    protected void Awake() {
        maxHealth = 300f; // ------------------------------------------------------------------needs to change -------------------------------------------------------------
    }
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hipsTransform = transform.Find("root/pelvis").transform;

        //initiate enemy attributes
        currentHealth = maxHealth;
        hitable = true;
        hitCoolDown = 5f;
        isHitting = false;
        hitDuration = 1.5f;
        disappearDuration = 5f;

        attackRange = 4f;
        attackDistance = 4f;
        attackWindUpDuration = 0.3f; //from 0 to 9, 30fps
        dealDamageDuration = 0.1f; //from 9 to 12, 30fps

        attackDuration = 1.833f;
        attackDamage = 5;
        attackRaycastHeight = 0.3f;

        //fov
        detectionRadius = 20f; fov.radius = detectionRadius;
        detectionAngle = 145f; fov.angle = detectionAngle;
        fovRaycastHeight = 0.5f;

        speed = 5f; agent.speed = speed;
        rotationSpeed = 10f;

        //roar
        roarDuration = 2.183f;

        //SFX
        audioSources = GetComponents<AudioSource>();
        audioSource = audioSources[0];
        audioSource2 = audioSources[1];

        roarAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Roar");
        attackAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Attack");
        hitAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Hit");
        dieAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Die");
        
        attack2AudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Attack");;
        attack3AudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Attack");;
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
            if(currentHealth <= maxHealth/2){
                AttackWithIllusion();
            }else{
                Attack();
            } 
        }
        //attack2 reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= attack2Range && isAttack2){
           if(currentHealth <= maxHealth/2){
                Attack2WithIllusion();
            }else{
                Attack2();
            } 
        }
        ///attack3 reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= attack3Range && isAttack3){
            if(currentHealth <= maxHealth/2){
                Attack3WithIllusion();
            }else{
                Attack3();
            } 
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

        if(generateIllusionCoroutine != null){StopCoroutine(generateIllusionCoroutine);}

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

    //attack with generate illusion function
    protected void AttackWithIllusion(){
        base.Attack();
        isAttack = false;
        isAttack2 = true;
        generateIllusionCoroutine = StartCoroutine(GenerateIllusion(attackIllusion, attackDuration));
    }

    //override the normal attack logic
    protected override IEnumerator AttackLogic()
    {
       if(alive && isAttacking){
            //deal damage first time
            yield return new WaitForSeconds(attackWindUpDuration);
            PlaySFX(attackAudioClip);
            isDealingDamage = true;
            StartCoroutine(DealDamageTimer(dealDamageDuration));
            StartCoroutine(DealDamage(attackDamage, attackDistance));

            //deal damage second time
            yield return new WaitForSeconds(attackWindUpDuration2);
            PlaySFX(attackAudioClip);
            isDealingDamage = true;
            StartCoroutine(DealDamageTimer(dealDamageDuration));
            StartCoroutine(DealDamage(attackDamage, attackDistance));
       }
    }

    //add in new attack2 function
    protected void Attack2(){
        SetAnimationActive(ExtendedAnimationState.Attack2);
        attack2TimerCoroutine = StartCoroutine(AttackTimer(attack2Duration));
        attack2LogicCoroutine = StartCoroutine(Attack2Logic());
        isAttack2 = false;
        isAttack3 = true;
    }

    //attack with generate illusion function
    protected void Attack2WithIllusion(){
        Attack2();
        isAttack2 = false;
        isAttack3 = true;
        generateIllusionCoroutine = StartCoroutine(GenerateIllusion2(attack2Illusion, attack2Duration));
    }

    //deal damage to player with time checking (Attack 2)
    protected IEnumerator Attack2Logic(){
        yield return new WaitForSeconds(attack2WindUpDuration);
        PlaySFX(attack2AudioClip);
        isDealingDamage = true;
        StartCoroutine(DealDamageTimer(dealDamageDuration));
        StartCoroutine(DealDamage(attack2Damage, attack2Distance));
        StartCoroutine(SmoothMoveForward(attack2MoveDistance, dealDamageDuration));
    }

    //add in new attack2 function
    protected void Attack3(){
        SetAnimationActive(ExtendedAnimationState.Attack3);
        attack3TimerCoroutine = StartCoroutine(AttackTimer(attack3Duration));
        attack3LogicCoroutine = StartCoroutine(Attack3Logic());
        isAttack3 = false;
        isAttack = true;
    }

    //attack with generate illusion function
    protected void Attack3WithIllusion(){
        Attack3();
        isAttack3 = false;
        isAttack = true;
        generateIllusionCoroutine = StartCoroutine(GenerateIllusion2(attack3Illusion, attack3Duration));
    }

    //deal damage to player with time checking (Attack 2)
    protected IEnumerator Attack3Logic(){
        yield return new WaitForSeconds(attack3WindUpDuration);
        PlaySFX(attack3AudioClip);
        isDealingDamage = true;
        StartCoroutine(DealDamageTimer(dealDamageDuration));
        StartCoroutine(DealDamage(attack3Damage, attack3Distance));
        StartCoroutine(SmoothMoveForward(attack3MoveDistance, dealDamageDuration));
    }
    
    //function that controll juugernaut move forward when use some attacks
    protected IEnumerator SmoothMoveForward(float distance, float duration){
        Vector3 startingPosition = transform.position;
        Vector3 targetPosition = transform.position + transform.forward * distance;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ensure the final position is correct
        transform.position = targetPosition;
    }

    //function  that will generate illusion to repeat attack
    protected IEnumerator GenerateIllusion(GameObject gameObject, float delayTime){
        yield return new WaitForSeconds(delayTime);
        Instantiate(gameObject, playerTransform.position + playerTransform.forward * 2f, transform.rotation);
    }

    //function  that will generate illusion to repeat attack
    protected IEnumerator GenerateIllusion2(GameObject gameObject, float delayTime){
        delayTime = UnityEngine.Random.Range(delayTime - 0.5f, delayTime + 0.5f);
        yield return new WaitForSeconds(delayTime);
        Instantiate(gameObject, transform.position, transform.rotation);
    }

    //put the attack2 attack3 state inside the dictionary
    protected override Dictionary<object, string> GetExtendedAnimationParameterNames()
    {
        return new Dictionary<object, string>
        {
            { ExtendedAnimationState.Attack2, "Attack2" },
            { ExtendedAnimationState.Attack3, "Attack3" }
        };
    }


}
