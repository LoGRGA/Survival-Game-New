using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabExplosion : MonoBehaviour
{
    //explosion damage
    private int damage = 5;
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        ApplyDamageToAllInRange();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

/*
    private void OnTriggerEnter(Collider other)
    {


        if(other.tag=="Player"){
            PlayerController player = other.GetComponent<PlayerController>();
            player.TakeDamge(damage);
            Debug.Log("hit player");
        }
        else{
            EnemyBehaviour enemy = other.GetComponentInParent<EnemyBehaviour>();
            enemy.TakeDamage(damage);
            Debug.Log("hit enemy");
        }
    }
    
*/
    private void ApplyDamageToAllInRange()
    {
        HashSet<GameObject> affectedObjects = new HashSet<GameObject>();

        // get all the collider in the range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);
        
        foreach (var hitCollider in hitColliders)
        {
            GameObject obj = hitCollider.gameObject;
            if (!affectedObjects.Contains(obj))
            {
                affectedObjects.Add(obj); 
                DealDamage(hitCollider);
            }
        }

        Debug.Log("counter= " + counter);
        Destroy(gameObject);
    }

    private void DealDamage(Collider other){
        if(other.tag=="Player"){
            PlayerController player = other.GetComponent<PlayerController>();
            player.TakeDamge(damage);
            Debug.Log("hit player");
            counter++;
        }
        else{
            EnemyBehaviour enemy = other.GetComponentInParent<EnemyBehaviour>();
            if(enemy != null){
                enemy.TakeDamage(damage);
                Debug.Log("hit enemy");
                counter++;
            }
            
        }
    }
}
