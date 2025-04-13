using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class PudgeNotMovingBehaviour : PudgeBehaviour
{
    // Update is called once per frame
    protected override void Update()
    {
        EnemyUpdate();

                //death check
        if(currentHealth <= 0 && !isDying){
            Die();
            // Javier Addition: ADDING SCORE WHEN ZOMBIE DIES
            if (ScoreManager_new.instance != null)
            {
                ScoreManager_new.instance.AddScore(1000); // Adjust points as needed
            }
        }

        //hook reation check
        if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && !isReleasingHook && !isRetractingHook && !isHookCoolDown && !isCastingRot){
            Hook();
            agent.enabled = false;
        }
        //stay idle if hook is in cd
        else if(alive && !isAttacking && !isRoaring && !isHitting && fov.canSeePlayer && !isReleasingHook && !isRetractingHook && !isCastingRot){
            Idle();
        }
        //stay idle if nothing happened 
        else if(alive && !isAttacking && !isRoaring && !isHitting && !fov.canSeePlayer){
            Idle();
            agent.enabled = false;
            isRoared = false;
        }

        //disable the movement when roaring
        if(isDying || isAttacking || isRoaring || isHitting || isCastingRot){
            agent.enabled = false;
        }

        //face to player while attacking
        if(isAttacking || isHitting || isRoaring || isFaceToPlayer || fov.canSeePlayer){
            FaceToPlayer();
        }

    }
}
