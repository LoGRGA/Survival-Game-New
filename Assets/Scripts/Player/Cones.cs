using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cones : MonoBehaviour
{
    int selectedCone = 0;
    public Weapons weapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedCone = selectedCone;
        WeaponStats weap = weapon.GetComponentInChildren<WeaponStats>();

        if (weap.transform.name == "Pocket_Knife")
            selectedCone = 0;

        if (weap.transform.name == "Dagger")
            selectedCone = 0;

        if (weap.transform.name == "Sword")
            selectedCone = 1;

        if (weap.transform.name == "Axe")
            selectedCone = 2;

        if (weap.transform.name == "Grim_Reaper_Scythe")
            selectedCone = 2;

        if (weap.transform.name == "Hammer")
            selectedCone = 2;

        if (weap.transform.name == "Guan_Dao")
            selectedCone = 2;

        if (weap.transform.name == "Runes_Axe")
            selectedCone = 2;

        if (weap.transform.name == "Lightning_Sword")
            selectedCone = 2;

        if (weap.transform.name == "Shuriken")
            selectedCone = 2;


        if (previousSelectedCone != selectedCone)
            SwapCones();
    }

    public void SwapCones()
    {

        int i = 0;

        foreach (Transform cone in transform)
        {
            if (i == selectedCone)
                cone.gameObject.SetActive(true);
            else
                cone.gameObject.SetActive(false);
            i++;
        }
    }
}
