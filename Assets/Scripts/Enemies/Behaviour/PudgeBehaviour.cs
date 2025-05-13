using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PudgeBehaviour : EnemyBehaviour
{
    //add in attack2 animation state
    protected enum ExtendedAnimationState { Attack2, Attack3, Rot, ReleaseHook, RetractHook }

    //special attack variable
    protected bool isAttack = true;
    protected bool isAttack2 = false;
    protected bool isAttack3 = false;

    protected float attack2Range = 4f;
    protected float attack2Distance = 4f;
    protected float attack2Duration = 3.15f;
    protected float attack2WindUpDuration = 1f; //from frame 0 to 60 
    protected int attack2Damage = 15;

    protected float attack3Range = 4f;
    protected float attack3Distance = 4f;
    protected float attack3Duration = 4.18f;
    protected float attack3WindUpDuration = 0.95f; //from frame 0 to 57
    protected float attack3WindUpDuration2 = 0.55f; //from frame 57 to 90
    protected float attack3WindUpDuration3 = 1.1f; //from frame 90 to 156
    protected int attack3Damage = 5;
    protected int attack3Damage3 = 10;

    //special Rot variable
    public Rot rot;
    protected bool isRot = false;
    protected bool isRotting = false;
    protected bool isCastingRot = false;
    protected float castRotDuration = 2f;
    protected float rotDuration = 10f;

    //special hook variavle
    public MeatHook meatHook;
    protected Transform hookParent;
    protected Vector3 hookOriginalPosition;
    protected Quaternion hookOriginalRotaion;
    protected Vector3 hookLocalPosition;
    protected Quaternion hookLocalRotaion;
    protected Vector3 hookReleaseDirection;
    protected Vector3 hookRetractDirection;
    protected float hookCooldDown = 5f;
    protected bool isHookCoolDown = false;
    protected bool isReleasingHook = false;
    protected bool isRetractingHook = false;
    protected float releaseHookMaxDuration = 2f;
    protected float releaseHookWindUpDuration = 1f;
    protected float hookFlySpeed = 30f;

    //Coroutine variable
    protected Coroutine attack2TimerCoroutine;
    protected Coroutine attack2LogicCoroutine;
    protected Coroutine attack3TimerCoroutine;
    protected Coroutine attack3LogicCoroutine;

    protected Coroutine releaseHookCoroutine;
    protected Coroutine releaseHookLogicCoroutine;

    protected Coroutine rotTimerCoroutine;
    
    //SFX
    protected AudioSource[] audioSources;
    protected AudioSource audioSource2;
    protected AudioClip attack3_3AudioClip;
    protected AudioClip hookingAudioClip;
    protected AudioClip releaseHookAudioClip;
    protected AudioClip rotAudioClip;
    

    protected virtual void Awake() {
        maxHealth = 600f; // ------------------------------------------------------------------needs to change -------------------------------------------------------------
        //Added for testing, will remove after stage 3 boss done
        currentHealth = maxHealth;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hipsTransform = transform.Find("Root/Hips").transform;
        //initiate enemy attributes
        currentHealth = maxHealth;
        hitable = true;
        hitCoolDown = 10f;
        isHitting = false;
        hitDuration = 1.55f;
        disappearDuration = 5.73f;

        attackRange = 4f;
        attackDistance = 4f;
        attackWindUpDuration = 0.95f; //from frame 0 to 57
        dealDamageDuration = 0.15f;

        attackDuration = 2.38f;
        attackDamage = 10;
        attackRaycastHeight = 1f;

        //fov
        detectionRadius = 30f; fov.radius = detectionRadius;
        detectionAngle = 145f; fov.angle = detectionAngle;
        fovRaycastHeight = 1.5f;

        speed = 3f; agent.speed = speed;
        rotationSpeed = 10f;

        //roar
        roarDuration = 3.58f;

        //SFX
        audioSources = GetComponents<AudioSource>();
        audioSource = audioSources[0];
        audioSource2 = audioSources[1];

        roarAudioClip = LoadAudioClip("Pudge SFX", "Pudge Roar");
        attackAudioClip = LoadAudioClip("Pudge SFX", "Pudge Attack");
        hitAudioClip = LoadAudioClip("Pudge SFX", "Pudge Hit");
        dieAudioClip = LoadAudioClip("Pudge SFX", "Pudge Die");

        attack3_3AudioClip = LoadAudioClip("Pudge SFX", "Pudge Attack3");
        hookingAudioClip = LoadAudioClip("Pudge SFX", "Hooking");
        releaseHookAudioClip = LoadAudioClip("Pudge SFX", "Pudge Release Hook");

        rotAudioClip = LoadAudioClip("Pudge SFX", "Pudge Rot");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        //death check
        if(currentHealth <= 0 && !isDying){
            Die();
            //Javier Addition: Handles Boss death for objective
            GetComponent<BossKillTracker>().HandleBossDeath();
            // Javier Addition: ADDING SCORE WHEN ZOMBIE DIES
            if (ScoreManager_new.instance != null)
            {
                ScoreManager_new.instance.AddScore(1000); // Adjust points as needed
            }
        }
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && !isRoared && !isRoarCoolDown){
            TryRoar();
        }
        //attack reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= attackRange && isAttack && !isReleasingHook && !isRetractingHook && !isCastingRot){
            Attack();
        }
        //attack2 reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= attack2Range && isAttack2 && !isReleasingHook && !isRetractingHook){
            Attack2();
        }
        //attack3 reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= attack3Range && isAttack3 && !isReleasingHook && !isRetractingHook){
            Attack3();
        }
        //rot reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= attackRange && isRot && !isReleasingHook && !isRetractingHook){
            Rot();
        }
        //hook reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && !isReleasingHook && !isRetractingHook && !isHookCoolDown && !isCastingRot){
            Hook();
            agent.enabled = false;
        }
        //Chases reaction check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && !isReleasingHook && !isRetractingHook && !isCastingRot){
            Chase();
        }
        //patrolling if nothing happened 
        else if(alive && !isAttacking && !isRoaring && !isHitting && !fov.canSeePlayer){
            Patrol();
            agent.enabled = false;
            isRoared = false;
        }

        //disable the movement when roaring
        if(isDying || isAttacking || isRoaring || isHitting || isCastingRot){
            agent.enabled = false;
        }

        //face to player while attacking
        if(isAttacking || isHitting || isRoaring || isFaceToPlayer){
            FaceToPlayer();
        }
    }

    //protected override void FixedUpdate()
    //{
    //    base.FixedUpdate();
    //}

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
        if(!isReleasingHook && !isRetractingHook && !isCastingRot){
            base.Hit();
            StopAttack(attack2TimerCoroutine);
            StopAttackLogic(attack2LogicCoroutine);

            StopAttack(attack3TimerCoroutine);
            StopAttackLogic(attack3LogicCoroutine);
        }
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

    //add in new attack2 function
    protected void Attack3(){
        SetAnimationActive(ExtendedAnimationState.Attack3);
        attack3TimerCoroutine = StartCoroutine(Attack3Timer());
        attack3LogicCoroutine = StartCoroutine(Attack3Logic());
        isAttack3 = false;
        isRot = true;
    }

    //controll isAttacking variable based on attackDuration
    protected virtual IEnumerator Attack3Timer(){
        isAttacking = true;
        yield return new WaitForSeconds(attack3Duration);
        isAttacking = false;
    }

    //deal damage to player with time checking (Attack 3)
    protected virtual IEnumerator Attack3Logic(){
        if(alive && isAttacking){
            //deal damage first time
            yield return new WaitForSeconds(attack3WindUpDuration);
            PlaySFX(attackAudioClip);
            isDealingDamage = true;
            StartCoroutine(DealDamageTimer(dealDamageDuration));
            StartCoroutine(DealDamage(attack3Damage, attack3Distance));

            //deal damage second time
            yield return new WaitForSeconds(attack3WindUpDuration2);
            PlaySFX(attackAudioClip);
            isDealingDamage = true;
            StartCoroutine(DealDamageTimer(dealDamageDuration));
            StartCoroutine(DealDamage(attack3Damage, attack3Distance));

            //deal damage third time
            yield return new WaitForSeconds(attack3WindUpDuration3);
            PlaySFX(attack3_3AudioClip);
            isDealingDamage = true;
            StartCoroutine(DealDamageTimer(dealDamageDuration));
            StartCoroutine(DealDamage(attack3Damage3, attack3Distance));
        }
    }

    protected void Rot(){
        //rot animation
        SetAnimationActive(ExtendedAnimationState.Rot);

        //rot SFX
        PlaySFX(rotAudioClip);

        //Rot logic
        StartCoroutine(RotLogic());
        
        //enable first attack
        isAttack = true;
        isRot = false;
    }

    protected IEnumerator RotLogic(){
        //set isCasting rot to true
        isCastingRot = true;

        //stop the previous rot time if pudge is rotting
        if(isRotting){
            StopCoroutine(rotTimerCoroutine);
        }

        //let rot enable it's children
        rot.SetIsRotActive(true);

        //start to count the rot duration
        rotTimerCoroutine = StartCoroutine(RotTimer());

        //set isCastingRot to false after cast animation done
        yield return new WaitForSeconds(castRotDuration);
        isCastingRot = false;

        //let rot play rot SFX
        rot.PlayRottingSFX();

    }

    protected IEnumerator RotTimer(){
        isRotting = true;
        yield return new WaitForSeconds(rotDuration);
        isRotting = false;
        rot.SetIsRotActive(false);
    }

    protected void Hook(){
        hookOriginalPosition = meatHook.transform.position;
        hookOriginalRotaion = meatHook.transform.rotation;
        SetAnimationActive(ExtendedAnimationState.ReleaseHook);
        releaseHookCoroutine = StartCoroutine(ReleaseHookTimer());
        releaseHookLogicCoroutine = StartCoroutine(ReleaseHookLogic());
        
    }

    protected IEnumerator ReleaseHookTimer(){
        isReleasingHook = true;
        yield return new WaitForSeconds(releaseHookMaxDuration);
        isReleasingHook = false;
        isRetractingHook = true;
    }


    protected IEnumerator ReleaseHookLogic(){
        //play release hook SFX
        PlaySFX(releaseHookAudioClip);

        //Set MeatHook able to deal damage
        meatHook.SetIsAbleDealDamage(true);

        //record the current hook parent, local postion and rotaion
        hookParent = meatHook.transform.parent;
        hookLocalPosition = meatHook.transform.localPosition;
        hookLocalRotaion = meatHook.transform.localRotation;

        //try to face to player while before release hook
        StartCoroutine(IsFaceToPlayer());

        //release hook windup
        yield return new WaitForSeconds(releaseHookWindUpDuration);

        //set the meat hook able to hook player
        meatHook.SetIsHookable(true);

        //play the SFX after wind-up
        PlayHookSFX();

        //remove the hook from the parent gameObject
        meatHook.transform.SetParent(null);

        //calculate the hook fly direction and rotation
        hookReleaseDirection = (playerTransform.position + Vector3.up - meatHook.transform.position).normalized;
        hookRetractDirection = (meatHook.transform.position - hookOriginalPosition).normalized;

        Quaternion lookRotaion = Quaternion.LookRotation(hookReleaseDirection);
        Quaternion additionalRotation = Quaternion.Euler(0,-90,0);
        meatHook.transform.rotation = lookRotaion * additionalRotation;

        while(isReleasingHook && !isRetractingHook){
            ReleaseHook();
            yield return null;
        }

        SetAnimationActive(ExtendedAnimationState.RetractHook);
        
        while(isRetractingHook){
            RetractHook();
            yield return null;
        }
       

    }

    protected void ReleaseHook(){
        meatHook.transform.position += hookReleaseDirection * hookFlySpeed * Time.deltaTime;

    }

    protected void RetractHook(){
        isReleasingHook = false;
        isRetractingHook = true;
        meatHook.transform.position += -hookReleaseDirection * hookFlySpeed * Time.deltaTime;
        if(Vector3.Distance(meatHook.transform.position, hookOriginalPosition) <= 2f){
            isRetractingHook = false;
            meatHook.SetIsHookable(false);
            meatHook.SetIsHookPlayer(false);

            //stop the release timer coroutine while retracting hook
            StopCoroutine(releaseHookCoroutine);

            //put meat hook back to pudge's hand
            meatHook.transform.SetParent(hookParent);
            meatHook.transform.localPosition = hookLocalPosition;
            meatHook.transform.localRotation = hookLocalRotaion;

            //start to count the meat hook cooldown time
            StartCoroutine(HookCoolDownTimer());
            
            //stop the hook sfx
            StopHookSFX();
        }
    }

    protected IEnumerator HookCoolDownTimer(){
        isHookCoolDown = true;
        yield return new WaitForSeconds(hookCooldDown);
        isHookCoolDown = false;
    }

    public void SetIsRetractingHook(bool value){
        isRetractingHook = value;
    }

    //SFX
    protected void PlayHookSFX(){
        audioSource2.clip = hookingAudioClip;
        audioSource2.Play();
    }

    protected void StopHookSFX(){
        audioSource2.clip = hookingAudioClip;
        audioSource2.Stop();
    }

    //put the attack2 attack3 state inside the dictionary
    protected override Dictionary<object, string> GetExtendedAnimationParameterNames()
    {
        return new Dictionary<object, string>
        {
            { ExtendedAnimationState.Attack2, "Attack2" },
            { ExtendedAnimationState.Attack3, "Attack3" },
            { ExtendedAnimationState.Rot, "Rot" },
            { ExtendedAnimationState.ReleaseHook, "ReleaseHook" },
            { ExtendedAnimationState.RetractHook, "RetractHook" }
        };
    }
}
