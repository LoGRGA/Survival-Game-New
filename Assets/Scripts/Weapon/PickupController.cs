using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public WeaponStats weap;
    public Rigidbody rb;
    public BoxCollider col;
    public Transform player, weapContainer, cam;

    public float pickupRange = 2f;
    public float dropForce = 1f;
    public float dropUpwardForce = 1f;

    public bool equipped;
    public static bool slotFull;

    // Start is called before the first frame update
    void Start()
    {
        if (!equipped)
        {
            weap.enabled = false;
            rb.isKinematic = false;
            col.isTrigger = false;
        }
        if (!equipped)
        {
            weap.enabled = true;
            rb.isKinematic = true;
            col.isTrigger = true;
        }
    }

    void Drop()
    {
        equipped = false;
        slotFull = false;

        // Make weapon not part of weapon (null)
        transform.SetParent(null);

        rb.isKinematic = false;
        col.isTrigger = false;

        rb.AddForce(cam.forward * dropForce, ForceMode.Impulse);
        rb.AddForce(cam.up * dropUpwardForce, ForceMode.Impulse);

        // Adding random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        //Enable weapon
        weap.enabled = false;
    }

    void Pickup()
    {
        //equipped = true;
        //slotFull = true;

        //// Make the weapon a child in camera, right hand and weapon
        //transform.SetParent(weapContainer);
        //transform.localPosition = Vector3.zero;
        //transform.localRotation = Quaternion.identity;
        //transform. localScale = Vector3.one;

        //rb.isKinematic = true;
        //col.isTrigger = true;

        //// Enable weapon
        //weap.enabled = true;

        foreach (Transform weap in weapContainer)
        {
            if (transform.name == weap.name)
            {
                weap.gameObject.SetActive(true);
                break;
            }
            else
                weap.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Check if player is in range for pickup
        Vector3 distToPlayer = player.position - transform.position;
        if (!equipped && distToPlayer.magnitude <= pickupRange && Input.GetKeyDown(KeyCode.E) && !slotFull)
            Pickup();

        if (equipped && Input.GetKeyDown(KeyCode.G))
            Drop();
    }
}
