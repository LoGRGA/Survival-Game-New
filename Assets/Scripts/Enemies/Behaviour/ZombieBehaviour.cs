using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZombieBehaviour : EnemyBehaviour
{
    protected void Awake() {
        maxHealth = 150f; // ------------------------------------------------------------------needs to change -------------------------------------------------------------
    }
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hipsTransform = transform.Find("mixamorig5:Hips").transform;

        //initiate enemy attributes
        currentHealth = maxHealth;
        hitable = true;
        hitCoolDown = 5f;
        isHitting = false;
        hitDuration = 2f / 1.5f;
        disappearDuration = 4.56f;

        attackRange = 2.5f;
        attackDistance = 2.5f;
        attackWindUpDuration = 1f / 1.2f;
        dealDamageDuration = 0.15f / 1.2f;

        attackDuration = 2.6f / 1.2f;
        attackDamage = 5;
        attackRaycastHeight = 0.2f;

        //fov
        detectionRadius = 20f; fov.radius = detectionRadius;
        detectionAngle = 145f; fov.angle = detectionAngle;
        fovRaycastHeight = 0.5f;

        speed = 3.5f; agent.speed = speed;
        rotationSpeed = 10f;

        //roar
        roarDuration = 2f / 1.2f;

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
            // Javier Addition: ADDING SCORE WHEN ZOMBIE DIES
            if (ScoreManager_new.instance != null)
            {
                ScoreManager_new.instance.AddScore(10); // Adjust points as needed
            }
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
    //protected override void FixedUpdate()
    //{
    //    base.FixedUpdate();
    //}
}
