using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabSmallBehaviour : EnemyBehaviour
{
    //explosion
    protected float explosionRange = 2f;

    protected void Awake() {
        maxHealth = 25f; // ------------------------------------------------------------------needs to change -------------------------------------------------------------
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

        attackRange = 2f;
        attackDistance = 2f;
        attackWindUpDuration = 0.65f; // from frme 0 to 15
        dealDamageDuration = 0.1f; //from frame 15 to 18

        attackDuration = 1.26f;
        attackDamage = 5;
        attackRaycastHeight = 0f;

        //fov
        detectionRadius = 20f; fov.radius = detectionRadius;
        detectionAngle = 145f; fov.angle = detectionAngle;
        fovRaycastHeight = 0.4f;

        speed = 3.5f; agent.speed = speed;
        rotationSpeed = 10f;

        //roar
        roarDuration = 3.3f / 2f;

        //SFX
        roarAudioClip = LoadAudioClip("Crab SFX", "Crab Roar");
        attackAudioClip = LoadAudioClip("Crab SFX", "Crab Attack");
        hitAudioClip = LoadAudioClip("Crab SFX", "Crab Hit");
        dieAudioClip = LoadAudioClip("Crab SFX", "Crab Die");

        //
        StartCoroutine(FaceToPlayerWhenSummoned());
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
            Attack();
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

    protected IEnumerator FaceToPlayerWhenSummoned(){
        isFaceToPlayer = true;
        yield return new WaitForSeconds(2f);
        isFaceToPlayer = false;
    }

    protected override void Die()
    {
        base.Die();

    }
}
