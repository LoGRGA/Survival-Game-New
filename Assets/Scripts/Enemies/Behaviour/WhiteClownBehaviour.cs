using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteClownBehaviour : EnemyBehaviour
{
    //add in attack2 animation state
    protected enum ExtendedAnimationState { Attack2, Hurricane }

    //special attack variable
    protected bool isAttack = true;
    protected bool isAttack2 = false;
    protected float attack2Range = 3f;
    protected float attack2Distance = 3f;
    protected float attack2Duration = 2f;
    protected float attack2WindUpDuration = 0.6f; //from frame 0 to 36
    protected int attack2Damage = 5;

    //special hurricane variable
    protected float hurricaneRange = 4f;
    protected float hurricaneDuration = 1.81f;
    protected float hurricaneDealDamageDuration = 0.05f;
    protected float hurricaneWindUpDuration = 0.4f; //from frame 0 to 24
    protected float hurricaneWindUpDuration2 = 0.4667f; //from frame 24 to 52
    protected float hurricaneWindUpDuration3 = 0.45f; //from frame 52 to 79
    protected float hurricaneWindUpDuration4 = 0.45f; //from frame 79 to 106

    protected float hurricaneCoolDown = 5f;
    protected bool isHurricaneCoolDown = false;
    protected int hurricaneDamage = 5;
    protected float hurricaneDistance = 3f;
    protected float hurricanMoveSpeed = 3f;

    protected LayerMask obstacleLayerMask;


    //Coroutine variable
    protected Coroutine attack2TimerCoroutine;
    protected Coroutine attack2LogicCoroutine;
    protected Coroutine hurricaneTimerCoroutine;
    protected Coroutine hurricaneLogicCoroutine;

    //SFX
    protected AudioClip attack2AudioClip;
    protected AudioClip hurricaneAudioClip;

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
        disappearDuration = 3f;

        attackRange = 2.5f;
        attackDistance = 2.5f;
        attackWindUpDuration = 0.4f; //from frame 0 to 24
        dealDamageDuration = 0.15f; //from frame 24 to 33

        attackDuration = 0.97f;
        attackDamage = 5;
        attackRaycastHeight = 0.3f;

        obstacleLayerMask = LayerMask.GetMask("Obstacle");

        //fov
        detectionRadius = 20f; fov.radius = detectionRadius;
        detectionAngle = 145f; fov.angle = detectionAngle;
        fovRaycastHeight = 0.8f;

        speed = 4f; agent.speed = speed;
        rotationSpeed = 10f;

        //roar
        roarDuration = 0.97f;

        //SFX
        roarAudioClip = LoadAudioClip("WhiteClown SFX", "WhiteClown Roar");
        attackAudioClip = LoadAudioClip("WhiteClown SFX", "WhiteClown Attack");
        hitAudioClip = LoadAudioClip("WhiteClown SFX", "WhiteClown Hit");
        dieAudioClip = LoadAudioClip("WhiteClown SFX", "WhiteClown Die");

        attack2AudioClip = LoadAudioClip("WhiteClown SFX", "WhiteClown Attack2");
        hurricaneAudioClip = LoadAudioClip("WhiteClown SFX", "WhiteClown Hurricane");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //death check
        if(currentHealth <= 0 && !isDying){
            Die();
            // Javier Addition: ADDING SCORE WHEN ZOMBIE DIES
            if (ScoreManager_new.instance != null)
            {
                ScoreManager_new.instance.AddScore(100); // Adjust points to preference
            }
        }
        ////hurricane reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= attackRange && !isHurricaneCoolDown){
            Hurricane();
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

    //override the Die() funcction to include stop attack2
    protected override void Die()
    {
        base.Die();
        StopAttackLogic(attack2LogicCoroutine);
        StopAttackLogic(hurricaneLogicCoroutine);
    }

    //override the Hit() funcction to include stop attack2
    protected override void Hit()
    {
        base.Hit();
        StopAttack(attack2TimerCoroutine);
        StopAttackLogic(attack2LogicCoroutine);

        StopAttack(hurricaneTimerCoroutine);
        StopAttackLogic(hurricaneLogicCoroutine);
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
            PlaySFX(attack2AudioClip);
            isDealingDamage = true;
            StartCoroutine(DealDamageTimer(dealDamageDuration));
            StartCoroutine(DealDamage(attack2Damage, attack2Distance));
        }
    }

    //add in new hurricane function
    protected void Hurricane(){
        //SFX
        PlaySFX(hurricaneAudioClip);

        SetAnimationActive(ExtendedAnimationState.Hurricane);
        hurricaneTimerCoroutine = StartCoroutine(AttackTimer(hurricaneDuration));
        hurricaneLogicCoroutine = StartCoroutine(HurricaneLogic());
        
        //start count hurricane cooldown
        StartCoroutine(HurricaneCoolDownTimer());

        //move froward
        StartCoroutine(HurricaneMoveForward());
    }

    protected IEnumerator HurricaneLogic(){
        //deal damage first time
        yield return new WaitForSeconds(hurricaneWindUpDuration);
        //PlaySFX(attack2AudioClip);
        isDealingDamage = true;
        StartCoroutine(DealDamageTimer(hurricaneDealDamageDuration));
        StartCoroutine(DealDamage(hurricaneDamage, hurricaneDistance));

        //deal damage second time
        yield return new WaitForSeconds(hurricaneWindUpDuration2);
        //PlaySFX(attack2AudioClip);
        isDealingDamage = true;
        StartCoroutine(DealDamageTimer(hurricaneDealDamageDuration));
        StartCoroutine(DealDamage(hurricaneDamage, hurricaneDistance));

        //deal damage third time
        yield return new WaitForSeconds(hurricaneWindUpDuration3);
        //PlaySFX(attack2AudioClip);
        isDealingDamage = true;
        StartCoroutine(DealDamageTimer(hurricaneDealDamageDuration));
        StartCoroutine(DealDamage(hurricaneDamage, hurricaneDistance));

        //deal damage fourth time
        yield return new WaitForSeconds(hurricaneWindUpDuration4);
        //PlaySFX(attack2AudioClip);
        isDealingDamage = true;
        StartCoroutine(DealDamageTimer(hurricaneDealDamageDuration));
        StartCoroutine(DealDamage(hurricaneDamage, hurricaneDistance));
    }

    protected IEnumerator HurricaneCoolDownTimer(){
        isHurricaneCoolDown = true;
        yield return new WaitForSeconds(hurricaneCoolDown);
        isHurricaneCoolDown = false;
    }

    protected IEnumerator HurricaneMoveForward(){
        float time = 0f;

        Vector3 hurricaneDirection = (playerTransform.position - transform.position).normalized;
        hurricaneDirection.y = 0;

        while(time <= hurricaneDuration && !IsTouchObstacle() && isAttacking){
            transform.position += hurricaneDirection * hurricanMoveSpeed * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }
    }

    protected bool IsTouchObstacle()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.5f, obstacleLayerMask);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }


    //put the attack2 state inside the dictionary
    protected override Dictionary<object, string> GetExtendedAnimationParameterNames()
    {
        return new Dictionary<object, string>
        {
            { ExtendedAnimationState.Attack2, "Attack2" },
            { ExtendedAnimationState.Hurricane, "Hurricane" }
        };
    }
}
