using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuggernautBehaviour : EnemyBehaviour
{
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

        speed = 3.5f; agent.speed = speed;
        rotationSpeed = 10f;

        //roar
        roarDuration = 2.183f;

        //SFX
        roarAudioClip = LoadAudioClip("Zombie SFX", "Zombie Roar");
        attackAudioClip = LoadAudioClip("Zombie SFX", "Zombie Attack");
        hitAudioClip = LoadAudioClip("Zombie SFX", "Zombie Hit");
        dieAudioClip = LoadAudioClip("Zombie SFX", "Zombie Die");
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
