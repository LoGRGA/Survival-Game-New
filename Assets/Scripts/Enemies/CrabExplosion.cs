using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabExplosion : MonoBehaviour
{
    //explosion damage
    private int damage = 10;

    // Start is called before the first frame update
    void Start()
    {
        ApplyDamageToAllInRange();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        Destroy(gameObject);
    }

    private void DealDamage(Collider other){
        if(other.tag=="Player"){
            PlayerController player = other.GetComponent<PlayerController>();
            player.TakeDamge(damage);
            player.AddDebuff("Burn");
        }
        else{
            EnemyBehaviour enemy = other.GetComponentInParent<EnemyBehaviour>();
            if(enemy != null){
                enemy.TakeDamage(damage);
            }
            
        }
    }
}