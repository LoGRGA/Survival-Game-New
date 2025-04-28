using System.Collections;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ObjectiveSwitcher : MonoBehaviour
{
    public GameObject objective1UI; // Drag your Objective 1 UI object here
    public GameObject objective2UI; // Drag your Objective 2 UI object here
    public float delayBeforeSwitch = 3f; // 3 second delay

    public void SwitchObjectives()
    {
        StartCoroutine(SwitchAfterDelay());
    }

    private IEnumerator SwitchAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeSwitch);

        if (objective1UI != null)
            objective1UI.SetActive(false);

        if (objective2UI != null)
            objective2UI.SetActive(true);

        Debug.Log("Objectives switched!");
    }
}

