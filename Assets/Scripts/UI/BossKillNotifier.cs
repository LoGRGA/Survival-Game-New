using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKillNotifier : MonoBehaviour
{
    public ObjectiveManager objectiveManager;

    public void NotifyBossKilled()
    {
        if (objectiveManager != null)
            objectiveManager.BossKilled();
    }

    // Example: Call this when the boss dies
    void OnDeath()
    {
        NotifyBossKilled();
        Destroy(gameObject);
    }
}

