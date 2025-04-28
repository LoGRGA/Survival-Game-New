using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float Timer = 5;


    public List<GameObject> pickupItems = new List<GameObject>();

    //Gold Player Inventory
    public int gold;
    public TextMeshProUGUI goldAmountText;
    public TextMeshProUGUI goldAmountText2;




    // Update is called once per frame
    void Update()
    {

        goldAmountText.text = gold + " Gold";
        goldAmountText2.text = gold + " Gold";
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    Timer -= 1 * Time.deltaTime;
        //}

        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    Timer = 5;
        //}

        //if(Timer <= 0)
        //{
        //    Timer = 0;

        //    foreach (var item in pickupItems)
        //    {
        //        if (item != null!)
        //        {
        //            item.GetComponent<Pickup>().PickupItem();
        //        }

        //        pickupItems.Clear();
        //    }

        //}

        //foreach (var item in pickupItems)
        //{
        //    if (item != null!)
        //    {
        //        item.GetComponent<Pickup>().PickupItem();
        //    }


        //}

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PickUp")
        {
            pickupItems.Add(other.gameObject);
        }


    }

    private void OnTriggerExit(Collider other)
    {
        //remove when get away from the item
        pickupItems.Remove(other.gameObject);
    }
}
