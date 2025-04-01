/*using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{

    private Transform target;
    private NavMeshAgent agent = null;
    private Animator anim;

    private float timeOfLastAttack = 0;
    private ZombieStats stats;
    public int aggroDistance;
    public bool canAttack = true;
    // Start is called before the first frame update
    void Start()
    {
        // GetReferences();
    }

    // Update is called once per frame
    void Update()
    {
        if(canAttack == true)
            MoveToTarget();
    }

    private void MoveToTarget()
    {
        agent.SetDestination(target.position);
        anim.SetFloat("Speed", 1f, 0.3f, Time.deltaTime);

        // Determine if the enemy is close enough to the player
        float distanceToTarget = Vector3.Distance(target.position, transform.position);
        if(distanceToTarget <= agent.stoppingDistance + 0.3)
        {
            anim.SetFloat("Speed", 0f, 0.3f, Time.deltaTime);
            // Attack if within range and time since last attack is sufficient
            if (Time.time >= timeOfLastAttack + stats.attackSpeed)
            {
                timeOfLastAttack = Time.time;
                Shootable targetStats = target.GetComponent<Shootable>();
                Attack(targetStats);
            }
        }
        else
        {
            // Rotate towards movement direction if not moving directly towards the player
            RotateTowardsMovementDirection();
        }
    }
    private void RotateTowardsMovementDirection()
    {
        Vector3 velocity = agent.velocity;
        if (velocity.magnitude > 0.1f) // Check if the enemy is moving
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    private void Attack(Shootable targetStats)
    {
        if (canAttack)
        {
            anim.SetTrigger("Attack");
            stats.DealDamage(targetStats);
        }
    }

    private void GetReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        stats = GetComponent<ZombieStats>();
        target = PlayerController.instance.transform;
    }
}*/
