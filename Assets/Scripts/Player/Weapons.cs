using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Weapons : MonoBehaviour
{
    public int selectedweapon = 0;
    PlayerController playerController;
    public Shield shield;


    //Settings
    float throwCooldown;

    [Header("Throwing")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject daggerObject;
    public float throwForce;
    public float throwUpwardForce;

    public int weaponslot;

    bool readyToThrow;

    // Start is called before the first frame update
    void Start()
    {
        readyToThrow = true;
        playerController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedweapon;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedweapon = 0;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            selectedweapon = 1;


        //Below are all temporary
        if (Input.GetKeyDown(KeyCode.Alpha3))
            selectedweapon = 2;

        if (Input.GetKeyDown(KeyCode.Alpha4))
            selectedweapon = 3;

        if (Input.GetKeyDown(KeyCode.Alpha5))
            selectedweapon = 4;

        if (Input.GetKeyDown(KeyCode.Alpha6))
            selectedweapon = 5;

        if (Input.GetKeyDown(KeyCode.Alpha7))
            selectedweapon = 6;

        if (Input.GetKeyDown(KeyCode.Alpha8))
            selectedweapon = 7;

        if (Input.GetKeyDown(KeyCode.Alpha9))
            selectedweapon = 8;

        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //    selectedweapon = 9;

        if (previousSelectedWeapon != selectedweapon)
            SwapWeapons();
    }
    public void SwapWeapons()
    {
        int i = 0;

        foreach(Transform weap in transform)
        {
            if (i == selectedweapon)
                weap.gameObject.SetActive(true);
            else
                weap.gameObject.SetActive(false);
            i++;
        }
    }

    public void Idle()
    { 
        WeaponStats weap = GetComponentInChildren<WeaponStats>();

        if (weap.transform.name == "Pocket_Knife" || weap.transform.name == "Dagger" || weap.transform.name == "Shuriken")
        {
            playerController.Idle();
        }
        else if (weap.transform.name == "Sword" || weap.transform.name == "Axe" || weap.transform.name == "Hammer" ||
                weap.transform.name == "Runes_Axe" || weap.transform.name == "Lightning_Sword")
        {
            playerController.SwordIdle();
        }
        else if (weap.transform.name == "Guan_Dao" || weap.transform.name == "Shuriken")
        {
            playerController.THIdle();
        }
        else
            playerController.Idle();
    }

    public void Walk()
    {
        WeaponStats weap = GetComponentInChildren<WeaponStats>();

        if (weap.transform.name == "Pocket_Knife" || weap.transform.name == "Dagger" || weap.transform.name == "Shuriken")
        {
            playerController.Walk();
        }
        else if (weap.transform.name == "Sword" || weap.transform.name == "Axe" || weap.transform.name == "Hammer" ||
                weap.transform.name == "Runes_Axe" || weap.transform.name == "Lightning_Sword")
        {
            playerController.ShieldIdle();
        }
        else if (weap.transform.name == "Guan_Dao" || weap.transform.name == "Shuriken")
        {
            playerController.THIdle();
        }
        else
            playerController.Walk();
    }

    public void Attack()
    {
        WeaponStats weap = GetComponentInChildren<WeaponStats>();

        if (weap.transform.name == "Pocket_Knife")
            PocketKnife();
        else if (weap.transform.name == "Dagger")
            PocketKnife();
        else if (weap.transform.name == "Sword")
            Sword();
        else if (weap.transform.name == "Axe")
            Axe();
        else if (weap.transform.name == "Grim_Reaper_Scythe")
            Grim();
        else if (weap.transform.name == "Hammer")
            Hammer();
        else if (weap.transform.name == "Guan_Dao")
            GDao();
        else if (weap.transform.name == "Runes_Axe")
            RAxe();
        else if (weap.transform.name == "Lightning_Sword")
            LSword();
        else if (weap.transform.name == "Shuriken")
            Shuriken();
    }

    public void SpecialAttack()
    {
        WeaponStats weap = GetComponentInChildren<WeaponStats>();

        if (weap.transform.name == "Pocket_Knife")
            PocketKnife();
        else if (weap.transform.name == "Dagger")
            Dagger();
        else if (weap.transform.name == "Sword")
            SwordSA();
        else if (weap.transform.name == "Axe")
            AxeSA();
        else if (weap.transform.name == "Grim_Reaper_Scythe")
            GrimHeavy();
        else if (weap.transform.name == "Hammer")
            HammerHeavy();
        else if (weap.transform.name == "Guan_Dao")
            GDaoHeavy();
        else if (weap.transform.name == "Runes_Axe")
            RAxeHeavy();
        else if (weap.transform.name == "Lightning_Sword")
            LSwordHeavy();
        else if (weap.transform.name == "Shuriken")
            ShurikenHeavy();
    }

    // Basic Attacks
    void PocketKnife()
    {
        playerController.Attack();
    }

    void Sword()
    {
        playerController.SwordAttack();
    }

    void Axe()
    {
        playerController.AxeAttack();
    }

    void Grim()
    {
        playerController.GrimAttack();
    }

    void Hammer()
    {
        playerController.HammerAttack();
    }

    void GDao()
    {
        playerController.GDaoAttack();
    }

    void RAxe()
    {
        playerController.RAxeAttack();
    }

    void LSword()
    {
        playerController.LSwordAttack();
    }

    void Shuriken()
    {

    }

    // Special Attacks
    void Dagger()
    {
        if (readyToThrow)
            playerController.Throw();
    }

    void SwordSA()
    {
        playerController.SwordHeavy();
    }

    void AxeSA()
    {
        playerController.AxeHeavy();
    }

    void GrimHeavy()
    {
        playerController.GrimHeavy();
    }

    void HammerHeavy()
    {
        playerController.HammerHeavy();
    }

    void GDaoHeavy()
    {
        playerController.GDaoHeavy();
    }

    void RAxeHeavy()
    {
        playerController.RAxeHeavy();
    }

    void LSwordHeavy()
    {
        playerController.LSwordHeavy();
    }

    void ShurikenHeavy()
    {

    }

    public void Thrown()
    {
        readyToThrow = false;

        //Quaternion rot = new Quaternion(4.038f, 81.843f, -90.581f, 0);

        // Instantiate object for throwing
        GameObject projectile = Instantiate(daggerObject, attackPoint.position, cam.rotation);

        // Rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // Force
        Vector3 force = cam.transform.forward * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(force, ForceMode.Impulse);

        Invoke(nameof(ResetThrow), throwCooldown);

        Destroy(projectile, 10);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}
