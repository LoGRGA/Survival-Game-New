using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{   
    //animator
    protected Animator animator;
    protected enum baseAnimationState{Idle, Roar, Move, Attack, Hit, Die};
    protected virtual Dictionary<baseAnimationState, string> GetBaseAnimationParameterNames(){
        return new Dictionary<baseAnimationState, string>
        {
            { baseAnimationState.Idle, "Idle" },
            { baseAnimationState.Roar, "Roar" },
            { baseAnimationState.Move, "Move" },
            { baseAnimationState.Attack, "Attack" },
            { baseAnimationState.Hit, "Hit" },
            { baseAnimationState.Die, "Die" }
        };
    }

    //attributes relate to player
    protected Transform playerTransform;
    protected float distanceToPlayer;
    protected Vector3 directionToPlayer;
    protected Vector3 playerDirection;
    protected Quaternion targetRotation;
    protected LayerMask targetMask;
    //Rigidbody rb;

    //attributes relate to enemy itself
    protected bool alive = true;
    protected bool isDying = false;
    protected float maxHealth;
    [SerializeField]protected float currentHealth;

    //hit related
    protected bool hitable;
    protected float hitCoolDown;
    protected bool isHitting;
    protected float hitDuration;
    protected float disappearDuration;

    //attack related
    protected float attackRange;
    protected float attackDistance;
    protected float attackWindUpDuration;
    protected float dealDamageDuration;
    protected float attackDuration;
    protected bool isDealingDamage = false;
    protected bool isAttacking = false;
    protected int attackDamage;
    protected Vector3 attackRaycastTransformPosition;
    protected float attackRaycastHeight;
    protected Transform hipsTransform;

    //patrol
    protected float obstacleRange = 5f;
    protected Quaternion direction;
    protected bool isIdling = true;
    protected bool isRotating = false;
    protected bool isMoving = false;
    protected float idleDuration = 3f;
    protected float moveDuration = 2f;
    
    //roar related
    protected float roarDuration;
    protected bool isRoaring = false;
    protected bool isRoared = false;
    protected float roarCoolDown = 15f;
    public bool isRoarCoolDown = false;

    //fov related
    protected FieldOfView fov; 
    protected float detectionRadius;
    protected float detectionAngle;
    protected Vector3 fovRaycastTransformPosition;
    protected float fovRaycastHeight;

    //AI
    protected NavMeshAgent agent;
    protected float speed;
    protected float rotationSpeed;
    protected bool isFaceToPlayer = false;

    //Coroutine variable
    protected Coroutine attackTimerCoroutine;
    protected Coroutine attackLogicCoroutine;
    protected bool isFaceToPlayerCoroutine = false;
    protected Coroutine idleCoroutine;
    protected bool isIdleCoroutine = false;
    protected Coroutine moveCoroutine;
    protected bool isMoveCoroutine = false;
    protected Coroutine roarCoroutine;

    //SFX
    protected AudioSource audioSource;
    protected AudioClip roarAudioClip;
    protected AudioClip attackAudioClip;
    protected AudioClip hitAudioClip;
    protected AudioClip dieAudioClip;
    protected AudioClip zombieRoarAudioClip;

    //For finalboss hpcheck use 
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        //attack raycast targetmask
        targetMask = LayerMask.GetMask("Player", "Obstacle");
        
        //FOV
        fov = GetComponent<FieldOfView>();

        //SFX
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected virtual void Update(){
        EnemyUpdate();
    }

    // Make the Update function individualy, so grandchild class can directly use it
    protected void EnemyUpdate(){
        //checking the distance between player
        distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        directionToPlayer = playerTransform.position - transform.position;
        attackRaycastTransformPosition = hipsTransform.position + new Vector3(0, attackRaycastHeight, 0);
        fovRaycastTransformPosition = hipsTransform.position + new Vector3(0, fovRaycastHeight, 0);
    }

    //die function
    protected virtual void Die(){
        alive = false;
        isDying = true;
        PlaySFX(dieAudioClip);
        SetAnimationActive(baseAnimationState.Die);
        StartCoroutine(DestroyWithDelay());
        StopAttackLogic(attackLogicCoroutine);
    }
    
    //destroy the enemy after die animation is done
    protected IEnumerator DestroyWithDelay(){
        yield return new WaitForSeconds(disappearDuration);
        Destroy(gameObject);
    }

    //take damage from player
    public void TakeDamage(int amount){
        currentHealth -= amount;
        //Debug.Log("Enemy Health = " + currentHealth);
        if(alive && hitable){
            Hit();
            agent.enabled = false;
        }else if(alive && !isFaceToPlayerCoroutine){
            StartCoroutine(IsFaceToPlayer());
        }
    }

    protected IEnumerator IsFaceToPlayer(){
        isFaceToPlayer = true;
        isFaceToPlayerCoroutine = true;
        yield return new WaitForSeconds(1f);
        isFaceToPlayer = false;
        isFaceToPlayerCoroutine = false;
    }

    //controll hit logic
    protected virtual void Hit(){
        PlaySFX(hitAudioClip);
        hitable = false;
        StartCoroutine(IsHittingTimer());
        StartCoroutine(HitCoolDownTimer());
        StopAttack(attackTimerCoroutine);
        StopAttackLogic(attackLogicCoroutine);
        StopRoar();
    }

    //controll isHitting variable
    protected IEnumerator IsHittingTimer(){
        isHitting = true;
        SetAnimationActive(baseAnimationState.Hit);
        yield return new WaitForSeconds(hitDuration);
        isHitting = false;
    }

    //controll the hit cool down time
    protected IEnumerator HitCoolDownTimer(){
        yield return new WaitForSeconds(hitCoolDown);
        hitable = true;
    }

    //stop the attack coroutin while hit
    protected void StopAttack(Coroutine coroutine){
        if (coroutine != null){
            StopCoroutine(coroutine);
            isAttacking = false;
        }
    }

    //stop the attack logic coroutin while hit
    protected void StopAttackLogic(Coroutine coroutine){
        if(coroutine != null){
            StopCoroutine(coroutine);
            isDealingDamage = false;
        }
    }

    protected void StopRoar(){
        if(roarCoroutine != null){
            StopCoroutine(roarCoroutine);
            roarCoroutine = null;
            isRoaring = false;
        }
    }

    //attack player
    protected virtual void Attack(){
        SetAnimationActive(baseAnimationState.Attack);
        attackTimerCoroutine = StartCoroutine(AttackTimer(attackDuration));
        attackLogicCoroutine = StartCoroutine(AttackLogic());
    }

    //controll isAttacking variable based on attackDuration
    protected virtual IEnumerator AttackTimer(float attackDuration){
        isAttacking = true;
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }

    //deal damage to player with time checking
    protected virtual IEnumerator AttackLogic(){
        if(alive && isAttacking){
            yield return new WaitForSeconds(attackWindUpDuration);
            PlaySFX(attackAudioClip);
            isDealingDamage = true;
            StartCoroutine(DealDamageTimer(dealDamageDuration));
            StartCoroutine(DealDamage(attackDamage, attackDistance));
        }
    }

    protected IEnumerator DealDamageTimer(float dealDamageDuration){
        yield return new WaitForSeconds(dealDamageDuration);
        isDealingDamage = false;
    }

    protected IEnumerator DealDamage(int attackDamage, float attackDistance){
        while(isDealingDamage && isAttacking){
                DealDamageRayCast(attackDamage, attackDistance);
                yield return null;
            }
    }

    //deal damage to player with ray cast check
    protected virtual void DealDamageRayCast(int attackDamage, float attackDistance){
        RaycastHit hit;
        Debug.DrawRay(attackRaycastTransformPosition, transform.forward, Color.red, 5f);
        if (Physics.Raycast(attackRaycastTransformPosition, transform.forward, out hit, attackDistance, targetMask)){
            //if (hit.collider.gameObject == playerTransform.gameObject){
                DealDamage(hit, attackDamage);
            //}
        }
    }

    //deal damage to player
    protected virtual void DealDamage(RaycastHit hit, int attackDamage){
        PlayerController playerHealth = hit.collider.GetComponent<PlayerController>();
        if (playerHealth != null){
            playerHealth.TakeDamge(attackDamage);
            isDealingDamage = false;
        }
    }

    //chase player
    protected virtual void Chase(){
        if(!isRoared && !isRoarCoolDown){
            PlaySFX(roarAudioClip);
            roarCoroutine = StartCoroutine(Roaring());
        }else{
            SetAnimationActive(baseAnimationState.Move);

            //AI auto find path to player
            agent.enabled = true;
            Vector3 targetPosition = playerTransform.position;
            agent.destination = targetPosition;
            FaceToPlayer();
        }
    }

    protected void TryRoar(){
        if(!isRoared && !isRoarCoolDown){
            PlaySFX(roarAudioClip);
            roarCoroutine = StartCoroutine(Roaring());
        }
    }

    protected virtual IEnumerator Roaring()
    {
        SetAnimationActive(baseAnimationState.Roar);
        StartCoroutine(RoarCoolDownTimer());
        isRoaring = true;
        isRoared = true;
        yield return new WaitForSeconds(roarDuration);
        isRoaring = false;
    }

    protected virtual IEnumerator RoarCoolDownTimer(){
        isRoarCoolDown = true;
        yield return new WaitForSeconds(roarCoolDown);
        isRoarCoolDown = false;
    }

    //patrol logic
    protected void Patrol(){
        Ray ray = new Ray(attackRaycastTransformPosition, transform.forward);
        Debug.DrawRay(attackRaycastTransformPosition, transform.forward, Color.green, 10f);
        RaycastHit hit;

        if(isIdling && !isIdleCoroutine){
            Idle();
            idleCoroutine = StartCoroutine(IdleTimer());
            float angle = UnityEngine.Random.Range(-110.0f, 110.0f);
            direction = Quaternion.LookRotation(Quaternion.Euler(0.0f, angle, 0.0f) * transform.forward);
        }else if(isRotating && !isMoving){
            Idle();
            isRotating = true;

            // rotate the agent over several frames
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                direction, rotationSpeed * Time.deltaTime);

            // if the agent within a certain angle of the correct direction
            if (Quaternion.Angle(transform.rotation, direction) < 1.0f)
            {
                isRotating = false;
                isMoving = true;
                isIdling = false;
            }
        }else if(isMoving){
            SetAnimationActive(baseAnimationState.Move);
            if(!isMoveCoroutine){
                moveCoroutine = StartCoroutine(MoveTimer());
            }
            transform.Translate(0, 0, speed * Time.deltaTime);
        }

        //collision avoidance
        if(Physics.SphereCast(ray, 0.75f, out hit, obstacleRange, targetMask)){
            // if the collision is within the collision avoidance range
            if ((hit.distance < obstacleRange)  && !isRotating){
                float angle = UnityEngine.Random.Range(-110.0f, 110.0f);
                direction = Quaternion.LookRotation(Quaternion.Euler(0.0f, angle, 0.0f) * transform.forward);

                if(idleCoroutine != null && moveCoroutine != null){
                    StopCoroutine(idleCoroutine);
                    isIdleCoroutine = false;
                    StopCoroutine(moveCoroutine);                
                    isMoveCoroutine = false;
                }   

                isIdling = false;
                isMoving = false;
                isRotating = true;
            }
        }
    }

    protected IEnumerator IdleTimer(){
        isIdleCoroutine = true;
        yield return new WaitForSeconds(idleDuration);
        isIdleCoroutine = false;
        isIdling = false;
        isRotating = true;
    }

    protected IEnumerator MoveTimer(){
        isMoveCoroutine = true;
        yield return new WaitForSeconds(moveDuration);
        isMoveCoroutine = false;
        isMoving = false;
        isIdling = true;
    }

    protected Dictionary<object, string> GetCombinedAnimationParameters(){
        var baseDictionary = GetBaseAnimationParameterNames();
        var extendedDictionary = GetExtendedAnimationParameterNames();

        var combinedDictionary = new Dictionary<object, string>();
        foreach (var kvp in baseDictionary)
        {
            combinedDictionary[kvp.Key] = kvp.Value;
        }
        foreach (var kvp in extendedDictionary)
        {
            if (!combinedDictionary.ContainsKey(kvp.Key))
            {
                combinedDictionary.Add(kvp.Key, kvp.Value);
            }
        }

        return combinedDictionary;
    }

    protected virtual Dictionary<object, string> GetExtendedAnimationParameterNames()
    {
        return new Dictionary<object, string>();
    }

    //set all animation state to false except the parm animation state to true
    protected void SetAnimationActive(object state)
    {
        var combinedAnimationParameters = GetCombinedAnimationParameters();

        foreach (var kvp in combinedAnimationParameters)
        {
            //if (animator.HasParameter(kvp.Value))
            if (HasParameter(animator, kvp.Value))
            {
                animator.SetBool(kvp.Value, false);
            }
        }

        if (combinedAnimationParameters.ContainsKey(state) && HasParameter(animator, combinedAnimationParameters[state]))
        {
            animator.SetBool(combinedAnimationParameters[state], true);
        }
        else
        {
            Debug.LogWarning($"Invalid animation state or parameter: {state}");
        }
    }

    //check if animator has particular animation state inside
    protected bool HasParameter(Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
            {
                return true;
            }
        }
        return false;
    }

    //face to direction to player
    protected void FaceToPlayer(){
        //ignore y-axis
        directionToPlayer.y = 0f; 
        
        //check if it's valid direction
        if (directionToPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            // using Slerp ahcheive roation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    //idle function
    protected virtual void Idle(){
        SetAnimationActive(baseAnimationState.Idle);
    }

    //SFX
    public static UnityEngine.AudioClip LoadAudioClip(string folder, string audioClip){
        return Resources.Load<AudioClip>("Audio/Enemy Audio/"+ folder + "/" + audioClip);
    }

    protected void PlaySFX(AudioClip audioClip){
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    protected void StopSFX(AudioClip audioClip){
        audioSource.clip = audioClip;
        audioSource.Stop();
    }

    //get Enemies max health
    public float GetEnemyMaxHealth(){
        return maxHealth;
    }

    //get Enemies current health
    public float GetEnemyCurrentHealth(){
        return currentHealth;
    }

    public Vector3 GetFOVRaycastTransformPosition(){
        return fovRaycastTransformPosition;
    }

    public float GetFOVRaycastHeight(){
        return fovRaycastHeight;
    }

    protected virtual void OnDisable() {
        StopAllCoroutines();
        isDealingDamage = false;
        isAttacking = false;
        isIdling = true;
        isRotating = false;
        isMoving = false;
        isRoaring = false;
        isRoared = false;
        isRoarCoolDown = false;
        isFaceToPlayer = false;
        isFaceToPlayerCoroutine = false;
        isIdleCoroutine = false;
        isMoveCoroutine = false;
        hitable = true;
        isHitting = false;
        fov.canSeePlayer = false;
    }
}
