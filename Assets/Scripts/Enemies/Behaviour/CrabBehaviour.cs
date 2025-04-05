using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBehaviour : EnemyBehaviour
{
    //summon variable
    protected float summonWindUpDuration = 1f;
    public GameObject crabPrefab;
    protected float throwForce = 7f;

    protected void Awake() {
        maxHealth = 50f; // ------------------------------------------------------------------needs to change -------------------------------------------------------------
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
        attackWindUpDuration = 0.65f; // from frme 0 to 15
        dealDamageDuration = 0.1f; //from frame 15 to 18

        attackDuration = 1.26f;
        attackDamage = 5;
        attackRaycastHeight = -0.5f;

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

    //override the Die() funcction to include stop attack2
    protected override void Die()
    {
        base.Die();
        StartCoroutine(SummonLogic());
    }

    protected IEnumerator SummonLogic(){
        yield return new WaitForSeconds(summonWindUpDuration);
        ThrowCrab();
    }

    protected void ThrowCrab(){
        float[] Angles = { 0f, -45f, 45f };

        foreach (float Angle in Angles)
        {
            //calculate the horizontal direction
            Vector3 horizontalDir = Quaternion.Euler(0, Angle, 0) * transform.forward;

            Vector3 throwDir = Quaternion.AngleAxis(-45, transform.right) * horizontalDir;

            //instatiate crab prefab
            GameObject crabInstance = Instantiate(crabPrefab, transform.position, transform.rotation);

            Rigidbody rb = crabInstance.GetComponent<Rigidbody>();

            if(Angle == 0){
                StartCoroutine(ChangeKinematic(rb,2f));
            }else{
                StartCoroutine(ChangeKinematic(rb,1.5f));
            }
            StartCoroutine(ChangeCollision(rb));
            if (rb != null)
            {
                rb.velocity = throwDir * throwForce;
            }
        }
    }

    protected IEnumerator ChangeKinematic(Rigidbody rb, float time){
        rb.isKinematic = false;
        yield return new WaitForSeconds(time);
        rb.isKinematic = true;
    }

    protected IEnumerator ChangeCollision(Rigidbody rb){
        rb.detectCollisions = false;
        yield return new WaitForSeconds(0.5f);
        rb.detectCollisions = true;
    }

    protected IEnumerator FaceToPlayerWhenSummoned(){
        isFaceToPlayer = true;
        yield return new WaitForSeconds(2f);
        isFaceToPlayer = false;
    }
}
