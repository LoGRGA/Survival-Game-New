using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3BossRoom : MonoBehaviour
{
    private float checkInterval = 0.5f;
    private Collider triggerCollider;
    public GameObject door;
    public GameObject targetObject;

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
                transform.rotation
            );

            bool targetFound = false;

            foreach (Collider col in colliders)
            {
                if (col.gameObject == targetObject)
                {
                    targetFound = true;
                    break;
                }
            }

            door.SetActive(!targetFound);
        }
    }
}
