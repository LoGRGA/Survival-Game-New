using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggernautBehaviour : EnemyBehaviour
{
    //Special Attack vaaraible
    protected float attackWindUpDuration2 = 0.4f; //from 9 to 21, 30fps

    //illusion gameobjects
    public GameObject attackIllusion;

    //Coroutine variable
    //protected Coroutine attackTimerCoroutine;
    //protected Coroutine
    protected Coroutine generateIllusionCoroutine;



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
        roarAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Roar");
        attackAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Attack");
        hitAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Hit");
        dieAudioClip = LoadAudioClip("Juggernaut SFX", "Juggernaut Die");
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
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && distanceToPlayer <= attackRange){
            AttackWithIllusion();
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

    }

    //override the Hit() funcction to include stop attack2
    protected override void Hit()
    { 
        base.Hit();

        StopCoroutine(generateIllusionCoroutine);
    }

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

    protected void AttackWithIllusion(){
        base.Attack();
        generateIllusionCoroutine = StartCoroutine(GenerateIllusion(attackIllusion, attackDuration));
    }

    protected IEnumerator GenerateIllusion(GameObject gameObject, float delaytime){
        yield return new WaitForSeconds(delaytime);
        Instantiate(gameObject, playerTransform.position + playerTransform.forward * 2f, transform.rotation);
    }


}
