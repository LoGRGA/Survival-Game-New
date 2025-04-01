using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBehaviour : EnemyBehaviour
{
    protected void Awake() {
        maxHealth = 100f; // ------------------------------------------------------------------needs to change -------------------------------------------------------------
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

        attackRange = 3f;
        attackDistance = 3f;
        attackWindUpDuration = 0.5f; // from frme 0 to 15
        dealDamageDuration = 0.1f; //from frame 15 to 18

        attackDuration = 1f;
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
        roarAudioClip = LoadAudioClip("Crab SFX", "Crab Roar");
        attackAudioClip = LoadAudioClip("Crab SFX", "Crab Attack");
        hitAudioClip = LoadAudioClip("Crab SFX", "Crab Hit");
        dieAudioClip = LoadAudioClip("Crab SFX", "Crab Die");
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
}
