using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;
    
    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    private EnemyBehaviour enemy;

    private Coroutine fovCoroutine;

    private void Start(){
        playerRef = GameObject.FindGameObjectWithTag("Player");
        fovCoroutine = StartCoroutine(FOVRoutine());

        enemy = GetComponent<EnemyBehaviour>();
    }

    private void Update(){
        if(fovCoroutine == null){
            fovCoroutine = StartCoroutine(FOVRoutine());
        }
    }

    private IEnumerator FOVRoutine(){
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while(true){
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck(){
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        //Debug.Log("track player");

        if(rangeChecks.Length != 0){
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, directionToTarget) < angle / 2){
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(enemy.GetFOVRaycastTransformPosition(), directionToTarget, distanceToTarget, obstructionMask)){
                    canSeePlayer = true;
                }
                else{
                    canSeePlayer = false;
                }
            }
            else{
                canSeePlayer = false;
            }
        }
        else if(canSeePlayer){
            canSeePlayer = false;
        }
    }

    public Vector3 GetHeightOffset()
    {
        if (enemy != null)
        {
            return enemy.GetFOVRaycastTransformPosition();
        }
        return new Vector3(0,0,0);
    }

    protected virtual void OnDisable(){
        fovCoroutine = null;
    }
}

//reference: https://www.youtube.com/watch?v=j1-OyLo77ss
