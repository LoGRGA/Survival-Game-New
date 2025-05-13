using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2BossRoom : MonoBehaviour
{
    private float checkInterval = 0.5f;
    private Collider triggerCollider;
    public GameObject door;
    public LayerMask hittableLayer;

    // Start is called before the first frame update
    void Start()
    {
        triggerCollider = GetComponent<Collider>();
        StartCoroutine(CheckForHittableObjects());
    }

    // Update is called once per frame
    void Update()
    {
    }


    private IEnumerator CheckForHittableObjects(){
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            Bounds colliderBounds = triggerCollider.bounds;
    
            Collider[] colliders = Physics.OverlapBox(
                colliderBounds.center,   
                colliderBounds.extents,  
                transform.rotation,   
                hittableLayer
            );

            if (colliders.Length == 0){
                door.SetActive(true);
            }
            else{
                door.SetActive(false);
            }
        }
    }
}
