using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickRoom2 : MonoBehaviour
{
    private float checkInterval = 0.5f;
    private Collider triggerCollider;
    public GameObject door;
    public LayerMask hittableLayer;

    private bool isCoroutineStart = false;
    // Start is called before the first frame update
    void Start()
    {
        triggerCollider = GetComponent<Collider>();
        StartCoroutine(CheckForHittableObjects());
        isCoroutineStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCoroutineStart)
        {
            StartCoroutine(CheckForHittableObjects());
        }
    }


    private IEnumerator CheckForHittableObjects()
    {
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

            if (colliders.Length <= 3)
            {
                door.SetActive(true);
            }
            else
            {
                door.SetActive(false);
            }
        }
    }
    
    void OnDisable()
    {
        isCoroutineStart = false;
    }
}
