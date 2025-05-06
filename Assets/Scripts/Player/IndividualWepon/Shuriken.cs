using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    
    public float speed = 10f; // Speed of the shuriken's movement
    public float rotationSpeed = 30f; // How fast the shuriken adjusts its direction
    public float lifeTime = 5f; // Lifetime of the shuriken before it is destroyed
    public float detectionRadius = 100f; // Radius to detect nearby enemies
    public int damage = 75;

    private Transform target;
    private float timer;
    private bool hitTarget = false;

    void Start()
    {
        timer = lifeTime;

        FindNearestTarget();
    }

    void Update()
    {
        if (target != null)
        {
            // Calculate direction toward the target
            Vector3 direction = (target.position - transform.position).normalized;

            // Smoothly rotate toward the target
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            // Move the shuriken forward
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            // If no target, move forward in the current direction
            transform.position += transform.forward * speed * Time.deltaTime;

            // Continuously check for a new nearest target
            FindNearestTarget();
        }

        // Destroy the shuriken after its lifetime expires
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FindNearestTarget()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRadius, LayerMask.GetMask("Hittable"));
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;
        List<GameObject> currentCollisions = new List<GameObject>();

        foreach (Collider e in enemies)
        {
            if (e == null) return;
            // Traverse up the hierarchy to find the Rigidbody
            Transform currentTransform = e.transform;
            Rigidbody parentRigidbody = null;

            while (currentTransform != null)
            {
                parentRigidbody = currentTransform.GetComponent<Rigidbody>();
                if (parentRigidbody != null) break; // Found the Rigidbody, exit the loop
                currentTransform = currentTransform.parent; // Move up the hierarchy
            }

            Debug.Log(currentTransform.name);
            currentCollisions.Add(currentTransform.gameObject);
        }

        foreach (GameObject gameObj in currentCollisions) {
            if (gameObj == null) return;
            if (gameObj.CompareTag("Enemy")) // Ensure the enemy has the right tag
            {
                float distanceToEnemy = Vector3.Distance(transform.position, gameObj.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = gameObj.transform;
                }
            }
        }

        target = closestEnemy;
    }


    private void OnCollisionEnter(Collision c)
    {
        if (hitTarget)
            return;
        else
            hitTarget = true;

        Debug.Log(c.gameObject.name);

        // Collision logic
        if (c.gameObject.transform.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour T))
        {
            T.TakeDamage(damage);
            Debug.Log("Hit enemy: " + c.gameObject.name);
            Destroy(gameObject); // Destroy shuriken upon impact
        }
    }
}

