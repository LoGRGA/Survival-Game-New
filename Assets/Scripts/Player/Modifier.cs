using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Modifier : MonoBehaviour
{
    public GameObject player;
    public GameObject[] stages;
    public GameObject[] weapons;
    public GameObject[] items;
    public Transform[] trans;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropWeapon(string weaponName){
        foreach(GameObject weapon in weapons){
            if(weapon.name == weaponName){
                Instantiate(weapon, player.transform.position + player.transform.forward*3 + player.transform.up, player.transform.rotation);
            }
        }
    }

    public void DropItem(string itemName){
        foreach(GameObject item in items){
            if(item.name == itemName){
                Instantiate(item, player.transform.position + player.transform.forward*3 + player.transform.up*0.5f, player.transform.rotation);
            }
        }
    }

    public void TPtoStage1Boss(){
        player.transform.position = trans[0].position;
        foreach(GameObject stage in stages){
            if(stage.name == "Stage1") stage.SetActive(true);
            else stage.SetActive(false);
        }
    }
    
    public void TPtoStage2Statue(){
        player.transform.position = trans[1].position;
        foreach(GameObject stage in stages){
            if(stage.name == "Stage2") stage.SetActive(true);
            else stage.SetActive(false);
        }
    }

    public void TPtoStage2Boss()
    {
        player.transform.position = trans[2].position;
        foreach (GameObject stage in stages)
        {
            if (stage.name == "Stage2") stage.SetActive(true);
            else stage.SetActive(false);
        }
    }

    public void TPtoStage3Boss(){
        player.transform.position = trans[3].position;
        foreach(GameObject stage in stages){
            if(stage.name == "Stage3") stage.SetActive(true);
            else stage.SetActive(false);
        }
    }

    public void TPtoCureRoom(){
        player.transform.position = trans[4].position;
        foreach(GameObject stage in stages){
            if(stage.name == "Cure Room") stage.SetActive(true);
            else stage.SetActive(false);
        }
    }


}
