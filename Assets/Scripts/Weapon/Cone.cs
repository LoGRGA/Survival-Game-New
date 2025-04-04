using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
    // Declare and initialize a new List of GameObjects called currentCollisions.
    List<GameObject> currentCollisions = new List<GameObject>();

    void OnCollisionEnter(Collision c)
    {
        if(!currentCollisions.Contains(c.gameObject))
            // Add the GameObject collided with to the list.
            currentCollisions.Add(c.gameObject);
    }

    void OnCollisionStay(Collision c)
    {
        if (!currentCollisions.Contains(c.gameObject))
            // Add the GameObject collided with to the list.
            currentCollisions.Add(c.gameObject);
    }

    void OnCollisionExit(Collision c)
    {
        // Remove the GameObject collided with from the list.
        currentCollisions.Remove(c.gameObject);
    }

    public List<GameObject> GetArray()
    {
        return currentCollisions;
    }
}
