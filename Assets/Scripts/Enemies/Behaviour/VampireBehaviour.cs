using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireBehaviour : EnemyBehaviour
{
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
        hitDuration = 2.15f;
        disappearDuration = 3f;

        attackRange = 3f;
        attackDistance = 3f;
        attackWindUpDuration = 0.9f;
        dealDamageDuration = 0.1f;

        attackDuration = 2.4f;
        attackDamage = 5;
        attackRaycastHeight = 0.2f;

        //fov
        detectionRadius = 20f; fov.radius = detectionRadius;
        detectionAngle = 145f; fov.angle = detectionAngle;
        fovRaycastHeight = 0.6f;

        speed = 3.5f; agent.speed = speed;
        rotationSpeed = 10f;

        //roar
        //roarDuration = 2f / 1.2f;

        //SFX
        //roarAudioClip = LoadAudioClip("Vampire SFX", "Vampire Roar");
        attackAudioClip = LoadAudioClip("Vampire SFX", "Vampire Attack");
        hitAudioClip = LoadAudioClip("Vampire SFX", "Vampire Hit");
        dieAudioClip = LoadAudioClip("Vampire SFX", "Vampire Die");
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
            Idle();
            //Patrol();
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

    //override the chase method as vampire will never roar
    protected override void Chase()
    {
        SetAnimationActive(baseAnimationState.Move);

        //AI auto find path to player
        agent.enabled = true;
        Vector3 targetPosition = playerTransform.position;
        agent.destination = targetPosition;
        FaceToPlayer();
    }
    
    protected void Idle(){
        SetAnimationActive(baseAnimationState.Idle);
    }
}
