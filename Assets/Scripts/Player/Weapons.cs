using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Weapons : MonoBehaviour
{

    public int selectedweapon = 0;
    PlayerController playerController;
    public Shield shield;
    public GameObject shuriVisual;
    public Slot slot1, slot2;

    private InventoryController inventory;
    private bool debugMode = false;
    private string[] weapons;
    private string weaponName;
    private int weaponIndex;

    //RunesAxe Stuff
    public float waveSpeed = 10f; // Speed of the slash wave projectile
    public float waveLifetime = 3f; // Time before the projectile disappears
    public int waveDamage = 50; // Damage dealt by the projectile
    //RunesAxe End

    //Lightning Stuff
    public GameObject lightningPrefab;
    public float aoeRadius = 5.0f; // Radius for AOE damage
    public int aoeDamage = 130; // Damage dealt by the lightning strike
    public LayerMask raycastLayerMask;
    //Lightning Stuff End

    //Settings
    float throwCooldown;

    [Header("Throwing")]
    public Camera mainCamera;
    public Transform cam;
    public Transform attackPoint;
    public GameObject daggerObject, shurikenObject;
    
    public float throwForce;    
    public float throwUpwardForce;
    public int weaponslot;

    private GameObject thrownObject;
    private float tempforce;

    bool readyToThrow;

    // Start is called before the first frame update
    void Start()
    {
        tempforce = throwForce;
        readyToThrow = true;
        playerController = GetComponentInParent<PlayerController>();
        inventory = FindObjectOfType<InventoryController>();
        mainCamera = Camera.main;
        weapons = new string[2];
        weapons[0] = null;
        weapons[1] = null;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            debugMode = !debugMode;
            if(debugMode)
                shuriVisual.SetActive(true);
            else shuriVisual.SetActive(false);
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.LogWarning(weapons[weaponIndex] + " " + selectedweapon);
        }

        int previousSelectedWeapon = selectedweapon;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (weaponIndex == 0)
            {
                if (weapons[1] != null)
                {
                    selectedweapon = 1;
                    weaponIndex = 1;
                    weaponName = weapons[1];
                }
                else { }
            }
            else if (weaponIndex == 1)
            {
                if (weapons[0] != null)
                {
                    selectedweapon = 0;
                    weaponIndex = 0;
                    weaponName = weapons[0];
                }
                else { }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!debugMode)
            {
                if (weapons[0] != null)
                {
                    selectedweapon = 0;
                    weaponIndex = 0;
                    weaponName = weapons[0];
                }
                else { }

            }
            else
                selectedweapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!debugMode)
            {
                if (weapons[1] != null)
                {
                    selectedweapon = 1;
                    weaponIndex = 1;
                    weaponName = weapons[1];
                }
                else { }

            }
            else
                selectedweapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            DropWeapon();
        }

        if (debugMode)
        {
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

            if (Input.GetKeyDown(KeyCode.Alpha0))
                selectedweapon = 9;
        }

        if (previousSelectedWeapon != selectedweapon)
            SwapWeapons();
    }

    //Changes weapon based on input/weapon name
    public void SwapWeapons()
    {
        if (debugMode)
        {
            int i = 0;

            foreach (Transform weap in transform)
            {
                if (i == selectedweapon)
                    weap.gameObject.SetActive(true);
                else
                    weap.gameObject.SetActive(false);
                i++;
            }
        }
        else
        {
            foreach (Transform weap in transform)
            {
                if (weap.transform.name == weaponName)
                    weap.gameObject.SetActive(true);
                else
                    weap.gameObject.SetActive(false);
            }
        }
    }

    public void ReplaceWeap(string weap)
    {
        //Debug.Log(weap);

        //Get the right weapon name
        if (weap == "PocketKnife")
            weaponName = "Pocket_Knife";
        else if (weap == "Dagger")
            weaponName = "Dagger";
        else if (weap == "Sword")
            weaponName = "Sword";
        else if (weap == "Axe")
            weaponName = "Axe";
        else if (weap == "ScytheHealth")
            weaponName = "Grim_Reaper_Scythe";
        else if (weap == "Hammer")
            weaponName = "Hammer";
        else if (weap == "Polearm")
            weaponName = "Guan_Dao";
        else if (weap == "RunesAxe")
            weaponName = "Runes_Axe";
        else if (weap == "LightningSword")
            weaponName = "Lightning_Sword";
        else if (weap == "Shuriken")
            weaponName = "Shuriken";
        else
        {
            Debug.LogWarning("Weapon Does not Exist :(");
            return;
        }

        //Get the right index to replace the weapon
        if (weapons[1] != null && weapons[0] != null)
        {
            weapons[weaponIndex] = weaponName;
            SwapWeapons();
        }
        else if (weapons[0] == null)
        {
            weapons[0] = weaponName;
            selectedweapon = 0;
            weaponIndex = 0;
            SwapWeapons();
        }
        else
        {
            weapons[1] = weaponName;
            selectedweapon = 1;
            weaponIndex = 1;
            SwapWeapons();
        }
    }

    //Removing Weapon from weapon list
    public void DropWeapon()
    {
        //Prevents dropping of items from full inventory
        foreach (string weap in weapons)
        {
            if (weap == null)
                return;
        }

        if (weaponIndex == 0)
        {         
            selectedweapon = 1;
            weaponIndex = 1;
            weaponName = weapons[1];
            SwapWeapons();
            weapons[0] = null;
            DropItem(slot1);
        }
        else if (weaponIndex == 1)
        {         
            selectedweapon = 0;
            weaponIndex = 0;
            weaponName = weapons[0];
            SwapWeapons();
            weapons[1] = null;
            DropItem(slot2);
        }
        else
            return;
    }

    //Removing Weapon from Inventory
    private void DropItem(Slot slot)
    {
        //Modified Drop Item Code from Slot.cs
        slot.amount -= 1;
        inventory.isFull[slot.i] = false;
        Spawn itemObject = slot.transform.GetComponentInChildren<Spawn>(true);
        Debug.LogWarning(itemObject.gameObject.name);
        GameObject.Destroy(itemObject.gameObject);

        Transform player = playerController.transform;
        Vector3 playerposition = player.position + Camera.main.transform.forward * 4;
        playerposition.y = player.position.y + 1;
        Instantiate(itemObject.itemPrefab, playerposition, Quaternion.identity);
    }

    public void Idle()
    { 
        WeaponStats weap = GetComponentInChildren<WeaponStats>();
        playerController.Idle();
    }

    public void Walk()
    {
        WeaponStats weap = GetComponentInChildren<WeaponStats>();
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
        
        throwForce = 10f;
        thrownObject = shurikenObject;
        if (readyToThrow)
            playerController.Throw();
    }

    // Special Attacks
    void Dagger()
    {
        throwForce = tempforce;
        thrownObject = daggerObject;
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
        throwForce = 1f;
        thrownObject = shurikenObject;
        if (readyToThrow)
            playerController.ShuriThrow();
    }

    public void Thrown()
    {
        readyToThrow = false;

        // Instantiate object for throwing
        GameObject projectile = Instantiate(thrownObject, attackPoint.position, cam.rotation);

        // Rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // Force
        Vector3 force = cam.transform.forward * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(force, ForceMode.Impulse);

        Invoke(nameof(ResetThrow), throwCooldown);

        Destroy(projectile, 10);
    }

    public void ShuriThrown()
    {
        readyToThrow = false;

        for (int i = -1; i <= 1; i++) // Loop to fire at three angles (-1, 0, 1)
        {
            // Instantiate object for throwing
            GameObject projectile = Instantiate(thrownObject, attackPoint.position, cam.rotation);

            // Modify the rotation based on the angle
            Quaternion angleRotation = Quaternion.Euler(0, i * 30, 0); // Adjust 30 degrees to the left/right
            projectile.transform.rotation = cam.rotation * angleRotation;

            // Rigidbody component
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            // Force
            Vector3 force = projectile.transform.forward * throwForce + projectile.transform.up * throwUpwardForce;

            projectileRb.AddForce(force, ForceMode.Impulse);

            Destroy(projectile, 10); // Destroy after 10 seconds
        }

        // Cooldown
        Invoke(nameof(ResetThrow), throwCooldown);

    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    //Runes Axe Stuff
    public void FireCrescentWave()
    {
        // Create the crescent wave dynamically
        GameObject crescentWave = new GameObject("CrescentWave");

        // Use the player's root to determine the forward direction and spawn position
        Transform playerRoot = transform.root; // Get the player's root object
        Vector3 spawnPosition = playerRoot.position + new Vector3(0f, 1f, 0f) + playerRoot.forward.normalized * 1.5f; // Spawn in front of the player's root
        crescentWave.transform.position = spawnPosition;
    
        // Align the crescent with the player's forward direction and parallel to the ground
        crescentWave.transform.rotation = Quaternion.LookRotation(playerRoot.forward, Vector3.up);

        // Add crescent wave components
        CrescentWave crescentWaveScript = crescentWave.AddComponent<CrescentWave>();
        crescentWaveScript.Initialize(waveSpeed, waveLifetime, waveDamage, raycastLayerMask);
    }


    //Lightning Sword Stuff
    public void TriggerLightningStrike()
    {
        // Get the mouse position in the world
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, raycastLayerMask))
        {
            Vector3 strikePosition = hit.point;

            // Instantiate the lightning effect at the hit position
            GameObject Lightning = Instantiate(lightningPrefab, strikePosition, Quaternion.identity);
            Destroy(Lightning, 5);
            // Apply AOE damage
            ApplyAOEDamage(strikePosition);
        }
    }

    private void ApplyAOEDamage(Vector3 center)
    {
        LayerMask attackLayer;
        attackLayer = LayerMask.GetMask("Hittable");
        Collider[] colliders = Physics.OverlapSphere(center, aoeRadius, attackLayer);
        Debug.Log($"Number of colliders detected in AOE: {colliders.Length}");
        List<GameObject> currentCollisions = new List<GameObject>();

        foreach (Collider c in colliders)
        {
            // Traverse up the hierarchy to find the Rigidbody
            Transform currentTransform = c.transform;
            Rigidbody parentRigidbody = null;

            while (currentTransform != null)
            {
                parentRigidbody = currentTransform.GetComponent<Rigidbody>();
                if (parentRigidbody != null) break;
                currentTransform = currentTransform.parent;
            }

            currentCollisions.Add(currentTransform.gameObject);
            
        }
        foreach (GameObject gObject in currentCollisions)
        {
            if (gObject.transform.TryGetComponent(out EnemyBehaviour T))
            {
                T.TakeDamage(aoeDamage);
            }
        }
    }
}
