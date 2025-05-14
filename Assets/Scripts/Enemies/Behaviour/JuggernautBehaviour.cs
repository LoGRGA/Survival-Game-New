using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class JuggernautBehaviour : EnemyBehaviour
{
    //add in attack2 animation state
    protected enum ExtendedAnimationState { Attack2, Attack3, Rage, BlinkBackAttack, JumpAttack, FlySword, SwordAura}

    //Special Attack varaible
    protected bool isAttack = true;
    protected bool isAttack2 = false;
    protected bool isAttack3 = false;
    protected bool isResetPosition = false;

    protected float attackWindUpDuration2 = 0.4f; //from 9 to 21, 30fps

    protected float attack2Range = 10f;
    protected float attack2Distance = 4f;
    protected float attack2Duration = 2.46f; //24fps
    protected float attack2WindUpDuration = 0.5f; //from 0 to 12, 24fps
    protected int attack2Damage = 10;
    protected float attack2MoveDistance = 4f;

    protected float attack3Range = 10f;
    protected float attack3Distance = 4f;
    protected float attack3Duration = 2.375f; //24fps
    protected float attack3WindUpDuration = 0.625f; //from 0 to 15, 24fps
    protected int attack3Damage = 10;
    protected float attack3MoveDistance = 8f;

    //Melee attack skill
    protected float meleeAttackSkillAttackRange = 15f;
    protected float meleeAttackSkillCoolDown = 10f;
    protected bool isMeleeAttackSkillCoolDown = false;

    //BlinkBackAttack variable
    protected float blinkBackAttackDistance = 3f;
    protected float blinkBackAttackDuration = 2.4666666f; //30fps
    protected float blinkBackAttackBlinkWindUpDuration = 0.7f; // from 0 to 21, 30fps
    protected float blinkBackAttackWindUpDuration = 0.3f; //from 21 to 30, 30fps
    protected int blinkBackAttackDamage = 15;
    protected bool isBlinkBackAttack = true;

    //JumpAttack variable
    protected float jumpAttackDistance = 3f;
    protected float jumpAttackDuration = 2.3f; 
    protected float jumpAttackWindUpDuration = 0.467f; //from 0 to 14, 30fps
    protected int jumpAttackDamage = 15;
    protected bool isJumpAttack = false;
    protected float jumpAttackJumpHeight = 3f;

    //range attack skill
    protected float rangeAttackSkillAttackRange = 20f;
    protected float rangeAttackSkillCoolDown = 15f;
    protected bool isRangeAttackSkillCoolDown = false;
    protected bool isCastingRangeSkill = false;

    //Fly sword variables
    protected float flySwordDuration = 2.65f;
    protected float flySwordWindUpDuration = 1f; // from 0 tp 60, 60 fps
    protected bool isFlySword = true;
    protected Transform swordSummonTrans;
    public GameObject flySword;

    //Sword Aura variables
    protected float swordAuraDuration = 2.933333f; 
    protected float swordAuraWindUpDuration = 1.75f; // from 0 to 105, 60fps
    protected bool isSwordAura = false;
    public GameObject swordAura;
    public GameObject juggernautSword;
    
    //Rage variables
    protected bool isRaged = false;
    protected bool isRaging = false;
    protected float rageDuration = 2.33333f;
    public GameObject rageVFX;

    //illusion gameobjects
    public GameObject attackIllusion;
    public GameObject attack2Illusion;
    public GameObject attack3Illusion;
    public GameObject blinkBackAttackIllusion;
    public GameObject jumpAttackIllusion;
    public GameObject flySowordIllusion;
    public GameObject swordAuraIllusion;

    //Coroutine variable
    protected Coroutine generateIllusionCoroutine;
    protected Coroutine attack2TimerCoroutine;
    protected Coroutine attack2LogicCoroutine;
    protected Coroutine attack3TimerCoroutine;
    protected Coroutine attack3LogicCoroutine;

    protected Coroutine meleeAttackSkillCoolDownCoroutine;
    protected Coroutine blinkBackAttackTimerCoroutine;
    protected Coroutine blinkBackAttackLogicCoroutine;
    protected Coroutine jumpAttackTimerCoroutine;
    protected Coroutine jumpAttackLogicCoroutine;

    //SFX
    protected AudioSource[] audioSources;
    protected AudioSource audioSource2;
    protected AudioClip attackSpeechAudioClip;
    protected AudioClip attack2AudioClip;
    protected AudioClip attack2SpeechAudioClip;
    protected AudioClip attack3AudioClip;
    protected AudioClip attack3SpeechAudioClip;

    protected AudioClip rageAudioClip;

    protected AudioClip blinkBackAttackBlinkAudioClip;
    protected AudioClip blinkBackAttackAudioClip;
    protected AudioClip jumpAttackAudioClip;
    protected AudioClip jumpAttackSpeechAudioClip;

    protected AudioClip flySwordSpeechAudioClip;
    protected AudioClip swordAuraSpeechAudioClip;


    protected void Awake() {
        maxHealth = 1200f; // ------------------------------------------------------------------needs to change -------------------------------------------------------------
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
        detectionRadius = 30f; fov.radius = detectionRadius;
        detectionAngle = 145f; fov.angle = detectionAngle;
        fovRaycastHeight = 0.5f;

        speed = 4.5f; agent.speed = speed;
        rotationSpeed = 10f;

        //roar
        roarDuration = 2.183f;

        //fly sword
        swordSummonTrans = transform.Find("SwordSummonPoint").transform;

        //SFX
        audioSources = GetComponents<AudioSource>();
        audioSource = audioSources[0];
        audioSource2 = audioSources[1];

        roarAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Roar");
        attackAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Attack");
        attackSpeechAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Attack Speech");
        hitAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Hit");
        dieAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Die");

        
        attack2AudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Attack2");
        attack2SpeechAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Attack2 Speech");

        attack3AudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Attack3");
        attack3SpeechAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Attack3 Speech");

        rageAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Rage");

        blinkBackAttackBlinkAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Blink");
        blinkBackAttackAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Attack");

        jumpAttackAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Jump Attack");
        jumpAttackSpeechAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Jump Attack Speech");

        flySwordSpeechAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Fly Sword Speech");
        swordAuraSpeechAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Sword Aura Speech");

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
                ScoreManager_new.instance.AddScore(5000); // Adjust points to preference
            }
        }
        //rage reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && !isRaged && currentHealth <= maxHealth/2 && !isRaging && !isCastingRangeSkill){
            Rage();
        }
        //roar reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && !isRoared && !isCastingRangeSkill && !isRoarCoolDown){
            TryRoar();
        }
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= rangeAttackSkillAttackRange && !isRangeAttackSkillCoolDown && isFlySword && !isRaging && !isCastingRangeSkill){
            if(currentHealth <= maxHealth/2){
                FlySwordWithIllusion();   
            }else{
                FlySword(true);
            } 
        }
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= rangeAttackSkillAttackRange && !isRangeAttackSkillCoolDown && isSwordAura && !isRaging && !isCastingRangeSkill){
            if(currentHealth <= maxHealth/2){
                SwordAuraWithIllusion();   
            }else{
                SwordAura(true);
            } 
        }
        //blink back attack reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= meleeAttackSkillAttackRange && !isMeleeAttackSkillCoolDown && isBlinkBackAttack && !isRaging && !isCastingRangeSkill){
            if(currentHealth <= maxHealth/2){
                BlinkBackAttackWithIllusion();   
            }else{
                BlinkBackAttack();
            } 
        }
        //jump attack reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= meleeAttackSkillAttackRange && !isMeleeAttackSkillCoolDown && isJumpAttack && !isRaging && !isCastingRangeSkill){
            if(currentHealth <= maxHealth/2){
                JumpAttackWithIllusion();
            }else{
                JumpAttack(true);
            } 
        }
        //attack reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= attackRange && isAttack && !isRaging && !isCastingRangeSkill){
            if(currentHealth <= maxHealth/2){
                AttackWithIllusion();
            }else{
                Attack(true);
            } 
        }
        //attack2 reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= attack2Range && isAttack2 && !isRaging && !isCastingRangeSkill){
           if(currentHealth <= maxHealth/2){
                Attack2WithIllusion();
            }else{
                Attack2(true);
            } 
        }
        ///attack3 reation check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= attack3Range && isAttack3 && !isRaging && !isCastingRangeSkill){
            if(currentHealth <= maxHealth/2){
                Attack3WithIllusion();
            }else{
                Attack3(true);
            } 
        }
        //Chases reaction check
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && !isRaging && !isCastingRangeSkill){
            Chase();
        }
        //patrolling if nothing happened 
        else if(alive && !isAttacking && !isRoaring && !isHitting && !fov.canSeePlayer && !isRaging && !isCastingRangeSkill){
            Patrol();
            agent.enabled = false;
            isRoared = false;
        }

        //disable the movement when roaring
        if((isDying || isAttacking || isRoaring || isHitting || isRaging || isCastingRangeSkill) && !isResetPosition){
            agent.enabled = false;
        }

        //face to player while attacking
        if(isAttacking || isHitting || isRoaring || isFaceToPlayer || isRaging || isCastingRangeSkill){
            FaceToPlayer();
        }
    }

    //override the Die() funcction to include stop attack2
    protected override void Die()
    {
        base.Die();

        StopAttackLogic(attack2LogicCoroutine);

        StopAttackLogic(attack3LogicCoroutine);

        StopAttackLogic(blinkBackAttackLogicCoroutine);

        StopAttackLogic(jumpAttackLogicCoroutine);
    }

    //override the Hit() funcction to include stop attack2
    protected override void Hit()
    { 
        if(!isRaging && !isCastingRangeSkill){
            base.Hit();

            if(generateIllusionCoroutine != null){StopCoroutine(generateIllusionCoroutine);}

            StopAttack(attack2TimerCoroutine);
            StopAttackLogic(attack2LogicCoroutine);

            StopAttack(attack3TimerCoroutine);
            StopAttackLogic(attack3LogicCoroutine);

            StopAttack(blinkBackAttackTimerCoroutine);
            StopAttackLogic(blinkBackAttackLogicCoroutine);

            StopAttack(jumpAttackTimerCoroutine);
            StopAttackLogic(jumpAttackLogicCoroutine);
        }
    }

    //override the Roar function
    protected override IEnumerator Roaring(){
        SetAnimationActive(baseAnimationState.Roar);
        StartCoroutine(RoarCoolDownTimer());
        juggernautSword.SetActive(true);
        isRoaring = true;
        isRoared = true;
        yield return new WaitForSeconds(roarDuration);
        isRoaring = false;
        juggernautSword.SetActive(false);
    }

    //override the Attack() function
    protected void Attack(bool speech){
        SetAnimationActive(baseAnimationState.Attack);
        if(speech)PlaySFX(attackSpeechAudioClip);
        attackTimerCoroutine = StartCoroutine(AttackTimer(attackDuration));
        attackLogicCoroutine = StartCoroutine(AttackLogic());
        isAttack = false;
        isAttack2 = true;
    }

    //attack with generate illusion function
    protected void AttackWithIllusion(){
        Attack(true);
        generateIllusionCoroutine = StartCoroutine(GenerateIllusion(attackIllusion, attackDuration));
    }

    //override the normal attack logic
    protected override IEnumerator AttackLogic()
    {
       if(alive && isAttacking){
            //deal damage first time
            yield return new WaitForSeconds(attackWindUpDuration);
            PlaySFX2(attackAudioClip);
            isDealingDamage = true;
            StartCoroutine(DealDamageTimer(dealDamageDuration));
            StartCoroutine(DealDamage(attackDamage, attackDistance));

            //deal damage second time
            yield return new WaitForSeconds(attackWindUpDuration2);
            PlaySFX2(attackAudioClip);
            isDealingDamage = true;
            StartCoroutine(DealDamageTimer(dealDamageDuration));
            StartCoroutine(DealDamage(attackDamage, attackDistance));
       }
    }

    //add in new attack2 function
    protected void Attack2(bool speech){
        SetAnimationActive(ExtendedAnimationState.Attack2);
        if(speech)PlaySFX(attack2SpeechAudioClip);
        attack2TimerCoroutine = StartCoroutine(AttackTimer(attack2Duration));
        attack2LogicCoroutine = StartCoroutine(Attack2Logic());
        StartCoroutine(EnableNavMeshAgent(attack2Duration));
        isAttack2 = false;
        isAttack3 = true;
    }

    //attack with generate illusion function
    protected void Attack2WithIllusion(){
        Attack2(true);
        generateIllusionCoroutine = StartCoroutine(GenerateIllusion2(attack2Illusion, attack2Duration));
    }

    //deal damage to player with time checking (Attack 2)
    protected IEnumerator Attack2Logic(){
        yield return new WaitForSeconds(attack2WindUpDuration);
        PlaySFX2(attack2AudioClip);
        isDealingDamage = true;
        StartCoroutine(DealDamageTimer(dealDamageDuration));
        StartCoroutine(DealDamage(attack2Damage, attack2Distance));
        StartCoroutine(SmoothMoveForward(attack2MoveDistance, dealDamageDuration));
    }

    //add in new attack2 function
    protected void Attack3(bool speech){
        SetAnimationActive(ExtendedAnimationState.Attack3);
        if(speech)PlaySFX(attack3SpeechAudioClip);
        attack3TimerCoroutine = StartCoroutine(AttackTimer(attack3Duration));
        attack3LogicCoroutine = StartCoroutine(Attack3Logic());
        StartCoroutine(EnableNavMeshAgent(attack3Duration));
        isAttack3 = false;
        isAttack = true;
    }

    //attack with generate illusion function
    protected void Attack3WithIllusion(){
        Attack3(true);
        generateIllusionCoroutine = StartCoroutine(GenerateIllusion2(attack3Illusion, attack3Duration));
    }

    //deal damage to player with time checking (Attack 2)
    protected IEnumerator Attack3Logic(){
        yield return new WaitForSeconds(attack3WindUpDuration);
        PlaySFX2(attack3AudioClip);
        isDealingDamage = true;
        StartCoroutine(DealDamageTimer(dealDamageDuration));
        StartCoroutine(DealDamageStun(attack3Damage, attack3Distance));
        StartCoroutine(SmoothMoveForward(attack3MoveDistance, dealDamageDuration));
    }
    
    //blink back attack function
    protected void BlinkBackAttack(){
        SetAnimationActive(ExtendedAnimationState.BlinkBackAttack);
        blinkBackAttackTimerCoroutine = StartCoroutine(AttackTimer(blinkBackAttackDuration));
        blinkBackAttackLogicCoroutine = StartCoroutine(BlinkBackAttackLogic());
        StartCoroutine(MeleeAttackSkillCoolDownTimer());
        StartCoroutine(EnableNavMeshAgent(blinkBackAttackDuration));
        isBlinkBackAttack = false;
        isJumpAttack = true;
    }

    protected void BlinkBackAttackWithIllusion(){
        BlinkBackAttack();
        generateIllusionCoroutine = StartCoroutine(GenerateIllusion2(blinkBackAttackIllusion, blinkBackAttackDuration));
    }

    protected IEnumerator BlinkBackAttackLogic(){
        yield return new WaitForSeconds(blinkBackAttackBlinkWindUpDuration);
        BinkToPlayerBack();
        PlaySFX(blinkBackAttackBlinkAudioClip);
        yield return new WaitForSeconds(blinkBackAttackWindUpDuration);
        PlaySFX2(blinkBackAttackAudioClip);
        isDealingDamage = true;
        StartCoroutine(DealDamageTimer(dealDamageDuration));
        StartCoroutine(DealDamage(blinkBackAttackDamage, blinkBackAttackDistance));
    }

    protected void BinkToPlayerBack(){
        transform.position = playerTransform.position - playerTransform.forward * 2f;
    }

    protected void JumpAttack(bool speech){
        SetAnimationActive(ExtendedAnimationState.JumpAttack);
        if(speech)PlaySFX(jumpAttackSpeechAudioClip);
        jumpAttackTimerCoroutine = StartCoroutine(AttackTimer(jumpAttackDuration));
        jumpAttackLogicCoroutine = StartCoroutine(JumpAttackLogic());
        StartCoroutine(MeleeAttackSkillCoolDownTimer());
        StartCoroutine(EnableNavMeshAgent(jumpAttackDuration));
        isBlinkBackAttack = true;
        isJumpAttack = false;
    }

    protected void JumpAttackWithIllusion(){
        JumpAttack(true);
        generateIllusionCoroutine = StartCoroutine(GenerateIllusion2(jumpAttackIllusion, jumpAttackDuration));
        
    }

    protected IEnumerator JumpAttackLogic(){
        Vector3 targetPosition = playerTransform.position;
        StartCoroutine(Jump());
        yield return new WaitForSeconds(jumpAttackWindUpDuration);
        StartCoroutine(ChargeTowardsPlayer(targetPosition));
        PlaySFX2(jumpAttackAudioClip);
        isDealingDamage = true;
        StartCoroutine(DealDamageTimer(dealDamageDuration+ 0.1f));
        StartCoroutine(DealDamageStun(jumpAttackDamage, jumpAttackDistance));
    }

    protected IEnumerator Jump(){
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < jumpAttackWindUpDuration){
            elapsedTime += Time.deltaTime;
            float ratio = elapsedTime / jumpAttackWindUpDuration;
           
            float heightRatio = Mathf.SmoothStep(0f, jumpAttackJumpHeight, ratio);
            transform.position = new Vector3(transform.position.x, startPosition.y + heightRatio, transform.position.z);

            yield return null;
        }
    }

    protected IEnumerator ChargeTowardsPlayer(Vector3 targetPosition){
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < dealDamageDuration)
        {
            elapsedTime += Time.deltaTime;

            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / dealDamageDuration);

            yield return null; 
        }
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
    }

    protected void FlySword(bool speech){
        SetAnimationActive(ExtendedAnimationState.FlySword);
        if(speech)PlaySFX(flySwordSpeechAudioClip);

        StartCoroutine(CastRangeSkillTimer(flySwordDuration));
        StartCoroutine(FlySwordLogic());
        StartCoroutine(RangeAttackSkillCoolDownTimer());

        isFlySword = false;
        isSwordAura = true;
    }

    protected void FlySwordWithIllusion(){
        FlySword(true);
        generateIllusionCoroutine = StartCoroutine(GenerateIllusion2(flySowordIllusion, flySwordDuration));
    }

    protected IEnumerator CastRangeSkillTimer(float duration){
        isCastingRangeSkill = true;
        yield return new WaitForSeconds(duration);
        isCastingRangeSkill = false;
        if(juggernautSword != null){
            juggernautSword.SetActive(false);
        }
    }

    protected IEnumerator FlySwordLogic(){
        yield return new WaitForSeconds(flySwordWindUpDuration);
        SummonSword();
    }

    protected void SummonSword(){
        Instantiate(flySword, swordSummonTrans.position, swordSummonTrans.rotation);
    }

    protected void SwordAura(bool speech){
        SetAnimationActive(ExtendedAnimationState.SwordAura);
        juggernautSword.SetActive(true);
        if(speech)PlaySFX(swordAuraSpeechAudioClip);

        StartCoroutine(CastRangeSkillTimer(swordAuraDuration));
        StartCoroutine(SwordAuraLogic());
        StartCoroutine(RangeAttackSkillCoolDownTimer());

        isFlySword = true;
        isSwordAura = false;
    }

    protected void SwordAuraWithIllusion(){
        SwordAura(true);
        generateIllusionCoroutine = StartCoroutine(GenerateIllusion2(swordAuraIllusion, swordAuraDuration));
    }

    protected IEnumerator SwordAuraLogic(){
        yield return new WaitForSeconds(swordAuraWindUpDuration);
        SummonSwordAura();
    }

    protected void SummonSwordAura(){
        Instantiate(swordAura, swordSummonTrans.position, swordSummonTrans.rotation);
    }

    protected IEnumerator MeleeAttackSkillCoolDownTimer(){
        isMeleeAttackSkillCoolDown = true;
        yield return new WaitForSeconds(meleeAttackSkillCoolDown);
        isMeleeAttackSkillCoolDown = false;
    }

    protected IEnumerator RangeAttackSkillCoolDownTimer(){
        isRangeAttackSkillCoolDown = true;
        yield return new WaitForSeconds(rangeAttackSkillCoolDown);
        isRangeAttackSkillCoolDown = false;

    }

    protected void Rage(){
        SetAnimationActive(ExtendedAnimationState.Rage);
        rageVFX.SetActive(true);
        PlaySFX(rageAudioClip);
        StartCoroutine(RageTimer());
        isRaged = true;
    }

    protected IEnumerator RageTimer(){
        isRaging = true;
        yield return new WaitForSeconds(rageDuration);
        isRaging = false;
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

    //using nav mesh agent to correct the position of jugg after attacks
    protected IEnumerator EnableNavMeshAgent(float delayTime){
        yield return new WaitForSeconds(delayTime);
        isResetPosition = true;
        agent.enabled = true;
        yield return new WaitForSeconds(0.1f);
        isResetPosition = false;
    }

    protected void PlaySFX2(AudioClip audioClip){
        audioSource2.clip = audioClip;
        audioSource2.Play();
    }

    //put the attack2 attack3 state inside the dictionary
    protected override Dictionary<object, string> GetExtendedAnimationParameterNames()
    {
        return new Dictionary<object, string>
        {
            { ExtendedAnimationState.Attack2, "Attack2" },
            { ExtendedAnimationState.Attack3, "Attack3" },
            { ExtendedAnimationState.Rage, "Rage" },
            { ExtendedAnimationState.BlinkBackAttack, "BlinkBackAttack"},
            { ExtendedAnimationState.JumpAttack, "JumpAttack"},
            { ExtendedAnimationState.FlySword, "FlySword"},
            { ExtendedAnimationState.SwordAura, "SwordAura"}
        };
    }

    protected IEnumerator DealDamageStun(int attackDamage, float attackDistance){
        while(isDealingDamage && isAttacking){
                DealDamageStunRayCast(attackDamage, attackDistance);
                yield return null;
        }
    }

    //deal damage to player with ray cast check
    protected virtual void DealDamageStunRayCast(int attackDamage, float attackDistance){
        RaycastHit hit;
        Debug.DrawRay(attackRaycastTransformPosition, transform.forward, Color.red, 5f);
        if (Physics.Raycast(attackRaycastTransformPosition, transform.forward, out hit, attackDistance, targetMask)){
            //if (hit.collider.gameObject == playerTransform.gameObject){
                DealDamageStun(hit, attackDamage);
            //}
        }
    }

    //deal damage to player
    protected virtual void DealDamageStun(RaycastHit hit, int attackDamage){
        PlayerController playerHealth = hit.collider.GetComponent<PlayerController>();
        if (playerHealth != null){
            playerHealth.TakeDamge(attackDamage);
            playerHealth.AddDebuff("Stun");
            isDealingDamage = false;
        }
    }

    protected override void OnDisable(){
        base.OnDisable();
        isAttack = true;
        isAttack2 = false;
        isAttack3 = false;
        isResetPosition = false;
        isMeleeAttackSkillCoolDown = false;
        isBlinkBackAttack = true;
        isJumpAttack = false;
        isRangeAttackSkillCoolDown = false;
        isCastingRangeSkill = false;   
        isFlySword = true;
        isSwordAura = false;
        isRaged = false;
        isRaging = false;
    }


}
