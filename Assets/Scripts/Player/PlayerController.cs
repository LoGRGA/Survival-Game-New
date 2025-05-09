using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class PlayerController : FPSInput
{
    //HAZIQ Health Warning Panel 
    //public GameObject healthWarningPanel;
    //public TMP_Text healthWarningText;
    //private bool hasShownWarning = false;
    //HAZIQ

    //
    //------------------------------------------------------------------------------------junjie add debuff------------------------------------------------------------------------------
    private List<string> debuffList = new List<string>();

    private Vector3 originalPos;
    private bool isInvincible, isGrimSpeed, isLightningCooldown, gDaoHeavy, smashDelay;
    private Cone cone;
    private Cones cones;
    private IEnumerator attackCoroutine;
    private PlayerController playerController;

    //Debuff Booleans
    public bool isPoisoned, isBurned, isBleeding, isStunned;

    public GameObject[] weapons;

    [Header("Camera")]
    public Camera cam;

    [Header("Stats")]
    public int maxHealth = 100;
    public TMP_Text healthText;
    public Slider slider;

    //HAZIQ CHANGE from int to public int
    public int currentHealth;
    //HAZIQ

    GameObject currentWeap;
    Weapons weap;
    Shield shield;

    [Header("Models")]
    public Transform RH;
    public Transform LH;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // get the components
        charController = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponentsInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        weap = GetComponentInChildren<Weapons>();
        shield = GetComponentInChildren<Shield>();
        cones = GetComponentInChildren<Cones>();

        currentHealth = maxHealth;
        healthText.SetText(currentHealth.ToString());

        slider.maxValue = maxHealth;
        slider.value = currentHealth; 

        weapons = new GameObject[2];

        currentWeap = weapons[0];

        attackLayer = LayerMask.GetMask("Hittable");

        originalPos = gameObject.transform.position;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        slider.maxValue = maxHealth;
        // initiate attack on fire button press
        if (Input.GetButtonDown("Fire1"))
        {
            weap.Attack();
        }
        // initiate Special Attack on RMB
        if (Input.GetButtonDown("Fire2"))
        {
            weap.SpecialAttack();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            playerController.AddDebuff("Bleed");
        }

        SetAnimations();

        Debuff();
        SceneChange();
    }

    // Animation
    public const string IDLE = "Idle";
    public const string SHIELDI = "Shield Idle";
    public const string SWORDI = "Sword Idle";
    public const string THIDLE = "ThIdle";
    public const string WALK = "Walk";
    public const string ATTACK1 = "Attack 1";
    public const string ATTACK2 = "Attack 2";
    public const string SWORDATTACK = "Sword Attack";
    public const string SWORDHEAVY = "Sword Heavy";
    public const string AXEATTACK = "Axe Attack";
    public const string AXEHEAVY = "Axe Heavy";
    public const string SLAM = "Hammer Heavy";
    public const string SPIN = "Spin";
    public const string SPIN1 = "SpinMirror";
    public const string THROW = "Throw";
    
    public const string BLOCk = "Block";

    string currentAnimationState;

    public void ChangeAnimationState(string newState)
    {
        // STOP THE SAME ANIMATION FROM INTERRUPTING WITH ITSELF //
        if (currentAnimationState == newState) return;


        //Debug.Log(currentAnimationState);
        // PLAY THE ANIMATION //
        currentAnimationState = newState;
        foreach (Animator anim in animator) {
            anim.CrossFadeInFixedTime(currentAnimationState, 0.2f);
        }
    }

    void SetAnimations()
    {
        // If player is not attacking
        if (!attacking)
        {
            LH.transform.localPosition = Vector3.zero + new Vector3(-0.133f, -1.441f, 0.126f);
            RH.transform.localPosition = Vector3.zero + new Vector3(0.05f, -1.46f, 0.7f); // Reset Arm animation
            LH.transform.localRotation = new Quaternion(-0.03524f, 0, 0.00504f, 0.99937f);
            RH.transform.localRotation = new Quaternion(-0.16044f, 0, 0.02294f, 0.98678f);
            if (movement.x == 0 && movement.z == 0)
            { weap.Idle(); }
            else
            { weap.Walk(); }
        }
    }

    public void Idle()
    {
        ChangeAnimationState(IDLE);
    }

    public void SwordIdle()
    {
        ChangeAnimationState(SHIELDI);
    }

    public void ShieldIdle()
    {
        ChangeAnimationState(SHIELDI);
    }

    public void THIdle()
    {
        ChangeAnimationState(THIDLE);
    }

    public void Walk()
    {
        ChangeAnimationState(WALK);
    }

    public void Throw()
    {
        if (!readyToAttack || attacking) return;
        attacking = true;
        readyToAttack = false;

        // Set Arm animation to throw at the right position
        RH.transform.position = RH.position - new Vector3(0, 0.4f, 0);
        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(Thrown), attackDelay);

        ChangeAnimationState(THROW);
    }

    public void ShuriThrow()
    {
        if (!readyToAttack || attacking) return;
        attacking = true;
        readyToAttack = false;

        // Set Arm animation to throw at the right position
        RH.transform.position = RH.position - new Vector3(0, 0.4f, 0);
        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(ShuriThrown), attackDelay);

        ChangeAnimationState(THROW);
    }

    // Attack

    [HideInInspector] public float attackDistance = 3f;
    [HideInInspector] public float attackDelay = 0.4f;
    [HideInInspector] public float attackSpeed = 1f;
    [HideInInspector] public int attackDamage = 25;
    [HideInInspector] public float hitSoundDelay;
    private float basicSpeed;
    private int basicDamage;

    
    [HideInInspector] public LayerMask attackLayer;

    [Header("Attacking")]
    public GameObject hitEffect;
    public AudioClip swordSwing;
    public AudioClip hitSound;
    public AudioClip grimStartSFX;
    public AudioClip grimEndSFX;
    public AudioClip thunder;
    public AudioClip smash;

    bool attacking = false;
    bool readyToAttack = true;
    int attackCount;

    public bool Attacking()
    {
        if (!readyToAttack || attacking) return false;

        readyToAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);
        Invoke(nameof(HitAudio), hitSoundDelay);

        return true;
    }

    public void HitAudio()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(swordSwing);
    }

    IEnumerator SmashAudio()
    {
        foreach (Animator anim in animator)
        {
            anim.speed = 0.5f;
        }
        ChangeAnimationState(SLAM);
        yield return new WaitForSeconds(0.8f);

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(smash);
        foreach (Animator anim in animator)
        {
            anim.speed = 1.0f;
        }
    }

    // Basic Attack
    public void Attack()
    {
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;

        bool attack = Attacking();
        if (!attack) 
            return;

        if (attackCount == 0)
        {
            ChangeAnimationState(ATTACK1);
            attackCount++;
        }
        else
        {
            RH.transform.localPosition =  new Vector3(-0.45f, -1.46f, 0.6f);
            RH.transform.Rotate(0, -20f, 0, Space.Self);
            ChangeAnimationState(ATTACK2);
            attackCount = 0;
        }
    }

    public void SwordAttack()
    {
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;
        attackDamage = 50;

        bool attack = Attacking();
        if (!attack)
            return;

        RH.transform.localPosition = Vector3.zero + new Vector3(0.05f, -1.46f, 0.0f);
        ChangeAnimationState(SWORDATTACK);
    }

    public void AxeAttack()
    {
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;
        attackSpeed = 2.5f;
        attackDelay = 2f;
        attackDamage = 60;
        hitSoundDelay = 2.5f;

        bool attack = Attacking();
        if (!attack)
            return;

        RH.transform.localPosition = Vector3.zero + new Vector3(0.05f, -1.46f, 0.2f);
        ChangeAnimationState(AXEATTACK);
    }

    public void GrimAttack()
    {
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;
        attackDamage = 60;

        bool attack = Attacking();
        if (!attack)
            return;

        RH.transform.localPosition = Vector3.zero + new Vector3(0.05f, -1.46f, 0.2f);
        ChangeAnimationState(SWORDATTACK);
    }

    public void HammerAttack()
    {
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;
        attackSpeed = 3f;
        attackDelay = 2.5f;
        attackDamage = 70;
        hitSoundDelay = 2.5f;

        bool attack = Attacking();
        if (!attack)
            return;

        RH.transform.localPosition = Vector3.zero + new Vector3(0.6f, -1.46f, 0f);
        ChangeAnimationState(AXEHEAVY);
    }

    public void GDaoAttack()
    {
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;
        attackDamage = 60;

        bool attack = Attacking();
        if (!attack)
            return;

        RH.transform.localPosition = Vector3.zero + new Vector3(0.6f, -1.46f, 0f);
        ChangeAnimationState(SWORDATTACK);
    }

    public void RAxeAttack()
    {
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;
        attackSpeed = 2.5f;
        attackDelay = 2f;
        attackDamage = 150;
        hitSoundDelay = 2.5f;

        bool attack = Attacking();
        if (!attack)
            return;

        RH.transform.localPosition = Vector3.zero + new Vector3(0.6f, -1.46f, 0f);
        ChangeAnimationState(AXEATTACK);
    }

    public void LSwordAttack()
    {
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;
        attackDamage = 100;

        bool attack = Attacking();
        if (!attack)
            return;

        RH.transform.localPosition = Vector3.zero + new Vector3(0.6f, -1.46f, 0f);
        ChangeAnimationState(SWORDATTACK);
    }

    public void Thrown()
    {
        weap.Thrown();
    }

    public void SwordHeavy()
    {
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;
        attackSpeed = 1.5f;
        attackDelay = 1f;
        attackDamage = 75;
        hitSoundDelay = 1f;

        bool attack = Attacking();
        if (!attack)
            return;

        RH.transform.localPosition = Vector3.zero + new Vector3(0.3f, -1.46f, 0.2f);
        ChangeAnimationState(SWORDHEAVY);
    }

    public void AxeHeavy()
    {
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;
        attackSpeed = 2.5f;
        attackDelay = 2f;
        attackDamage = 100;
        hitSoundDelay = 2f;

        bool attack = Attacking();
        if (!attack)
            return;

        RH.transform.localPosition = Vector3.zero + new Vector3(0.6f, -1.46f, 0f);
        ChangeAnimationState(AXEHEAVY);
    }

    public void GrimHeavy()
    {
        if(isGrimSpeed)
            return;

        isGrimSpeed = true;
        speed += 5f;

        StartCoroutine(GrimSpeed());
    }

    public void HammerHeavy()
    {
        if (smashDelay)
            return;
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;
        attackSpeed = 1.0f;
        attackDelay = 0.8f;
        attackDamage = 150;
        hitSoundDelay = 1.0f;

        cones.SelectCone(3);
        bool attack = Attacking();
        if (!attack)
            return;

        //LH.transform.Rotate(-30f, 0f, 0f, Space.Self);
        //RH.transform.Rotate(-30f, 0f, 0f, Space.Self);
        RH.transform.localPosition = Vector3.zero + new Vector3(0.3f, -1.46f, 0f);
        StartCoroutine(SmashAudio());
        StartCoroutine(SmashDelay());
    }

    public void GDaoHeavy()
    {
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;
        attackSpeed = 0.5f;
        attackDelay = 0.2f;
        attackDamage = 30;
        hitSoundDelay = 0.2f;

        // Start attacking as long as RMB is held
        if (!gDaoHeavy)
        {
            gDaoHeavy = true;
            StartCoroutine(gDaoLoop());
        }

    }

    public void RAxeHeavy()
    {
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;
        attackSpeed = 2.5f;
        attackDelay = 2f;
        attackDamage = 50;
        hitSoundDelay = 2f;

        bool attack = Attacking();
        if (!attack)
            return;

        RH.transform.localPosition = Vector3.zero + new Vector3(0.6f, -1.46f, 0f);
        ChangeAnimationState(AXEHEAVY);
        StartCoroutine(RAxeProjectile());
    }

    public void LSwordHeavy()
    {
        if (isLightningCooldown)
            return;

        weap.TriggerLightningStrike();
        StartCoroutine(LightningCooldown());
    }

    public void ShuriThrown()
    {
        weap.ShuriThrown();
    }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    void AttackRaycast()
    {
        cone = GetComponentInChildren<Cone>(false);

        foreach (GameObject gObject in cone.GetArray())
        {
            if(gObject.IsDestroyed()) continue;
            //print(gObject.name);
            
            if (Physics.Linecast(cam.transform.position, gObject.transform.position, out RaycastHit hits, attackLayer))
            {
                HitTarget(hits.point);

                if (hits.transform.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour T))
                {
                    T.TakeDamage(attackDamage);
                    GrimHeal(T);
                }
            }
        }


        attackDamage = basicDamage;
    }

    void HitTarget(Vector3 pos)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(hitSound);

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(GO, 20);
    }

    // Health Logic
    public void TakeDamge(int amount)
    {
        if (!isInvincible)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);  // Prevent health from going below 0
            healthText.SetText(currentHealth.ToString());  // Update health text display
            slider.value = currentHealth;  // Update slider value
        }
        else 
        {
            ShieldStat shieldstat = GetComponentInChildren<ShieldStat>();
            shieldstat.BlockDamage(amount);
        }
