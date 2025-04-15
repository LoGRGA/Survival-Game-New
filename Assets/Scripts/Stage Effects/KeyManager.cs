using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance;

    [HideInInspector] public KeyObjectInteraction chosenObject;
    private bool keyAssigned = false;
    private List<KeyObjectInteraction> allObjects = new List<KeyObjectInteraction>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Register(KeyObjectInteraction obj)
    {
        allObjects.Add(obj);

        // Assign key randomly once all are registered (optional if you want to delay)
        if (!keyAssigned && allObjects.Count > 1) // 2+ objects ensures randomness
        {
            int randomIndex = Random.Range(0, allObjects.Count);
            chosenObject = allObjects[randomIndex];
            keyAssigned = true;
            Debug.Log("Key assigned to: " + chosenObject.gameObject.name);
        }
    }

    public bool IsKeyHolder(KeyObjectInteraction obj)
    {
        return obj == chosenObject;
    }
}
