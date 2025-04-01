using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    public int damage;
    private bool hitTarget = false;
    void Start()
    {   
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision c)
    {
        if (hitTarget)
            return;
        else
            hitTarget = true;

        //Debug.Log(c.gameObject.name);

        if (c.gameObject.transform.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour T))
        {
            T.TakeDamage(damage);
        }

        // Allow projectile to stick to surface
        rb.isKinematic = true;
        transform.SetParent(c.transform);
        transform.LookAt(transform.parent.position);
        transform.Rotate(0f, 0f, 180f, Space.Self);
    }

    void Update()
    {
        if (rb.isKinematic == false)
            transform.Rotate(-540 * Time.deltaTime, 0, 0);
        
    }
}