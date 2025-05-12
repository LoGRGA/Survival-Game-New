using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireBehaviour : EnemyBehaviour
{
    //attack counter
    protected float attackCounter = 0;

    //invisible
    protected bool isInvisible = true;
    protected Transform skin;
    public VampireKnife knife;

    //footprint
    protected float time = 0;
    protected bool isLeft = true;
    protected Transform leftFootPrint;
    protected Transform rightFootPrint;
    public GameObject leftFootPrintGameObj;
    public GameObject rightFootPrintGameObj;




    protected void Awake() {
        maxHealth = 150f; // ------------------------------------------------------------------needs to change -------------------------------------------------------------
    }
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        hipsTransform = transform.Find("mixamorig:Hips").transform;

        //get the skin and knife game object
        skin = transform.Find("Vampire");

        skin.gameObject.SetActive(!isInvisible);

        //footprint
        leftFootPrint = transform.Find("left foot print");
        rightFootPrint = transform.Find("right foot print");


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
        attackDamage = 10;
        attackRaycastHeight = 0.2f;

        //fov
        detectionRadius = 20f; fov.radius = detectionRadius;
        detectionAngle = 145f; fov.angle = detectionAngle;
        fovRaycastHeight = 0.6f;

        speed = 4f; agent.speed = speed;
        rotationSpeed = 10f;

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
            // Javier Addition: ADDING SCORE WHEN ZOMBIE DIES
            if (ScoreManager_new.instance != null)
            {
                ScoreManager_new.instance.AddScore(300); // Adjust points to preference
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
        //stay idle if nothing happened 
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

        //checking the invisibility
        if(!alive || distanceToPlayer <= attackRange || isHitting){
            isInvisible = false;
            skin.gameObject.SetActive(!isInvisible);
        }else{
            isInvisible = true;
            skin.gameObject.SetActive(!isInvisible);
        }


        //increase the time variable
        time += Time.deltaTime;

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

        if(time >= 0.5){
            GenerateFootPrint();
            time = 0;
        }
    }

    protected override void DealDamage(RaycastHit hit, int attackDamage){
        PlayerController playerHealth = hit.collider.GetComponent<PlayerController>();
        if (playerHealth != null){
            playerHealth.TakeDamge(attackDamage);
            playerHealth.AddDebuff("Bleed");
            isDealingDamage = false;
        }
    }

    protected override void Attack()
    {
        base.Attack();
        attackCounter++;
    }

    protected override IEnumerator AttackLogic(){
        if(alive && isAttacking){
            yield return new WaitForSeconds(attackWindUpDuration);
            if(attackCounter > 1){
                PlaySFX(attackAudioClip);
            }
            
            isDealingDamage = true;
            StartCoroutine(DealDamageTimer(dealDamageDuration));
            StartCoroutine(DealDamage(attackDamage, attackDistance));
        }
    }

    public bool GetIsInvisible(){
        return isInvisible;
    }

    protected void GenerateFootPrint(){
        if(isLeft){
            Instantiate(leftFootPrintGameObj,leftFootPrint.transform.position,transform.rotation);
            isLeft = false;
        }else{
            Instantiate(rightFootPrintGameObj,rightFootPrint.transform.position,transform.rotation);
            isLeft = true;
        }
    }
}