/*
        //Haziq Warning Panel
        if (!hasShownWarning && currentHealth <= maxHealth / 2)
        {
            ShowHealthWarning("Your health is at 50 percent!\n Time to restock some health potion at my shop!\nCome!\nI will also put in a discount price for the potion!!");
            hasShownWarning = true;
        }
        //Haziq
*/
    }
    public void Heal(int amount)
    {
        
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);  // Prevent health from going above maxHealth
            healthText.SetText(currentHealth.ToString());  // Update health text display
            slider.value = currentHealth;  // Update slider value
/*
        //HAZIQ
        // Reset warning Panel if health goes above 50%
        if (currentHealth > maxHealth / 2)
        {
            hasShownWarning = false;
        }
        //HAZIQ
*/
    }

    private void GrimHeal(EnemyBehaviour T)
    {
        WeaponStats weapstat = GetComponentInChildren<WeaponStats>();
        if (weapstat.transform.name == "Grim_Reaper_Scythe")
        {
            Heal(4);
            maxHealth += 1;
        }
    }

    public void InvincibleSwap(string bol)
    {
        if (bol == "true")
        {
            isInvincible = true;
        }
        else
        {
            isInvincible = false;
        }
    }

    //Scene Change
    void SceneChange()
    {
        if (Input.GetKey(KeyCode.Y))
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "GameScene") // Change the name to the demo scene name
            {
                SceneManager.LoadScene("SampleScene");
            }
            else if (scene.name == "SampleScene")
            {
                SceneManager.LoadScene("GameScene"); // Change the name to the demo scene name
            }
        }
    }

    //Debuffs

    private void Debuff()
    {
        foreach (string d in debuffList)
        {
            switch (d)
            {
                case "Poison":

                    if (isPoisoned)
                        StopCoroutine(PoisonDegen(10));
                    isPoisoned = true;
                    StartCoroutine(PoisonDegen(10));
                    break;
                case "Burn":
                    if (isBurned)
                        StopCoroutine(Burned(3));
                    isBurned = true;
                    StartCoroutine(Burned(3));
                    break;
                case "Bleed":
                    if (isBleeding)
                        StopCoroutine(Bleeding(20));
                    isBleeding = true;
                    StartCoroutine(Bleeding(20));
                    break;
                case "Stun":
                    if (isStunned)
                        StopCoroutine(Stun(3));
                    isStunned = true;
                    StartCoroutine(Stun(3));
                    break;
                default:
                    Debug.LogWarning("Debuff does not exist");
                    break;
            }
            
        }
        debuffList.Clear();
    }

    IEnumerator PoisonDegen(int degenCount)
    {
        for (int degenCounter = degenCount; degenCounter > 0; degenCounter--)
        {
            if (degenCounter != 0)
            {
                //TakeDamge(3);
                speed -= 3;
                yield return new WaitForSeconds(1.0f);
                speed += 3;
                Debug.Log("Poison Counter left = " + degenCounter);
            }
        }
        isPoisoned = false;
    }

    IEnumerator Burned(int burnCount)
    {
        for (int burnCounter = burnCount; burnCounter > 0; burnCounter--)
        {
            if (burnCounter != 0)
            {
                TakeDamge(5);
                yield return new WaitForSeconds(3.0f);
                Debug.Log("Burn Counter left = " + burnCounter);
            }
        }
        isBurned = false;
    }

    IEnumerator Bleeding(int bleedCount)
    {
        for (int bleedCounter = bleedCount; bleedCounter > 0; bleedCounter--)
        {
            if (bleedCounter != 0)
            {
                speed -= 1;
                TakeDamge(1);
                yield return new WaitForSeconds(0.5f);
                speed += 1;
                Debug.Log("Bleed Counter left = " + bleedCounter);
            }
        }
        isBleeding = false;
    }

    IEnumerator Stun(int stunCount)
    {
        // Disable player controls during stun
        if (playerController != null)
            playerController.enabled = false;

        for (int stunCounter = stunCount; stunCounter > 0; stunCounter--)
        {
            if (stunCounter != 0)
            {
                yield return new WaitForSeconds(1f);
                Debug.Log("Stun Counter left = " + stunCounter);
            }
        }
        // Re-enable player controls after stun ends
        if (playerController != null)
            playerController.enabled = true;

        isStunned = false;
    }

    IEnumerator GrimSpeed()
    {
        if(grimStartSFX)
            audioSource.PlayOneShot(grimStartSFX);

        yield return new WaitForSeconds(20);
        isGrimSpeed = false;
        speed -= 5;

        if (grimEndSFX)
            audioSource.PlayOneShot(grimEndSFX);
    }

    IEnumerator LightningCooldown()
    {
        if (thunder)
        {
            audioSource.pitch = Random.Range(0.9f, 1.2f);
            audioSource.PlayOneShot(thunder);
        }

        isLightningCooldown = true;

        yield return new WaitForSeconds(7f);

        isLightningCooldown = false;
    }

    IEnumerator RAxeProjectile()
    {
        yield return new WaitForSeconds(2);

        weap.FireCrescentWave();
    }

    IEnumerator SmashDelay()
    {
        smashDelay = true;

        yield return new WaitForSeconds(3);

        smashDelay = false;
    }

    IEnumerator gDaoLoop()
    {
        int count = 0;

        foreach (Animator anim in animator)
        {
            anim.speed = 1.4f;
        }

        while (Input.GetButton("Fire2"))
        {
            bool attack = Attacking();
            if (attack)
            {
                if (count == 0) 
                {
                    ChangeAnimationState(SPIN);
                    count++;
                }

                else if (count == 1)
                {
                    ChangeAnimationState(SPIN1);
                    count--;
                }
            }

            yield return new WaitForSeconds(0.5f);
        }

        //Stop attacking when Fire2 is released
        foreach (Animator anim in animator)
        {
            anim.speed = 1.0f;
        }
        gDaoHeavy = false;
        ChangeAnimationState(IDLE);
    }

    //junjie add
    public void ChangePlayerPosition(Vector3 position){
        transform.position = position;
    }

    //------------------------------------------------------------------------------------junjie add debuff------------------------------------------------------------------------------
    public void AddDebuff(string debuff){
        debuffList.Add(debuff);
    }
/*
    //HAZIQ Add Warning Panel 
    private void ShowHealthWarning(string message)
    {
        healthWarningText.text = message;
        healthWarningPanel.SetActive(true);
        //StartCoroutine(HideHealthWarningAfterDelay(5f));
        StopAllCoroutines(); // Stop any previous typing coroutines
        StartCoroutine(TypeTextEffect(message));
        StartCoroutine(HideHealthWarningAfterDelay(8f));
    }

    //Type of write effect of the text for the warning panel
    private IEnumerator TypeTextEffect(string message)
    {
        healthWarningText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            healthWarningText.text += letter;
            yield return new WaitForSeconds(0.030f); // Speed of typing (0.03 = fast, 0.1 = slower)
        }
    }

    private IEnumerator HideHealthWarningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        healthWarningPanel.SetActive(false);
    }

    //HAZIQ
*/
}
